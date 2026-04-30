using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// AI-controlled player.
/// 
/// As player class not fleshed out yet, some fileds Ill do here, until it done
/// 
/// 
/// </summary>
public class AIPlayer : Player
{
<<<<<<< Updated upstream
    private int ownPlayerIndex;
    private int totalActivePlayers;
    private List<Card> ownHand = new List<Card>();
 
=======
	private int ID;
	private CharacterCard character;
	private int position;
	private List<Card> hand = new List<Card>();
	private bool eliminated;

	private RoomCard currentTargetRoom;
>>>>>>> Stashed changes
    private DetectiveSheet detectiveSheet;
    private List<UnknownDisproval> unknownDisprovals = new List<UnknownDisproval>();

    //For Decisions:
    private RoomCard currentTargetRoom;
    private double suspectConfidence;
    private double weaponConfidence;
    private double roomConfidence;
    private StrategyType strategyType;
    private Dictionary<string, int> cardBiases = new Dictionary<string, int>();

<<<<<<< Updated upstream
    //Card name reference data (Could be gone later)
    private static readonly string[] AllSuspectNames = {
        "Miss Scarlett", "Colonel Mustard", "Mrs White",
        "Reverend Green", "Mrs Peacock", "Professor Plum"
    };
    private static readonly string[] AllWeaponNames = {
        "Candlestick", "Knife", "Lead Pipe", "Revolver", "Rope", "Wrench"
    };
    private static readonly string[] AllRoomNames = {
        "Kitchen", "Ballroom", "Conservatory", "Dining Room", "Billiards Room",
        "Library", "Lounge", "Hall", "Study"
    };

