using UnityEngine;

/// <summary>
/// Represents a single card in the detective sheet (e.g. Ms Scarlett)
/// Tracks whether the card has been crossed out automatically
/// or manually
/// </summary>
public class DetectiveCardEntry
{
    private string cardName;
    private bool autoCrossedOut;
    private bool manualCrossedOut;

    /// <summary>
    /// Creates a nre detective card entry
    /// </summary>
    /// <param name="cardName">Name of the card</param>
    public DetectiveCardEntry(string cardName)
    {
        this.cardName = cardName;
        autoCrossedOut = false;
        manualCrossedOut = false;
    }

    /// <summary>
    /// Get the name of the card
    /// </summary>
    /// <returns>Name of the card</returns>
    public string GetCardName() { return cardName; }

    /// <summary>
    /// Returns the card is crossed out either automatically
    /// or manually
    /// </summary>
    /// <returns>True if crossed out</returns>
    public bool IsCrossedOut()
    {
        return autoCrossedOut || manualCrossedOut;
    }

    /// <summary>
    /// Returns if the card is manually crossed out
    /// </summary>
    /// <returns>True if manually crossed out</returns>
    public bool IsManualCrossedOut()
    {
        return manualCrossedOut;
    }

    /// <summary>
    /// Marks the card as automatically crossed out
    /// The cards player has seen
    /// </summary>
    public void MarkAuto()
    {
        autoCrossedOut = true;
    }

    /// <summary>
    /// Marks the card as manually crossed out by
    /// the player
    /// </summary>
    public void MarkManual()
    {
        manualCrossedOut = true;
    }

    /// <summary>
    /// Removes the manual cross mark
    /// </summary>
    public void UndoManual()
    {
        manualCrossedOut = false;
    }

}

