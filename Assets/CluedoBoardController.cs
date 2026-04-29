using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Main controller for a Cluedo (Clue) board game in Unity.
/// Manages game state, player turns, movement, accusations, and suggestions.
/// </summary>
public class CluedoBoardController : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  Enumerations
    // ─────────────────────────────────────────────

    public enum Suspect
    {
        MissScarlett, ColMustard, MrsWhite,
        RevGreen, MrsPeacock, ProfPlum
    }

    public enum Weapon
    {
        Candlestick, Knife, LeadPipe,
        Revolver, Rope, Wrench
    }

    public enum Room
    {
        Kitchen, Ballroom, Conservatory,
        BilliardRoom, Library, Study,
        Hall, Lounge, DiningRoom,
        Hallway  // Represents corridor/non-room tiles
    }

    public enum GamePhase
    {
        Setup,
        Rolling,
        Moving,
        MakingSuggestion,
        ResolvingSuggestion,
        MakingAccusation,
        GameOver
    }

    // ─────────────────────────────────────────────
    //  Data Structures
    // ─────────────────────────────────────────────

    [Serializable]
    public class CluedoCard
    {
        public string cardName;
        public CardType type;
        public Suspect? suspect;
        public Weapon? weapon;
        public Room? room;

        public enum CardType { Suspect, Weapon, Room }

        public CluedoCard(Suspect s) { type = CardType.Suspect; suspect = s; cardName = s.ToString(); }
        public CluedoCard(Weapon w)  { type = CardType.Weapon;  weapon  = w; cardName = w.ToString(); }
        public CluedoCard(Room r)    { type = CardType.Room;    room    = r; cardName = r.ToString(); }
    }

    [Serializable]
    public class Solution
    {
        public Suspect suspect;
        public Weapon  weapon;
        public Room    room;

        public bool Matches(Suspect s, Weapon w, Room r) =>
            suspect == s && weapon == w && room == r;
    }

    [Serializable]
    public class Player
    {
        public string      playerName;
        public Suspect     character;
        public List<CluedoCard> hand = new();
        public BoardCell   currentCell;
        public bool        isEliminated;
        public bool        isHuman;

        public bool HasCard(CluedoCard card) =>
            hand.Any(c => c.cardName == card.cardName);

        public CluedoCard RevealCardFor(CluedoCard suspect, CluedoCard weapon, CluedoCard room)
        {
            // Returns first matching card this player can show, or null
            foreach (var c in hand)
                if (c.cardName == suspect.cardName ||
                    c.cardName == weapon.cardName  ||
                    c.cardName == room.cardName)
                    return c;
            return null;
        }
    }

    [Serializable]
    public class BoardCell
    {
        public Vector2Int   gridPosition;
        public Room         room;                    // Hallway if corridor
        public bool         isRoomEntrance;
        public Room         entranceLeadsTo;
        public List<BoardCell> neighbors = new();
        public GameObject   worldObject;             // Linked Unity scene object

        public bool IsInRoom => room != Room.Hallway;
    }

    // ─────────────────────────────────────────────
    //  Inspector-Exposed Fields
    // ─────────────────────────────────────────────

    [Header("Board Setup")]
    [SerializeField] private int boardWidth  = 24;
    [SerializeField] private int boardHeight = 25;

    [Header("Players")]
    [SerializeField] private int numberOfPlayers = 3;   // 2–6

    [Header("Prefabs & References")]
    [SerializeField] private GameObject playerTokenPrefab;
    [SerializeField] private Transform  boardRoot;

    [Header("UI (assign in Inspector)")]
    [SerializeField] private GameObject suggestionPanel;
    [SerializeField] private GameObject accusationPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMPro.TextMeshProUGUI phaseLabel;
    [SerializeField] private TMPro.TextMeshProUGUI currentPlayerLabel;
    [SerializeField] private TMPro.TextMeshProUGUI diceResultLabel;
    [SerializeField] private TMPro.TextMeshProUGUI logText;

    // ─────────────────────────────────────────────
    //  Private State
    // ─────────────────────────────────────────────

    private List<Player>    players       = new();
    private List<BoardCell> allCells      = new();
    private Dictionary<Room, List<BoardCell>> roomCells = new();

    private Solution        solution;
    private int             currentPlayerIndex;
    private int             diceRoll;
    private int             movesRemaining;
    private GamePhase       phase = GamePhase.Setup;

    private List<string>    gameLog = new();

    // ─────────────────────────────────────────────
    //  Unity Lifecycle
    // ─────────────────────────────────────────────

    private void Start()
    {
        InitialiseBoard();
        DealCards();
        StartGame();
    }

    // ─────────────────────────────────────────────
    //  Initialisation
    // ─────────────────────────────────────────────

    private void InitialiseBoard()
    {
        // Build the cell grid (populate neighbours, assign rooms, etc.)
        // In a full project you would load this from a tilemap or ScriptableObject.
        // Here we create a simplified logical graph.
        BuildCellGraph();
        AssignStartingPositions();
    }

    private void BuildCellGraph()
    {
        // Create grid cells
        var grid = new BoardCell[boardWidth, boardHeight];
        for (int x = 0; x < boardWidth; x++)
        for (int y = 0; y < boardHeight; y++)
        {
            var cell = new BoardCell
            {
                gridPosition = new Vector2Int(x, y),
                room         = Room.Hallway
            };
            grid[x, y] = cell;
            allCells.Add(cell);
        }

        // Assign room regions (simplified rectangular regions)
        AssignRoomRegion(grid, Room.Kitchen,       0,  0, 5, 5);
        AssignRoomRegion(grid, Room.Ballroom,      8,  0, 15, 5);
        AssignRoomRegion(grid, Room.Conservatory, 18,  0, 23, 5);
        AssignRoomRegion(grid, Room.BilliardRoom, 18,  7, 23, 12);
        AssignRoomRegion(grid, Room.Library,      18, 14, 23, 19);
        AssignRoomRegion(grid, Room.Study,        18, 21, 23, 24);
        AssignRoomRegion(grid, Room.Hall,          9, 17, 14, 24);
        AssignRoomRegion(grid, Room.Lounge,        0, 19, 5, 24);
        AssignRoomRegion(grid, Room.DiningRoom,    0,  9, 5, 15);

        // Connect orthogonal neighbours
        for (int x = 0; x < boardWidth; x++)
        for (int y = 0; y < boardHeight; y++)
        {
            var cell = grid[x, y];
            if (x > 0)              cell.neighbors.Add(grid[x - 1, y]);
            if (x < boardWidth - 1) cell.neighbors.Add(grid[x + 1, y]);
            if (y > 0)              cell.neighbors.Add(grid[x, y - 1]);
            if (y < boardHeight - 1)cell.neighbors.Add(grid[x, y + 1]);
        }

        // Cache cells per room
        foreach (Room r in Enum.GetValues(typeof(Room)))
            roomCells[r] = allCells.Where(c => c.room == r).ToList();

        Log("Board graph built.");
    }

    private void AssignRoomRegion(BoardCell[,] grid, Room room, int x0, int y0, int x1, int y1)
    {
        for (int x = x0; x <= x1; x++)
        for (int y = y0; y <= y1; y++)
            grid[x, y].room = room;
    }

    private void AssignStartingPositions()
    {
        // Classic Cluedo starting positions (edge cells)
        var starts = new Dictionary<Suspect, Vector2Int>
        {
            { Suspect.MissScarlett, new Vector2Int(7,  24) },
            { Suspect.ColMustard,   new Vector2Int(23, 17) },
            { Suspect.MrsWhite,     new Vector2Int(9,  0)  },
            { Suspect.RevGreen,     new Vector2Int(14, 0)  },
            { Suspect.MrsPeacock,   new Vector2Int(23, 6)  },
            { Suspect.ProfPlum,     new Vector2Int(0,  18) }
        };

        // Create players (first player is human by default)
        var suspects = Enum.GetValues(typeof(Suspect)).Cast<Suspect>().ToList();
        for (int i = 0; i < Mathf.Min(numberOfPlayers, 6); i++)
        {
            var suspect = suspects[i];
            var startPos = starts[suspect];
            var startCell = allCells.First(c => c.gridPosition == startPos);

            var player = new Player
            {
                playerName  = suspect.ToString(),
                character   = suspect,
                currentCell = startCell,
                isHuman     = (i == 0)
            };
            players.Add(player);
        }
    }

    private void DealCards()
    {
        // Build the full deck
        var deck = new List<CluedoCard>();
        foreach (Suspect s in Enum.GetValues(typeof(Suspect))) deck.Add(new CluedoCard(s));
        foreach (Weapon  w in Enum.GetValues(typeof(Weapon)))  deck.Add(new CluedoCard(w));
        foreach (Room    r in Enum.GetValues(typeof(Room)))
            if (r != Room.Hallway) deck.Add(new CluedoCard(r));

        // Randomly select solution (one of each type)
        deck.Shuffle();
        var solutionSuspect = deck.First(c => c.type == CluedoCard.CardType.Suspect);
        var solutionWeapon  = deck.First(c => c.type == CluedoCard.CardType.Weapon);
        var solutionRoom    = deck.First(c => c.type == CluedoCard.CardType.Room);

        solution = new Solution
        {
            suspect = solutionSuspect.suspect.Value,
            weapon  = solutionWeapon.weapon.Value,
            room    = solutionRoom.room.Value
        };

        deck.Remove(solutionSuspect);
        deck.Remove(solutionWeapon);
        deck.Remove(solutionRoom);

        // Shuffle and deal remaining cards
        deck.Shuffle();
        for (int i = 0; i < deck.Count; i++)
            players[i % players.Count].hand.Add(deck[i]);

        Log($"Solution sealed. {deck.Count} cards dealt to {players.Count} players.");
    }

    // ─────────────────────────────────────────────
    //  Game Flow
    // ─────────────────────────────────────────────

    private void StartGame()
    {
        currentPlayerIndex = 0;
        BeginTurn();
    }

    private void BeginTurn()
    {
        var player = CurrentPlayer;
        if (player.isEliminated)
        {
            AdvanceTurn();
            return;
        }

        Log($"── {player.playerName}'s turn ──");
        SetPhase(GamePhase.Rolling);
        UpdateUI();

        if (!player.isHuman)
            StartCoroutine(AITurn(player));
    }

    /// <summary>Call this from a UI button for the human player.</summary>
    public void OnRollDice()
    {
        if (phase != GamePhase.Rolling || !CurrentPlayer.isHuman) return;

        diceRoll = RollDice();
        movesRemaining = diceRoll;
        Log($"{CurrentPlayer.playerName} rolled {diceRoll}.");
        SetPhase(GamePhase.Moving);
        UpdateUI();
    }

    /// <summary>
    /// Move the current player to a neighbouring cell.
    /// Call from UI (e.g. clicking a highlighted cell).
    /// </summary>
    public void OnCellSelected(BoardCell targetCell)
    {
        if (phase != GamePhase.Moving || !CurrentPlayer.isHuman) return;

        var player = CurrentPlayer;
        if (!IsReachableInOneStep(player.currentCell, targetCell)) return;

        MovePlayer(player, targetCell);
        movesRemaining--;

        if (movesRemaining <= 0 || player.currentCell.IsInRoom)
        {
            OnMovementFinished(player);
        }

        UpdateUI();
    }

    private void OnMovementFinished(Player player)
    {
        if (player.currentCell.IsInRoom)
        {
            Log($"{player.playerName} entered {player.currentCell.room}.");
            SetPhase(GamePhase.MakingSuggestion);

            if (!player.isHuman)
                StartCoroutine(AIMakeSuggestion(player));
        }
        else
        {
            EndTurn();
        }
    }

    /// <summary>Human player submits a suggestion.</summary>
    public void OnSuggestionSubmitted(Suspect suspect, Weapon weapon)
    {
        if (phase != GamePhase.MakingSuggestion || !CurrentPlayer.isHuman) return;
        ProcessSuggestion(CurrentPlayer, suspect, weapon, CurrentPlayer.currentCell.room);
    }

    /// <summary>Human player makes a final accusation.</summary>
    public void OnAccusationSubmitted(Suspect suspect, Weapon weapon, Room room)
    {
        if (!CurrentPlayer.isHuman) return;
        ProcessAccusation(CurrentPlayer, suspect, weapon, room);
    }

    private void ProcessSuggestion(Player suggester, Suspect suspect, Weapon weapon, Room room)
    {
        Log($"{suggester.playerName} suggests: {suspect}, {weapon}, in {room}.");
        SetPhase(GamePhase.ResolvingSuggestion);

        // Move accused token into the room
        var accused = players.FirstOrDefault(p => p.character == suspect);
        if (accused != null && roomCells.ContainsKey(room))
        {
            accused.currentCell = roomCells[room][0];
            Log($"{accused.playerName} moved to {room} by suggestion.");
        }

        // Rotate through other players to find a refutation
        StartCoroutine(ResolveSuggestion(suggester, suspect, weapon, room));
    }

    private IEnumerator ResolveSuggestion(Player suggester, Suspect suspect, Weapon weapon, Room room)
    {
        var suspectCard = new CluedoCard(suspect);
        var weaponCard  = new CluedoCard(weapon);
        var roomCard    = new CluedoCard(room);

        bool refuted = false;
        int startIdx = (players.IndexOf(suggester) + 1) % players.Count;
        int idx      = startIdx;

        do
        {
            var refuter = players[idx];
            if (!refuter.isEliminated && refuter != suggester)
            {
                yield return new WaitForSeconds(0.5f);   // Pacing for AI opponents

                var shown = refuter.RevealCardFor(suspectCard, weaponCard, roomCard);
                if (shown != null)
                {
                    if (suggester.isHuman)
                        Log($"{refuter.playerName} shows you: [{shown.cardName}]");
                    else
                        Log($"{refuter.playerName} showed a card to {suggester.playerName}.");

                    refuted = true;
                    break;
                }
                else
                {
                    Log($"{refuter.playerName} cannot refute.");
                }
            }
            idx = (idx + 1) % players.Count;
        }
        while (idx != startIdx);

        if (!refuted)
            Log("Nobody could refute the suggestion!");

        EndTurn();
    }

    private void ProcessAccusation(Player accuser, Suspect suspect, Weapon weapon, Room room)
    {
        Log($"{accuser.playerName} accuses: {suspect}, {weapon}, in {room}!");

        if (solution.Matches(suspect, weapon, room))
        {
            Log($"✓ Correct! {accuser.playerName} wins!");
            SetPhase(GamePhase.GameOver);
            ShowWinScreen(accuser);
        }
        else
        {
            Log($"✗ Wrong! {accuser.playerName} is eliminated.");
            accuser.isEliminated = true;

            if (players.Count(p => !p.isEliminated) == 0)
            {
                Log("All players eliminated. Game over.");
                SetPhase(GamePhase.GameOver);
            }
            else
            {
                EndTurn();
            }
        }
    }

    private void EndTurn()
    {
        AdvanceTurn();
    }

    private void AdvanceTurn()
    {
        int tried = 0;
        do
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            tried++;
            if (tried > players.Count) { SetPhase(GamePhase.GameOver); return; }
        }
        while (CurrentPlayer.isEliminated);

        BeginTurn();
    }

    // ─────────────────────────────────────────────
    //  AI Logic
    // ─────────────────────────────────────────────

    private IEnumerator AITurn(Player aiPlayer)
    {
        yield return new WaitForSeconds(1f);

        diceRoll       = RollDice();
        movesRemaining = diceRoll;
        Log($"{aiPlayer.playerName} (AI) rolled {diceRoll}.");
        SetPhase(GamePhase.Moving);

        // Simple AI: move toward a random room
        yield return StartCoroutine(AIMoveToRoom(aiPlayer));
    }

    private IEnumerator AIMoveToRoom(Player aiPlayer)
    {
        // Pick a random room target
        var targetRoom   = (Room)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Room)).Length - 1);
        var targetCells  = roomCells[targetRoom];
        var destination  = FindCellInBFS(aiPlayer.currentCell, targetCells, movesRemaining);

        if (destination != null)
        {
            yield return StartCoroutine(AnimateMoveAlongPath(aiPlayer, destination));
        }

        yield return new WaitForSeconds(0.5f);
        OnMovementFinished(aiPlayer);
    }

    private IEnumerator AIMakeSuggestion(Player aiPlayer)
    {
        yield return new WaitForSeconds(1f);

        // Random AI suggestion (a smarter AI would track deductions)
        var suspect = (Suspect)UnityEngine.Random.Range(0, 6);
        var weapon  = (Weapon) UnityEngine.Random.Range(0, 6);
        ProcessSuggestion(aiPlayer, suspect, weapon, aiPlayer.currentCell.room);
    }

    // ─────────────────────────────────────────────
    //  Movement Helpers
    // ─────────────────────────────────────────────

    private void MovePlayer(Player player, BoardCell cell)
    {
        player.currentCell = cell;
        if (player.currentCell.worldObject != null)
            player.currentCell.worldObject.transform.position =
                cell.worldObject ? cell.worldObject.transform.position : Vector3.zero;
    }

    private bool IsReachableInOneStep(BoardCell from, BoardCell to) =>
        from.neighbors.Contains(to);

    /// <summary>BFS to find the best reachable cell toward a set of targets within N steps.</summary>
    private BoardCell FindCellInBFS(BoardCell start, List<BoardCell> targets, int maxSteps)
    {
        var visited = new HashSet<BoardCell> { start };
        var queue   = new Queue<(BoardCell cell, int steps)>();
        queue.Enqueue((start, 0));

        BoardCell closest = null;

        while (queue.Count > 0)
        {
            var (cell, steps) = queue.Dequeue();

            if (targets.Contains(cell))
                return cell;

            if (steps >= maxSteps) continue;

            foreach (var neighbour in cell.neighbors)
            {
                if (visited.Contains(neighbour)) continue;
                visited.Add(neighbour);
                queue.Enqueue((neighbour, steps + 1));
                closest = neighbour;
            }
        }
        return closest;
    }

    private IEnumerator AnimateMoveAlongPath(Player player, BoardCell destination)
    {
        // BFS pathfind then animate step-by-step
        var path = BFSPath(player.currentCell, destination);
        foreach (var cell in path)
        {
            MovePlayer(player, cell);
            UpdateUI();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private List<BoardCell> BFSPath(BoardCell start, BoardCell end)
    {
        var prev    = new Dictionary<BoardCell, BoardCell> { [start] = null };
        var queue   = new Queue<BoardCell>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == end) break;
            foreach (var n in current.neighbors)
                if (!prev.ContainsKey(n))
                {
                    prev[n] = current;
                    queue.Enqueue(n);
                }
        }

        var path = new List<BoardCell>();
        for (var c = end; c != null && c != start; c = prev.ContainsKey(c) ? prev[c] : null)
            path.Insert(0, c);
        return path;
    }

    // ─────────────────────────────────────────────
    //  Dice
    // ─────────────────────────────────────────────

    private int RollDice() =>
        UnityEngine.Random.Range(1, 7) + UnityEngine.Random.Range(1, 7);

    // ─────────────────────────────────────────────
    //  UI
    // ─────────────────────────────────────────────

    private void SetPhase(GamePhase newPhase)
    {
        phase = newPhase;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (phaseLabel)         phaseLabel.text         = phase.ToString();
        if (currentPlayerLabel) currentPlayerLabel.text = CurrentPlayer?.playerName ?? "";
        if (diceResultLabel)    diceResultLabel.text    = phase == GamePhase.Moving
                                                            ? $"Moves left: {movesRemaining}"
                                                            : $"Roll: {diceRoll}";
        if (logText)            logText.text            = string.Join("\n", gameLog.TakeLast(8));

        if (suggestionPanel) suggestionPanel.SetActive(phase == GamePhase.MakingSuggestion && CurrentPlayer.isHuman);
        if (accusationPanel) accusationPanel.SetActive(phase == GamePhase.Moving           && CurrentPlayer.isHuman);
        if (winPanel)        winPanel.SetActive(phase == GamePhase.GameOver);
    }

    private void ShowWinScreen(Player winner)
    {
        if (winPanel) winPanel.SetActive(true);
        Log($"🏆 Winner: {winner.playerName}!");
    }

    // ─────────────────────────────────────────────
    //  Logging
    // ─────────────────────────────────────────────

    private void Log(string message)
    {
        Debug.Log($"[Cluedo] {message}");
        gameLog.Add(message);
        if (gameLog.Count > 50) gameLog.RemoveAt(0);
        if (logText) logText.text = string.Join("\n", gameLog.TakeLast(8));
    }

    // ─────────────────────────────────────────────
    //  Convenience
    // ─────────────────────────────────────────────

    private Player CurrentPlayer => players.Count > 0 ? players[currentPlayerIndex] : null;

    // ─────────────────────────────────────────────
    //  Public Queries (for UI / other scripts)
    // ─────────────────────────────────────────────

    public GamePhase  CurrentPhase    => phase;
    public Player     GetCurrentPlayer() => CurrentPlayer;
    public List<Player> GetAllPlayers() => players;
    public Solution   GetSolution()    => solution;          // Debug only — hide in production!
}

// ─────────────────────────────────────────────────────────────────────────────
//  Extension: Fisher-Yates shuffle for List<T>
// ─────────────────────────────────────────────────────────────────────────────

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}