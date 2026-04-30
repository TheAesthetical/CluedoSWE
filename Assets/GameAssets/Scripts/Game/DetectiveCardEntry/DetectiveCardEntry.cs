using TMPro;
using UnityEngine;

/// <summary>
/// Represents a single card in the detective sheet (e.g. Ms Scarlett)
/// The sheet UI has fixed number of player columns 6 rest columns will be inactive
/// 
/// For each column this entry tracks three things:
/// active: is there player in this col
/// autoCrossedOut: the game auto marked this card
/// manualCrossedOut: the human player ticked the cross button
/// </summary>
public class DetectiveCardEntry
{
    private string cardName;
    
    private bool[] active;
    private bool[] autoCrossedOut;
    private bool[] manualCrossedOut;

    /// <summary>
    /// Creates a new entry.
    /// columnCount should mathc the number of button per row in the UI
    /// activePlayercount is how many of those columns are real players (Ai and human) this game
    /// </summary>
    /// <param name="cardName">Name of the card</param>
    /// <param name="columnCount">Totla UI Colums</param>
    /// <param name="activePlayerCount">Real players this game</param>
    public DetectiveCardEntry(string cardName, int columnCount, 
    int activePlayerCount)
    {
        this.cardName = cardName;
        active = new bool[columnCount];
        autoCrossedOut = new bool[columnCount];
        manualCrossedOut = new bool[columnCount]; 

        // First N columns are real players, the rest are empty seats.
        int realPlayers = Mathf.Clamp(activePlayerCount, 0, columnCount);
        for (int i = 0; i < realPlayers; i++)
        {
            active[i] = true;
        }
    }

    /// <summary>
    /// Backwards compatible constructor: assumes a single active column.
    /// Prefer the (cardName, columnCount, activePlayerCount) form.
    /// </summary>
    public DetectiveCardEntry(string cardName) : this(cardName, 1, 1) { }

    /// <summary>
    /// Returns the name of the card
    /// </summary>
    public string GetCardName() { return cardName; }

    /// <summary>
    /// Total number of UI columns this entry tracks
    /// </summary>
    public int GetColumnCount() { return active.Length; }

    /// <summary>
    /// True if this column corresponds to a real player
    /// </summary>
    /// <param name="playerIndex">Player of postion</param>
    /// <returns>True if the corresponds to a real player</returns>
    public bool IsColumnActive(int playerIndex)
    {
        if (!IsValidIndex(playerIndex)) return false;
        return active[playerIndex];
    }

    /// <summary>
    /// True if this column should be displayed by the UI
    /// Includes inactive columns. (so empty seats are x out)
    /// and active columns that have been auto/manual crossed out
    /// </summary>
    /// <param name="playerIndex">Postion of player</param>
    /// <returns></returns>
    public bool ShouldDisplayCross(int playerIndex)
    {
        if (!IsValidIndex(playerIndex)) return false;
        if (!active[playerIndex]) return true;
        return autoCrossedOut[playerIndex] || manualCrossedOut[playerIndex];
    }

    /// <summary>
    /// True if this column is meaningfully crossed out for deduction purposes
    /// Inactive columns return false: they aren't real information
    /// </summary>
    /// <param name="playerIndex">Postion of player</param>
    /// <returns>Returns true if column crossed out</returns>
    public bool IsCrossedOut(int playerIndex)
    {
        if (!IsValidIndex(playerIndex)) return false;
        if (!active[playerIndex]) return false;
        return autoCrossedOut[playerIndex] || manualCrossedOut[playerIndex];
    }

    /// <summary>
    /// Checks if the card been Auto crossed out
    /// </summary>
    /// <param name="playerIndex">Player postion</param>
    /// <returns>reutrns true if auto crossed out</returns>
    public bool IsAutoCrossedOut(int playerIndex)
    {
        if (!IsValidIndex(playerIndex)) return false;
        return active[playerIndex] && autoCrossedOut[playerIndex];
    }

    /// <summary>
    /// Returns if the card is manually crossed out
    /// </summary>
    /// <param name="playerIndex">Player postion</param>
    /// <returns>True if manually crossed out</returns>
    public bool IsManualCrossedOut(int playerIndex)
    {
        if (!IsValidIndex(playerIndex)) return false;
        return active[playerIndex] && manualCrossedOut[playerIndex];
    }

    /// <summary>
    /// System marks this column crossed
    /// inactive columns are not included
    /// </summary>
    public void MarkAuto(int playerIndex)
    {
        if (!IsValidIndex(playerIndex)) return;
        if (!active[playerIndex]) return;
        autoCrossedOut[playerIndex] = true;
    }


    /// <summary>
    /// Marks the card as manually crossed out by
    /// the player
    /// inactive columns are not included
    /// </summary>
    public void MarkManual(int playerIndex)
    {
        if (!IsValidIndex(playerIndex)) return;
        if (!active[playerIndex]) return;
        manualCrossedOut[playerIndex] = true;
    }

    /// <summary>
    /// Human player un-ticks the cross button
    /// </summary>
    /// <param name="playerIndex">Player postion</param>
    public void UndoManual(int playerIndex)
    {
        if (!IsValidIndex(playerIndex)) return;
        if (!active[playerIndex]) return;
        manualCrossedOut[playerIndex] = false;
    }

    /// <summary>
    ///  True if a card been automatically crossed out
    /// </summary>
    /// <returns>Return true if card been crossed out automatically</returns>
    public bool IsAutoCrossedOutAnywhere()
    {
        for (int i = 0; i < active.Length; i++)
        {
            if (active[i] && autoCrossedOut[i]) return true;
        }
        return false;
    }

    /// <summary>
    /// True if any active column (auto or manual) is crossed
    /// </summary>
    public bool IsCrossedOutAnywhere()
    {
        for (int i = 0; i < active.Length; i++)
        {
            if (!active[i]) continue;
            if (autoCrossedOut[i] || manualCrossedOut[i]) return true;
        }
        return false;
    }

    public bool IsCrossedOut() { return IsCrossedOutAnywhere(); }
    public bool IsAutoCrossedOut() { return IsAutoCrossedOutAnywhere(); }
    public bool IsManualCrossedOut()
    {
        return manualCrossedOut.Length > 0 && active[0] && manualCrossedOut[0];
    }
    public void MarkAuto() { MarkAuto(0); }
    public void MarkManual() { MarkManual(0); }
    public void UndoManual() { UndoManual(0); }

    private bool IsValidIndex(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= active.Length)
        {
            Debug.LogWarning("DetectiveCardEntry: invalid index " + playerIndex
                + " for card " + cardName + " (have " + active.Length + " columns)");
            return false;
        }
        return true;
    }

}

