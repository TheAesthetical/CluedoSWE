using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class <c>GameController</c> controls the flow of the game, including the setup, turns and win conditions. 
/// </summary>
public class GameController : MonoBehaviour
{
    private List<Player> players;
    private Deck deck;
    private MurderEnvelope murderEnvelope;
    private Dice dice;

    private int currentPlayerIndex;
    private bool gameOver;

    /// <summary>
    /// Unity Start() method
    /// </summary>
    void Start()
    {
        InitialiseGame();
        StartGame();
    }

    /// <summary>
    /// Sets up all of the game components and prepares the game.
    /// </summary>
    private void InitialiseGame()
    {
        players = new List<Player>();
        deck = new Deck();
        murderEnvelope = new MurderEnvelope();
        dice = new Dice();

        currentPlayerIndex = 0;
        gameOver = false;

        // TODO: 
        // - Load Cards
        // - Create Players
        // - Initialise Murder Envelope
        // - Shuffle and Deal Cards
    }

    /// <summary>
    /// Begins the game loop.
    /// </summary>
    private void StartGame()
    {
        Debug.Log("Game Started");

        // TODO:
        // - Begin First Turn
        // - Game Loop?
    }

    /// <summary>
    /// Executes the current player's turn
    /// </summary>
    private void TakeTurn()
    {
        Player currentPlayer = players[currentPlayerIndex];

        // TODO:
        // - Handle Dice Roll
        // - Handle Movement
        // - Handle Accusation
        // - Handle Suggestion
        // - End Turn

    }

    /// <summary>
    /// Handles a player's suggestion.
    /// </summary>
    /// <param name="suggestion">The <c>Suggestion</c> the player makes. </param>
    private void HandleSuggestion(Suggestion suggestion)
    {
        // TODO:
        // - Ask other player's to disprove
        // - Reveal one matching card (if possible)
        // - End Suggestion phase

    }

    /// <summary>
    /// Handles a player's accusation. 
    /// </summary>
    /// <param name="currentPlayer"> The current player. </param>
    /// <param name="accusation">The <c>Suggestion</c> class is used to represent the Accusation. </param>
    private void HandleAccusation(Player currentPlayer, Suggestion accusation)
    {   

        bool correct = murderEnvelope.CheckAccusation(accusation.GetCharacter(), accusation.GetWeapon(), accusation.GetRoom());

        if (correct)
        {
            EndGame(currentPlayer);
        } else
        {
            //TODO:
            // - Eliminate the player
            
        }
    }

    /// <summary>
    /// Moves to the next player
    /// </summary>
    private void NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex+1) % players.Count; // handles wrapping to index 0 at the end of the list
    }

    /// <summary>
    /// Ends the game and announces the winner.
    /// </summary>
    /// <param name="winner">The winning Player.</param>
    private void EndGame(Player winner)
    {
        gameOver = true;
        Debug.Log("Game Over! Winner: " + winner);
    }

}