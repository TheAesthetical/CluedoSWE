using UnityEngine;

/// <summary>
/// Class <c> Player </c> is an abstract class that represents a generic card. It provides the base for each card type. 
/// </summary>
public abstract class Player
{
    private int ID;
    private CharacterCard character; // maybe in a PlayerController class instead
    private int position;
    private Card[] hand;
    private bool eliminated; // maybe, might use a queue and dequeue when eliminated

    /// <summary>
    /// Makes a suggestion.
    /// </summary>
    public void MakeSuggestion()
    {

    }

    /// <summary>
    /// Makes an accusation.
    /// </summary>
    public void MakeAccusation()
    {

    }

    /// <summary>
    /// Shows the card from the suggestion
    /// </summary>
    public void ShowCard()
    {
        // maybe not needed if we just return the card from canDisprove()?
    }

    /// <summary>
    /// Returns whether the Player can disprove the accusation against them.
    /// </summary>
    /// <param name="suggestion">The suggestion being made.</param>
    /// <returns>Returns a bool representing whether the player matches the accusation.</returns>
    public bool CanDisprove(string suggestion) // string is placeholder until suggestion class
    {
        foreach (Card card in hand)
        {
            // if (card == suggestion.getCharacter() || card == suggestion.getWeapon() || card == suggestion.getLocation())
            // {
            //  return false;
            // }
        }
        return true;
    }

    /// <summary>
    /// Eliminates the Player.
    /// </summary>
    public void Eliminate()
    {
        eliminated = true;
    }
}
