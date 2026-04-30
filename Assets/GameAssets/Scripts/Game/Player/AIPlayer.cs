using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// AI-controlled player.
/// Decides movement, target rooms, suggestions, accusations,
/// and updates its detective sheet knowledge.
/// </summary>
public class AIPlayer : Player
{
    private RoomCard currentTargetRoom;
    private DetectiveSheet detectiveSheet;

    private double suspectConfidence;
    private double weaponConfidence;
    private double roomConfidence;

    private StrategyType strategyType;
    private Dictionary<string, int> cardBiases = new Dictionary<string, int>();

    public AIPlayer()
    {
        detectiveSheet = new DetectiveSheet();

        suspectConfidence = 0;
        weaponConfidence = 0;
        roomConfidence = 0;

        strategyType = StrategyType.Safe;
    }

    /// <summary>
    /// Main AI turn loop.
    /// </summary>
    public void TakeTurn(GameController gameController, Dice dice)
    {
        //1 Roll dice
        int diceRoll = dice.Roll();

        //2 Rerieve all legal postions
        List<Vector2Int> legalMoves = GetLegalMoves(diceRoll);


        //4 Bias Update
        UpdateBiases();

        //5 Target Selection
        currentTargetRoom = ChooseTargetRoom();

        Vector2Int bestMove = BestMoveTowardsTarget(legalMoves);

        MoveTo(bestMove);

        if (IsInRoom())
        {
            if (ShouldMakeAccusation())
            {
                Suggestion accusation = BuildSuggestion();
                MakeAccusation(accusation);
            }
            else
            {
                Suggestion suggestion = BuildSuggestion();
                MakeSuggestion(suggestion);
            }
        }

        UpdateDetectiveSheet();
        UpdateConfidence();

        EndTurn();
    }

    /// <summary>
    /// Gets all board positions the AI can legally move to.
    /// </summary>
    private List<Vector2Int> GetLegalMoves(int diceRoll)
    {
        // TODO: connect this to Board movement/pathfinding.
        return new List<Vector2Int>();
    }

    /// <summary>
    /// Updates decision weights based on detective sheet and strategy
    /// </summary>
    private void UpdateBiases()
    {
        cardBiases.Clear();
        List<DetectiveCardEntry> entries = detectiveSheet.GetEntries();
        int possibleSuspects = 0;
        int possibleWeapons = 0;
        int possibleRooms = 0;
        foreach (DetectiveCardEntry entry in entries)
        {
            string cardName = entry.GetCardName();
            if (entry.IsCrossedOut())
                continue;
            if (IsSuspect(cardName))
                possibleSuspects++;
            else if (IsWeapon(cardName))
                possibleWeapons++;
            else if (IsRoom(cardName))
                possibleRooms++;
        }

        foreach (DetectiveCardEntry entry in entries)
        {

            string cardName = entry.GetCardName();
            if (entry.IsCrossedOut())
            {
                cardBiases[cardName] = 0;
                continue;
            }

            if (IsSuspect(cardName))
                cardBiases[cardName] = CalculateBias(possibleSuspects);
            else if (IsWeapon(cardName))
                cardBiases[cardName] = CalculateBias(possibleWeapons);
            else if (IsRoom(cardName))
                cardBiases[cardName] = CalculateBias(possibleRooms);
        }
    }

    private bool IsSuspect(string cardName)
    {
        return cardName == "Miss Scarlett" ||
            cardName == "Colonel Mustard" ||
            cardName == "Mrs White" ||
            cardName == "Reverend Green" ||
            cardName == "Mrs Peacock" ||
            cardName == "Professor Plum";
    }

    private bool IsWeapon(string cardName)
    {
        return cardName == "Candlestick" ||
            cardName == "Dagger" ||
            cardName == "Lead Pipe" ||
            cardName == "Revolver" ||
            cardName == "Rope" ||
            cardName == "Wrench";
    }

    private bool IsRoom(string cardName)
    {
        return cardName == "Kitchen" ||
            cardName == "Ballroom" ||
            cardName == "Conservatory" ||
            cardName == "Dining Room" ||
            cardName == "Billiard Room" ||
            cardName == "Library" ||
            cardName == "Lounge" ||
            cardName == "Hall" ||
            cardName == "Study";
    }

    private int CalculateBias(int remainingCount)
    {
    if (remainingCount <= 1)
        return 100;

    if (remainingCount <= 2)
        return 75;

    if (remainingCount <= 3)
        return 50;

    return 20;
    }

    /// <summary>
    /// Chooses a target room for the AI to move towards.
    /// </summary>
    private RoomCard ChooseTargetRoom()
    {
        List<RoomCard> closestRooms = GetThreeClosestRooms();

        if (closestRooms.Count == 1)
        {
            return currentTargetRoom;
        }
        if (currentTargetRoom == null)
        {
            return ChooseNewTarget(closestRooms);
        }

        return ChooseExistingTarget(closestRooms);
    }

    private RoomCard ChooseNewTarget(List<RoomCard> closestRooms)
    {
        if (closestRooms.Count == 1)
        {
            return closestRooms[0];
        }
        if (closestRooms.Count == 2)
        {
            return WeightedRoomChoice(
                closestRooms[0], 65,
                closestRooms[1], 35
            );
        }
        return WeightedRoomChoice(
                closestRooms[0], 50,
                closestRooms[1], 35,
                closestRooms[2], 15
            );
    }

