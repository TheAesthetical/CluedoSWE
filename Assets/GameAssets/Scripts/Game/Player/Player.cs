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

    /// <summary>
    /// Sets up the player at game start
    /// Call once berfore cards are delt
    /// </summary>
    /// <param name="playerIndex">Player index</param>
    /// <param name="totalActivePlayers">How many active players</param>
    public virtual void Initialise(int playerIndex, int totalActivePlayers) { }
    
    /// <summary>
    /// Call once cards are dealt
    /// This auto crosses out all cards in player own hand
    /// Override to react to your hand
    /// </summary>
    /// <param name="dealtCards">Cards dealt</param>
    public virtual void OnHandDealt(List<Card> cards) { }

    /// <summary>
    /// Call after every suggestion by anyone
    /// Helps the deduction
    /// disproverIndex == -1 means nobody could disprove
    /// shownCard: only not null if Ai was suggesster/disprover
    /// Null being passed means well record the disproval as "unknwn which card"
    /// Override to track deduction
    /// </summary>
    /// <param name="suggestionIndex">Index of player made the suggestion</param>
    /// <param name="suggestion">Suggestion made</param>
    /// <param name="disproverIndex">Index of the disporver or -1 if nobody disproved</param>
    /// <param name="shownCard">The card shown (if Known) or null</param>
    public virtual void OnSuggestionMade(int suggesterIndex, Suggestion suggestion,
    int disproverIndex, Card shownCard) { }

    /// <summary>
    /// Call when its this players turn
    /// Override to drive the turn
    /// </summary>
    /// <param name="gameController"></param>
    /// <param name="dice"></param>
    public virtual void TakeTurn(GameController gameController, Dice dice) { }
                
}
