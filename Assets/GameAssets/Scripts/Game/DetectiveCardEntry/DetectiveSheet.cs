using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents the full detective Sheet
/// Store all card entries
/// provides operations for modifying their state
/// </summary>
public class DetectiveSheet
{
    private List<DetectiveCardEntry> entries;

    /// <summary>
    /// Creates a new sheet and populates it with default cards
    /// </summary>
    public DetectiveSheet()
    {
        entries = new List<DetectiveCardEntry>();
        AddDefaultCards();
    }

    /// <summary>
    /// Adds all defualt Cluedo cards to the sheet
    /// </summary>
    private void AddDefaultCards()
    {
        //Players
        entries.Add(new DetectiveCardEntry("Miss Scarlett"));
        entries.Add(new DetectiveCardEntry("Colonel Mustard"));
        entries.Add(new DetectiveCardEntry("Mrs White"));
        entries.Add(new DetectiveCardEntry("Reverend Green"));
        entries.Add(new DetectiveCardEntry("Mrs Peacock"));
        entries.Add(new DetectiveCardEntry("Professor Plum"));

        //Weapons
        entries.Add(new DetectiveCardEntry("Candlestick"));
        entries.Add(new DetectiveCardEntry("Dagger"));
        entries.Add(new DetectiveCardEntry("Lead Pipe"));
        entries.Add(new DetectiveCardEntry("Revolver"));
        entries.Add(new DetectiveCardEntry("Rope"));
        entries.Add(new DetectiveCardEntry("Wrench"));

        //Rooms
        entries.Add(new DetectiveCardEntry("Kitchen"));
        entries.Add(new DetectiveCardEntry("Ballroom"));
        entries.Add(new DetectiveCardEntry("Conservatory"));
        entries.Add(new DetectiveCardEntry("Dining Room"));
        entries.Add(new DetectiveCardEntry("Billiard Room"));
        entries.Add(new DetectiveCardEntry("Library"));
        entries.Add(new DetectiveCardEntry("Lounge"));
        entries.Add(new DetectiveCardEntry("Hall"));
        entries.Add(new DetectiveCardEntry("Study"));
    }

    /// <summary>
    /// Returns all card entries in the sheet
    /// </summary>
    /// <returns>List of detective card entries</returns>
    public List<DetectiveCardEntry> GetEntries()
    {
        return entries;
    }

    /// <summary>
    /// Toggles the mnaual crossout state of a given card entry
    /// If it's manually crossed out: it will be undone
    /// Else: it will be marked as manually crossed oout
    /// </summary>
    /// <param name="entry">The card entry to modify</param>
    public void ToggleManualCrossOut(DetectiveCardEntry entry)
    {
        if (entry.IsManualCrossedOut())
            entry.UndoManual();
        else
            entry.MarkManual();
    }

}