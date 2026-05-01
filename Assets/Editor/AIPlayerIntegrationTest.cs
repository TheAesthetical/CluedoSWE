using System.Collections.Generic;
using NUnit.Framework;

/// <summary>
/// Integration tests for AIPlayer
/// Demonstrates the GameController should work
/// </summary>
public class AIPlayerIntegrationTest
{
    private CharacterCard MakeChar(string n) { return new CharacterCard(n, null); }
    private WeaponCard MakeWeap(string n) { return new WeaponCard(n, null); }
    private RoomCard MakeRoom(string n) { return new RoomCard(n, null); }

    /// <summary>
    /// At game start: Initialise + OnHandDealt
    /// The AI auto crosses its own cards ruling them out of the envelope
    /// </summary>
    [Test]
    public void OnHandDealt_AutoCrossesOwnHand()
    {
        AIPlayer ai = new AIPlayer(0, MakeChar("Miss Scarlett"));
        ai.Initialise(playerIndex: 0, totalActivePlayers: 3);

        ai.OnHandDealt(new List<Card> {
            MakeChar("Miss Scarlett"),
            MakeWeap("Knife"),
            MakeRoom("Library")
        });

        DetectiveSheet sheet = ai.GetSheetForDebug();

        // Cards in our hand should be ruled out of the envelope (we have them)
        Assert.IsTrue(sheet.IsRuledOutOfEnvelope("Miss Scarlett"));
        Assert.IsTrue(sheet.IsRuledOutOfEnvelope("Knife"));
        Assert.IsTrue(sheet.IsRuledOutOfEnvelope("Library"));

        // Other cards still candidates
        Assert.IsFalse(sheet.IsRuledOutOfEnvelope("Professor Plum"));
    }

    /// <summary>
    /// During play: OnSuggestionMade drives smart deduction
    /// Players who pass on a suggestion dont hold any of those three cards
    /// </summary>
    [Test]
    public void OnSuggestionMade_DeductsFromPassersAndShownCards()
    {
        AIPlayer ai = new AIPlayer(0, MakeChar("Miss Scarlett"));
        ai.Initialise(playerIndex: 0, totalActivePlayers: 3);
        ai.OnHandDealt(new List<Card>());

        // disproverIndex = -1 means everyone passed.
        ai.OnSuggestionMade(
            suggestionIndex: 1,
            suggestion: new Suggestion(MakeChar("Professor Plum"), MakeRoom("Hall"), MakeWeap("Rope")),
            disproverIndex: -1,
            shownCard: null);

        DetectiveSheet sheet = ai.GetSheetForDebug();

        // Player 2 was asked and passed 
        Assert.IsTrue(sheet.GetEntryByName("Professor Plum").IsKnownNotHeldBy(2));
        Assert.IsTrue(sheet.GetEntryByName("Hall").IsKnownNotHeldBy(2));
        Assert.IsTrue(sheet.GetEntryByName("Rope").IsKnownNotHeldBy(2));

        //AI suggests, player 1 disproves by showing the Knife directly
        ai.OnSuggestionMade(
            suggestionIndex: 0,
            suggestion: new Suggestion(MakeChar("Mrs White"), MakeRoom("Kitchen"), MakeWeap("Knife")),
            disproverIndex: 1,
            shownCard: MakeWeap("Knife"));

        Assert.IsTrue(sheet.IsRuledOutOfEnvelope("Knife"));
    }

    /// <summary>
    /// example showing the full call sequence GameController would
    /// use during a normal game
    /// Read this if you're integrating AIPlayer
    /// </summary>
    [Test]
    public void FullGameFlow_IntegrationExample()
    {
        //GameStart
        AIPlayer ai = new AIPlayer(0, MakeChar("Miss Scarlett"));
        ai.Initialise(playerIndex: 0, totalActivePlayers: 3);
        ai.SetStrategy(StrategyType.Safe);
        ai.OnHandDealt(new List<Card> {
            MakeChar("Miss Scarlett"),
            MakeWeap("Candlestick"),
            MakeRoom("Kitchen")
        });

        // Round 1: AI just watches 
        // Player 1 suggests player 2 disproves
        // AI didn't see the card -> shownCard is null
        ai.OnSuggestionMade(
            suggestionIndex: 1,
            suggestion: new Suggestion(MakeChar("Professor Plum"), MakeRoom("Library"), MakeWeap("Rope")),
            disproverIndex: 2,
            shownCard: null);

        // Round 2: AI made the suggestion was shown a card 
        // We pass the actual card because the AI saw it
        ai.OnSuggestionMade(
            suggestionIndex: 0,
            suggestion: new Suggestion(MakeChar("Reverend Green"), MakeRoom("Library"), MakeWeap("Knife")),
            disproverIndex: 1,
            shownCard: MakeRoom("Library"));

        //Verify the AI learnt something 
        DetectiveSheet sheet = ai.GetSheetForDebug();
        Assert.IsTrue(sheet.IsRuledOutOfEnvelope("Library"),
            "Library should be ruled out - player 1 showed it to us");

        // AIs turn 
        // TODO
    }

    /// <summary>
    /// Deduction test
    /// </summary>
    [Test]
    public void UnknownCard_DeducedThroughElimination()
    {
        AIPlayer ai = new AIPlayer(0, MakeChar("Miss Scarlett"));
        ai.Initialise(playerIndex: 0, totalActivePlayers: 3);
        ai.OnHandDealt(new List<Card>());

        //Ai not shown the disproval
        ai.OnSuggestionMade(
            suggestionIndex: 1,
            suggestion: new Suggestion(MakeChar("Mrs Peacock"), MakeRoom("Lounge"), MakeWeap("Wrench")),
            disproverIndex: 2,
            shownCard: null);

        DetectiveSheet sheet = ai.GetSheetForDebug();
        Assert.IsFalse(sheet.GetEntryByName("Wrench").IsAutoCrossedOut(2),
        "Wretch shouldn't be auto crossed out");
        
        //Nobody to disprove player 2 does not have Peacock
        ai.OnSuggestionMade(
            suggestionIndex: 0,
            suggestion: new Suggestion(MakeChar("Mrs Peacock"), MakeRoom("Hall"), MakeWeap("Rope")),
            disproverIndex: -1,
            shownCard: null);

        //Nobody disproves: player 2 does not have Lounge
        ai.OnSuggestionMade(
            suggestionIndex: 0,
            suggestion: new Suggestion(MakeChar("Mrs White"), MakeRoom("Lounge"), MakeWeap("Candlestick")),
            disproverIndex: -1,
            shownCard: null);

        Assert.IsTrue(sheet.GetEntryByName("Wrench").IsAutoCrossedOut(2),
        "Ai should have deduced p2 holds the wrench");
        Assert.IsTrue(sheet.IsRuledOutOfEnvelope("Wrench"),
        "Wrench should be ruled out");
    }
}
