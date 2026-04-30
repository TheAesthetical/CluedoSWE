using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents the full detective Sheet for one player
/// Hold one DecteiveCrdEntry per card
/// Each entry has fixed number of columns mathcing the UI
/// and know ehich those columns have either human or Ai players in this game
/// One DectiveSheet exist per player
/// </summary>
public class DetectiveSheet
{
    /// <summary>Number of buttons per row in the detective sheet UI.</summary>
    public const int DEFAULT_COLUMN_COUNT = 6;

    private List<DetectiveCardEntry> entries;
    private int columnCount;
    private int activePlayerCount;

    /// <summary>
    /// Creates a new sheet for a game with the given number of real players
    /// Columns count is defualt 6 to match the UI
    /// </summary>
    public DetectiveSheet(int activePlayerCount)
        : this(DEFAULT_COLUMN_COUNT, activePlayerCount) { }

    /// <summary>
    /// Creates a sheet with exact column count and active player count
    /// Can be used if the UI coloumns to could change size
    /// </summary>
    /// <param name="columnCount"></param>
    /// <param name="activePlayerCount"></param>
    public DetectiveSheet(int columnCount, int activePlayerCount)
    {
        this.columnCount = columnCount;
        this.activePlayerCount = activePlayerCount;
        entries = new List<DetectiveCardEntry>();
        AddDefaultCards();
    }

    public DetectiveSheet() : this(1, 1) { }


    /// <summary>
    /// Convenience: at game start, auto-cross every card in this player's own
    /// hand under their own column
    /// Call this once after the game deals cards
    /// </summary>
    /// <param name="hand">The players dealt cards</param>
    /// <param name="ownPlayerIndex">This players column index in the sheet</param>
    public void InitialiseFromHand(IEnumerable<Card> hand, int ownPlayerIndex)
    {
        if (hand == null) return;
        foreach (Card card in hand)
        {
            if (card != null) MarkAuto(card.ToString(), ownPlayerIndex);
        }
    }

    /// <summary>
    /// Adds all default Cluedo cards to the sheet
    /// </summary>
    private void AddDefaultCards()
    {
        // Suspects
        AddEntry("Miss Scarlett");
        AddEntry("Colonel Mustard");
        AddEntry("Mrs White");
        AddEntry("Reverend Green");
        AddEntry("Mrs Peacock");
        AddEntry("Professor Plum");

        // Weapons
        AddEntry("Candlestick");
        AddEntry("Dagger");
        AddEntry("Lead Pipe");
        AddEntry("Revolver");
        AddEntry("Rope");
        AddEntry("Wrench");

        // Rooms
        AddEntry("Kitchen");
        AddEntry("Ballroom");
        AddEntry("Conservatory");
        AddEntry("Dining Room");
        AddEntry("Billiard Room");
        AddEntry("Library");
        AddEntry("Lounge");
        AddEntry("Hall");
        AddEntry("Study");
    }

    private void AddEntry(string cardName)
    {
        entries.Add(new DetectiveCardEntry(cardName, columnCount, activePlayerCount));
    }

    /// <summary>
    /// Returns all card entries in the sheet
    /// </summary>
    /// <returns>List of detective card entries</returns>
    public List<DetectiveCardEntry> GetEntries() { return entries; }

    /// <summary>
    /// Number of UI columns per row
    /// </summary>
    public int GetColumnCount() { return columnCount; }

    /// <summary>
    /// Number of real players this game
    /// </summary>
    public int GetActivePlayerCount() { return activePlayerCount; }

    /// <summary>
    /// Look Up a card by entry name
    /// </summary>
    /// <param name="cardName">Card name</param>
    /// <returns>Return null if not found</returns>
    public DetectiveCardEntry GetEntryByName(string cardName)
    {
        foreach (DetectiveCardEntry entry in entries)
        {
            if (entry.GetCardName() == cardName) return entry;
        }
        return null;
    }


    /// <summary>
    /// System marks a card as known to be in a specific players hand
    /// (or shown by them)
    /// Used for auto-cross
    /// </summary>
    public void MarkAuto(string cardName, int playerIndex)
    {
        DetectiveCardEntry entry = GetEntryByName(cardName);
        if (entry != null) entry.MarkAuto(playerIndex);
        else Debug.LogWarning("DetectiveSheet: card not found: " + cardName);
    }

    /// <summary>
    /// overload: takes a Card object instead of a name
    /// </summary>
    public void MarkAuto(Card card, int playerIndex)
    {
        if (card == null) return;
        MarkAuto(card.ToString(), playerIndex);
    }


    /// <summary>
    /// Toggles the manual cross for a card in a players column
    /// </summary>
    public void ToggleManualCrossOut(DetectiveCardEntry entry, int playerIndex)
    {
        if (entry == null) return;
        if (entry.IsManualCrossedOut(playerIndex)) entry.UndoManual(playerIndex);
        else entry.MarkManual(playerIndex);
    }

    public void MarkAuto(string cardName) { MarkAuto(cardName, 0); }
    public void MarkAuto(Card card)
    {
        if (card != null) MarkAuto(card.ToString(), 0);
    }
    public void ToggleManualCrossOut(DetectiveCardEntry entry)
    {
        ToggleManualCrossOut(entry, 0);
    }

    //For AI


    /// <summary>
    /// Returns true if the Ai can rule this card out of the murder envolpe
    /// That happens when card is croosed in ANy active player column
    /// So it should be sombody holds it
    /// </summary>
    /// <param name="cardName">name of the card</param>
    /// <returns>Returns true if the Ai can rule this card out of the murder envolpe</returns>
    public bool IsRuledOutOfEnvelope(string cardName)
    {
        DetectiveCardEntry entry = GetEntryByName(cardName);
        if (entry == null) return false;
        return entry.IsAutoCrossedOutAnywhere();
    }



}