    private List<RoomCard> GetThreeClosestRooms()
    {
        //TODO:
        // This should cal distance from AI postion to each room
        //ATM returns a test list of rooms  from the game/board/deck
        return new List<RoomCard>();
    }

    private RoomCard ChooseExistingTarget(List<RoomCard> closestRooms)
    {
    if (currentTargetRoom != null && UnityEngine.Random.value < 0.90f)
        return currentTargetRoom;

    if (closestRooms.Count == 1)
        return closestRooms[0];

    if (closestRooms.Count == 2)
    {
        return WeightedRoomChoice(
            closestRooms[0], 95,
            closestRooms[1], 5
        );
    }

    return WeightedRoomChoice(
        closestRooms[0], 90,
        closestRooms[1], 5,
        closestRooms[2], 5
    );
}

    private RoomCard WeightedRoomChoice(RoomCard room1, int weight1, 
    RoomCard room2, int weight2)
    {
        int roll = UnityEngine.Random.Range(1, weight1 + weight2 + 1);
        if (roll <= weight1)
        {
            return room1;
        }
        return room2;
    }

    private RoomCard WeightedRoomChoice(RoomCard room1, int weight1,
    RoomCard room2, int weight2, RoomCard room3, int weight3)
    {
        int totalWeight = weight1 + weight2 + weight3;
        int roll = UnityEngine.Random.Range(1, totalWeight + 1);

        if (roll <= weight1)
        {
            return room1;
        }

        if (roll <= weight1 + weight2)
        {
            return room2;
        }
        return room3;

    }

    /// <summary>
    /// Chooses the best legal move towards the current target room.
    /// </summary>
    private Vector2Int BestMoveTowardsTarget(List<Vector2Int> legalMoves)
    {
        if (legalMoves.Count == 0)
            return Vector2Int.zero;

        // TODO: replace with distance-to-room logic.
        return legalMoves[0];
    }

    /// <summary>
    /// Moves the AI to the chosen board position.
    /// </summary>
    private void MoveTo(Vector2Int position)
    {
        // TODO: update player position when board/player movement is ready.
    }

    /// <summary>
    /// Checks whether the AI is currently inside a room.
    /// </summary>
    private bool IsInRoom()
    {
        // TODO: connect to board tile/room detection.
        return false;
    }

    /// <summary>
    /// Builds a suggestion or accusation from the AI's current knowledge.
    /// </summary>
    private Suggestion BuildSuggestion()
    {
        CharacterCard character = ChooseSuspectCard();
        WeaponCard weapon = ChooseWeaponCard();
        RoomCard room = ChooseRoomCard();

        // Your Suggestion constructor order is:
        // Suggestion(CharacterCard characterIn, RoomCard roomIn, WeaponCard weaponIn)
        return new Suggestion(character, room, weapon);
    }

    private CharacterCard ChooseSuspectCard()
    {
        // TODO: choose a suspect not crossed out in detectiveSheet.
        return null;
    }

    private WeaponCard ChooseWeaponCard()
    {
        // TODO: choose a weapon not crossed out in detectiveSheet.
        return null;
    }

    private RoomCard ChooseRoomCard()
    {
        // TODO: use current room once board logic exists.
        return currentTargetRoom;
    }

    /// <summary>
    /// Decides whether confidence is high enough to accuse.
    /// </summary>
    private bool ShouldMakeAccusation()
    {
        double confidence = GetOverallConfidence();

        if (confidence >= 100)
            return true;

        if (confidence >= 90)
            return UnityEngine.Random.value < 0.33f;

        return false;
    }

    private void MakeSuggestion(Suggestion suggestion)
    {
        Debug.Log("AI suggestion: " +
                  suggestion.GetCharacter() + ", " +
                  suggestion.GetWeapon() + ", " +
                  suggestion.GetRoom());

        // TODO: pass suggestion to GameController once HandleSuggestion is public.
    }

    private void MakeAccusation(Suggestion accusation)
    {
        Debug.Log("AI accusation: " +
                  accusation.GetCharacter() + ", " +
                  accusation.GetWeapon() + ", " +
                  accusation.GetRoom());

        // TODO: pass accusation to GameController once HandleAccusation is public.
    }

    /// <summary>
    /// Updates the AI detective sheet using known information.
    /// </summary>
    private void UpdateDetectiveSheet()
    {
        // TODO:
        // Auto-cross cards in AI hand.
        // Auto-cross cards shown by other players.
        // Do not auto-cross unknown envelope candidates.
    }

    /// <summary>
    /// Updates confidence values for suspect, weapon, and room.
    /// </summary>
    private void UpdateConfidence()
    {
        // TODO:
        // Increase confidence when fewer unknown candidates remain.
        // Keep separate confidence for suspect, weapon, and room.
    }

    private double GetOverallConfidence()
    {
        return (suspectConfidence + weaponConfidence + roomConfidence) / 3.0;
    }

    private void EndTurn()
    {
        Debug.Log("AI turn ended.");

        // TODO: tell GameController to move to next player.
    }
}

