using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Class <c> Player </c> is an abstract class that represents a generic card. It provides the base for each player type. 
/// </summary>
public abstract class Player
{
    private int ID;
    private CharacterCard character;
    private int position;
    private List<Card> hand = new List<Card>();
    private bool eliminated;

	/// <summary>
	/// Initialises a new instance of the Card class. Set to protected as this should not be instantiated directly!
	/// </summary>
	/// <param name="ID">The player's ID.</param>
	/// <param name="character">The player's <c>CharacterCard</c>.</param>
	protected Player(int ID, CharacterCard character)
    {
        this.ID = ID;
        this.character = character;
        this.position = 0;
        this.eliminated = false;
    }

    /// <summary>
    /// Makes a suggestion.
    /// </summary>
    public Suggestion MakeSuggestion(CharacterCard character, RoomCard room, WeaponCard weapon)
    {
        return new Suggestion(character, room, weapon);
    }

    /// <summary>
    /// Makes an accusation.
    /// </summary>
    public void MakeAccusation()
    {
        // empty for now, probably the same as suggestion
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
    public bool CanDisprove(Suggestion suggestion) // string is placeholder until suggestion class
    {
        foreach (Card card in hand)
        {
            if (card == suggestion.GetCharacter() || card == suggestion.GetWeapon() || card == suggestion.GetRoom())
            {
                return false;
            }
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

    /// <summary>
    /// A method to return the Character card of the current player.
    /// </summary>
    /// <returns>A CharacterCard. </returns>
    public CharacterCard GetCharacter()
    {
        return character;
    }

    /// <summary>
    /// A method to add a card to the player's hand.
    /// </summary>
    /// <param name="card"> The card to be added. </param>
    public void AddCard(Card card)
    {
        hand.Add(card);
    }

    /// <summary>
    /// Returns the hand.
    /// </summary>
    public List<Card> GetHand()
    {
        return hand;
    }

}