    public AIPlayer()
    {
=======
    public AIPlayer(int ID, CharacterCard character) : base(ID, character)
    {
		this.ID = ID;
		this.character = character;
		this.position = 0;
		this.eliminated = false;

		detectiveSheet = new DetectiveSheet();

>>>>>>> Stashed changes
        suspectConfidence = 0;
        weaponConfidence = 0;
        roomConfidence = 0;
        strategyType = StrategyType.Safe;
    }


    //GameController should call these
    
    /// <summary>
    /// Set up the Ai. Call once before dealing cards
    /// </summary>
    /// <param name="playerIndex">Player postion</param>
    /// <param name="totalActivePlayers">How many active players</param>
    public void Initialise(int playerIndex, int totalActivePlayers)
    {
        this.ownPlayerIndex = playerIndex;
        this.totalActivePlayers = totalActivePlayers;
        this.detectiveSheet = new DetectiveSheet(totalActivePlayers);
        this.ownHand.Clear();
        this.unknownDisprovals.Clear();

    }

    /// <summary>
    /// Choose between safe and deceptive (bluff) play styles
    /// </summary>
    /// <param name="strategy">Safe or Deceptive</param>
    public void SetStrategy(StrategyType strategy)
    {
        strategyType = strategy;
    }

    /// <summary>
    /// Call once cards are dealt
    /// This auto crosses out all cards in player own hand
    /// </summary>
    /// <param name="dealtCards">Cards dealt</param>
    public void onHandDealt(List<Card> dealtCards)
    {
        if (dealtCards == null) return;
        ownHand = new List<Card>(dealtCards);
        if (detectiveSheet != null)
        {
            detectiveSheet.InitialiseFromHand(ownHand, ownPlayerIndex);
        }
        UpdateConfidence();
    }
    
    /// <summary>
    /// Call when another player shows this AI a card
    /// Ai trting to make suggestion that they could disprove
    /// </summary>
    /// <param name="card">Card shown</param>
    /// <param name="byPlayerIndex">Player postion</param>
    public void OnCardShown(Card card, int byPlayerIndex)
    {
        if (card == null || detectiveSheet == null) return;
        detectiveSheet.MarkAuto(card, byPlayerIndex);
        UpdateConfidence();
    }

    /// <summary>
    /// Call after every suggestion by anyone. Helps the deduction
    /// disproverIndex == -1 means nobody could disprove
    /// shownCard: only not null if Ai was suggesster/disprover
    /// Null being passed means well record the disproval as "unknwn which card"
    /// </summary>
    /// <param name="suggestionIndex">Index of player made the suggestion</param>
    /// <param name="suggestion">Suggestion made</param>
    /// <param name="disproverIndex">Index of the disporver or -1 if nobody disproved</param>
    /// <param name="shownCard">The card shown (if Known) or null</param>
    public void OnSuggestionMade(int suggestionIndex, Suggestion suggestion,
    int disproverIndex, Card shownCard)
    {
        if (suggestion == null || detectiveSheet == null) return;

        string cName = suggestion.GetCharacter() != null ? suggestion.GetCharacter().ToString() : null;
        string wName = suggestion.GetWeapon() != null ? suggestion.GetWeapon().ToString() : null;
        string rName = suggestion.GetRoom() != null ? suggestion.GetRoom().ToString() : null;


        //After suggestion made, its chance for other players to disprove this claim
        //so it keep going until one players can disprove the suggestion (they have one of the cards)
        //If disproverIndex is -1, every other active player passed i.e so nobody could disprove the claim
        for (int offset =1; offset < totalActivePlayers; offset++)
        {
            int index = (suggestionIndex + offset) % totalActivePlayers;
            if (index == disproverIndex) break;
            //The player has passed, they dont have one the cards
            if (cName != null) detectiveSheet.MarkNotHeldBy(cName, index);
            if (wName != null) detectiveSheet.MarkNotHeldBy(wName, index);
            if (rName != null) detectiveSheet.MarkNotHeldBy(rName, index);
        }

        //The disprover, has at least one the three
        // If we acctually saw which card, its auto crossed out
        if (disproverIndex >= 0)
        {
            if (shownCard != null)
            {
                detectiveSheet.MarkAuto(shownCard, disproverIndex);
            }
            else
            {
                unknownDisprovals.Add(new UnknownDisproval
                {
                    DisproverIndex = disproverIndex,
                    CardNames = new[] { cName, wName, rName }
                });
            }
        }

        // check unknown disprovals: if two of three cards in a past disproval
        // are now known not to be in disprovers hand, the third must be
        ResolveUnknownDisprovals();
        UpdateConfidence();
    }

    //TurnLOOP

    public void TakeTurn(GameController gameController, Dice dice)
    {
        int diceRoll = dice.Roll();
        List<Vector2Int> legalMoves = GetLegalMoves(diceRoll);

        UpdateBiases();
        currentTargetRoom = ChooseTargetRoom();

        Vector2Int bestMove = BestMoveTowardsTarget(legalMoves);
        MoveTo(bestMove);
        if (IsInRoom())
        {
            if (ShouldMakeAccusation())
            {
                Suggestion accusation = BuildAccusation();
                MakeAccusation(accusation);
            }
            else
            {
                Suggestion suggestion = BuildSuggestion();
                MakeSuggestion(suggestion);
            }
        }

        UpdateConfidence();
        EndTurn();
    }

    /// <summary>
    /// For each past disproval where we didn't see the card, check if we now
    /// know that 2 of the 3 cards are NOT in the disprover's hand
    /// If so,the third must be: autocross it
    /// Repeats until no progress is made (one resolved disproval can unlock another)
    /// </summary>
    private void ResolveUnknownDisprovals()
    {
        bool madeProgress = true;
        while (madeProgress)
        {
            madeProgress = false;
            for (int i = unknownDisprovals.Count -1; i >= 0; i--)
            {
                UnknownDisproval u = unknownDisprovals[i];
                int possibleCount = 0;
                string lastPossible = null;
                bool resolved = false;

                foreach (string name in u.CardNames)
                {
                    if (name == null) continue;
                    DetectiveCardEntry entry = detectiveSheet.GetEntryByName(name);
                    if (entry == null) continue;
                    if (entry.IsAutoCrossedOut(u.DisproverIndex))
                    {
                        //We know that elsewhere has this card (resoloved)
                        resolved = true;
                        break;
                    }
                    if (!entry.IsKnownNotHeldBy(u.DisproverIndex))
                    {
                        possibleCount++;
                        lastPossible = name;
                    }
                }

                if (resolved)
                {
                    unknownDisprovals.RemoveAt(i);
                    madeProgress = true;
                }
                else if (possibleCount ==1 && lastPossible != null)
                {
                    detectiveSheet.MarkAuto(lastPossible, u.DisproverIndex);
                    unknownDisprovals.RemoveAt(i);
                    madeProgress = true;
                }
            }
        }
    }

    /// <summary>
    /// Updates decision weights based on detective sheet and strategy
    /// </summary>
    private void UpdateBiases()
    {
        if (detectiveSheet == null) return;
        cardBiases.Clear();

        int possibleSuspects = CountUnruledOut(AllSuspectNames);
        int possibleWeapons = CountUnruledOut(AllWeaponNames);
        int possibleRooms = CountUnruledOut(AllRoomNames);
        
        foreach (string name in AllSuspectNames) cardBiases[name] = BiasFor(name, possibleSuspects);
        foreach (string name in AllWeaponNames) cardBiases[name] = BiasFor(name, possibleWeapons);
        foreach (string name in AllRoomNames) cardBiases[name] = BiasFor(name, possibleRooms);
    }

    /// <summary>
    /// Give a score based on how useful it is for a suggestion
    /// Card = 0: been rulled out
    /// Cards with fewer remaing cards are given a higher score
    /// </summary>
    /// <param name="cardName">name of the card</param>
    /// <param name="remainingInGroup">Number of cards in a group not been ruled out</param>
    /// <returns>Bias score used for choosing suggestions</returns>
    private int BiasFor(string cardName, int remainingInGroup)
    {
        DetectiveCardEntry entry = detectiveSheet.GetEntryByName(cardName);
        if (entry == null) return 0;
        if (entry.IsAutoCrossedOutAnywhere()) return 0; //Rulled out

        //Higher weight when fewer candiates remain 

        if (remainingInGroup <= 1) return 100;
        if (remainingInGroup <= 2) return 75;
        if (remainingInGroup <= 3) return 50;
        return 20;
    }

    /// <summary>
    /// Counts how many cards havent been ruled out
    /// </summary>
    /// <param name="names">Array of card names from a group </param>
    /// <returns>Number cards that havent been ruled out</returns>
    private int CountUnruledOut(string[] names)
    {
        int count = 0;
        foreach (string n in names)
        {
            DetectiveCardEntry e = detectiveSheet.GetEntryByName(n);
            if (e != null && !e.IsAutoCrossedOutAnywhere()) count++;
        }
        return count;
    }



    /// <summary>
    /// Chooses a target room for the AI to move towards.
    /// </summary>
    private RoomCard ChooseTargetRoom()
    {
        List<RoomCard> closestRooms = GetThreeClosestRooms();
        if (closestRooms == null || closestRooms.Count == 0) return currentTargetRoom;
        if (currentTargetRoom == null) return ChooseNewTarget(closestRooms);
        return ChooseExistingTarget(closestRooms);
    }

    private RoomCard ChooseNewTarget(List<RoomCard> closestRooms)
    {
        if (closestRooms.Count == 1) return closestRooms[0];
        if (closestRooms.Count == 2)
        {
            return WeightedRoomChoice(closestRooms[0], 65, closestRooms[1], 35);
        }
        return WeightedRoomChoice(closestRooms[0], 50, closestRooms[1], 35, closestRooms[2], 15);
    }

    private RoomCard ChooseExistingTarget(List<RoomCard> closestRooms)
    {
        if (currentTargetRoom != null && UnityEngine.Random.value < 0.90f)
        {
            return currentTargetRoom;
        }
        if (closestRooms.Count == 1) return closestRooms[0];
        if (closestRooms.Count == 2)
        {
            return WeightedRoomChoice(closestRooms[0], 95, closestRooms[1], 5);
        }
        return WeightedRoomChoice(closestRooms[0], 90, closestRooms[1], 5, closestRooms[2], 5);
    }

    private RoomCard WeightedRoomChoice(RoomCard r1, int w1, RoomCard r2, int w2)
    {
        int roll = UnityEngine.Random.Range(1, w1 + w2 + 1);
        return roll <= w1 ? r1 : r2;
    }

    private RoomCard WeightedRoomChoice(RoomCard r1, int w1, RoomCard r2, int w2, RoomCard r3, int w3)
    {
        int total = w1 + w2 + w3;
        int roll = UnityEngine.Random.Range(1, total + 1);
        if (roll <= w1) return r1;
        if (roll <= w1 + w2) return r2;
        return r3;
    }

    //Suggestion and Accusation building


    private Suggestion BuildSuggestion()
    {
        bool deceiveSuspect = ShouldBluff();
        bool deceiveWeapon = ShouldBluff();

        CharacterCard character = deceiveSuspect 
        ? PickOwnHandSuspect() ?? ChooseSuspectCard() : ChooseSuspectCard();

        WeaponCard weapon = deceiveWeapon 
        ? PickOwnHandWeapon() ?? ChooseWeaponCard() : ChooseWeaponCard();

        RoomCard room = ChooseRoomCard();

        return new Suggestion(character, room, weapon);
    }

    private Suggestion BuildAccusation()
    {
        return new Suggestion(
        PickBestEnvelopeCandidate<CharacterCard>(AllSuspectNames),
        PickBestEnvelopeCandidate<RoomCard>(AllRoomNames),
        PickBestEnvelopeCandidate<WeaponCard>(AllWeaponNames)
        );
    }

    private bool ShouldBluff()
    {
        if (strategyType != StrategyType.Deceptive) return false;

        //Don't bluff when confidnet (need to be accruarte)
        if (GetOverallConfidence() > 75) return false;
        return UnityEngine.Random.value < 0.30f;
    }

    private CharacterCard ChooseSuspectCard()
    {
        string name = WeightedPick(AllSuspectNames);
        if (name == null) return null;
        // caller must resolve to a real card object
        return new CharacterCard(name, null);

    }

    private WeaponCard ChooseWeaponCard()
    {
        string name = WeightedPick(AllWeaponNames);
        if (name == null) return null;
        return new WeaponCard(name, null);
    }

    /// <summary>
    /// Choose room card: must match the room the AI is currently in
    /// </summary>
    private RoomCard ChooseRoomCard()
    {
        // Until Board is wired up: fall back to the target room
        return currentTargetRoom;
    }

    /// <summary>
    /// Pick a random card weighted by cardBiases
    /// </summary>
    /// <param name="names">card names</param>
    /// <returns></returns>
    private string WeightedPick(string[] names)
    {
        int totalWeight = 0;
        foreach (string n in names)
        {
            int w;
            if (cardBiases.TryGetValue(n, out w)) totalWeight += w;
        }
        if (totalWeight <= 0)
        {
            // No good options: fall back to anything not ruled out
            foreach (string n in names)
            {
                DetectiveCardEntry e = detectiveSheet.GetEntryByName(n);
                if (e != null && !e.IsAutoCrossedOutAnywhere()) return n;
            }
            return names.Length > 0 ? names[0] : null;
        }

        int roll = UnityEngine.Random.Range(1, totalWeight + 1);
        int running = 0;
        foreach (string n in names)
        {
            int w;
            if (!cardBiases.TryGetValue(n, out w)) continue;
            running += w;
            if (roll <= running) return n;
        }
        return names[0];
    }

    /// <summary>
    /// Pick the most likey murder cards in a category
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="names"></param>
    /// <returns></returns>
    private T PickBestEnvelopeCandidate<T>(string[] names) where T : Card
    {
        foreach (string n in names)
        {
            DetectiveCardEntry e = detectiveSheet.GetEntryByName(n);
            if (e != null && e.IsDefinitelyInEnvelope())
            {
                return CreateCardOfType<T>(n);
            }
        }
        // Fallback: first non-ruled-out card (best guess)
        foreach (string n in names)
        {
            DetectiveCardEntry e = detectiveSheet.GetEntryByName(n);
            if (e != null && !e.IsAutoCrossedOutAnywhere())
            {
                return CreateCardOfType<T>(n);
            }
        }
        return null;
    }

    private T CreateCardOfType<T>(string name) where T : Card
    {
        // If moves to scriptable object cards, replace with a lookup instead.
        if (typeof(T) == typeof(CharacterCard)) return new CharacterCard(name, null) as T;
        if (typeof(T) == typeof(WeaponCard)) return new WeaponCard(name, null) as T;
        if (typeof(T) == typeof(RoomCard)) return new RoomCard(name, null) as T;
        return null;
    }


    //Bluff

    /// <summary>
    /// Pick card from the AI player own hand
    /// </summary>
    /// <returns></returns>
    private CharacterCard PickOwnHandSuspect()
    {
        foreach (Card c in ownHand)
        {
            CharacterCard cc = c as CharacterCard;
            if (cc != null) return cc;
        }
        return null;
    }

    private WeaponCard PickOwnHandWeapon()
    {
        foreach (Card c in ownHand)
        {
            WeaponCard wc = c as WeaponCard;
            if (wc != null) return wc;
        }
        return null;
    }

    //Confidence

    private void UpdateConfidence()
    {
        if (detectiveSheet == null) return;

        suspectConfidence = ConfidenceFor(AllSuspectNames);
        weaponConfidence= ConfidenceFor(AllWeaponNames);
        roomConfidence= ConfidenceFor(AllRoomNames);
    }

    private double ConfidenceFor(string[] names)
    {
        int total = names.Length;
        int remaining = 0;
        bool definite = false;

        foreach (string n in names)
        {
            DetectiveCardEntry e = detectiveSheet.GetEntryByName(n);
            if (e== null) continue;
            if (e.IsDefinitelyInEnvelope()) 
            { 
                definite = true; 
                break;
            }
            if (!e.IsAutoCrossedOutAnywhere()) remaining++;
        }

            if (definite) return 100;
            if (remaining <= 0) return 0;
            if (remaining == 1) return 100;

            // Rises sharply near the end
            // 6 candidates - 0.5%, 4 - 30%, 3 - 42%, 2 - 70%, 1 - 100
            double ratio = (double)(total - remaining + 1) / total;
            return Mathf.Clamp((float)(100.0 * ratio * ratio * ratio), 0f, 99f);
        
    }

    private double GetOverallConfidence()
    {
        return (suspectConfidence + weaponConfidence + roomConfidence) / 3.0;
    }

    private bool ShouldMakeAccusation()
    {
        if (suspectConfidence >= 100 && weaponConfidence >= 100 && roomConfidence >= 100)
            return true;
        if (GetOverallConfidence() >= 90)
            return UnityEngine.Random.value < 0.33f;
        return false;
    }

    //For board and GameController interfration

    /// <summary>TODO: connect to Board pathfinding once available</summary>
    private List<Vector2Int> GetLegalMoves(int diceRoll)
    {
        return new List<Vector2Int>();
    }

    /// <summary>TODO: connect to Board room/distance logic.</summary>
    private List<RoomCard> GetThreeClosestRooms()
    {
        return new List<RoomCard>();
    }

    private Vector2Int BestMoveTowardsTarget(List<Vector2Int> legalMoves)
    {
        if (legalMoves.Count == 0) return Vector2Int.zero;
        return legalMoves[0]; // TODO: distance heuristic
    }

    private void MoveTo(Vector2Int position)
    {
        // TODO: actual board movement
    }

    private bool IsInRoom()
    {
        return false; // TODO: board tile detection
    }

    private void MakeSuggestion(Suggestion suggestion)
    {
        Debug.Log("[AI ]" + ownPlayerIndex + "] suggestion: " +
        Safe(suggestion.GetCharacter()) + ", " +
        Safe(suggestion.GetWeapon()) + ", " +
        Safe(suggestion.GetRoom()));
        // TODO: gameController.HandleSuggestion(suggestion) when public
    }

    private void MakeAccusation(Suggestion suggestion)
    {
        Debug.Log("[AI ]" + ownPlayerIndex + "] ACCUSATION: " +
        Safe(suggestion.GetCharacter()) + ", " +
        Safe(suggestion.GetWeapon()) + ", " +
        Safe(suggestion.GetRoom()));
        // TODO: gameController.HandleAccusation(this, accusation) when public
    }

    private string Safe(object o) { return o!= null ? o.ToString() : "(null)";}

    private void EndTurn()
    {
        Debug.Log("[AI " + ownPlayerIndex + "] turn ended.");
        // TODO: gameController.NextPlayer() when public.
    }

    //For testing and debugging
    public DetectiveSheet GetSheetForDebug() { return detectiveSheet; }
    public double GetSuspectConfidence() { return suspectConfidence; }
    public double GetWeaponConfidence() { return weaponConfidence; }
    public double GetRoomConfidence() { return roomConfidence; }


    //Internal Types
    private class UnknownDisproval
    {
        public int DisproverIndex;
        public string[] CardNames; // [character, weapon, room] — any may be null
    }





}




