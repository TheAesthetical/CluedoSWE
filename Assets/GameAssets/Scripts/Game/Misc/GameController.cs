using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class <c>GameController</c> controls the flow of the game, including the setup, turns and win conditions. 
/// </summary>
public class GameController : MonoBehaviour
{
    private List<Player> players;
    private Deck deck;
    private Deck weaponsDeck;
    private Deck roomsDeck;
    private Deck charactersDeck;

    private MurderEnvelope murderEnvelope;
    
    private int currentPlayerIndex;
    private bool gameOver;

    // Dice Controller
    [SerializeField] private DiceController diceController;

    // WEAPON CARD SPRITES -----
    [SerializeField] private Sprite candlestickCardSprite;
    [SerializeField] private Sprite knifeCardSprite;
    [SerializeField] private Sprite leadpipeCardSprite;
    [SerializeField] private Sprite revolverCardSprite;
    [SerializeField] private Sprite ropeCardSprite;
    [SerializeField] private Sprite wrenchCardSprite;

    // CHARACTER CARD SPRITES -------
    [SerializeField] private Sprite mustardCardSprite;
    [SerializeField] private Sprite scarlettCardSprite;
    [SerializeField] private Sprite peacockCardSprite;
    [SerializeField] private Sprite whiteCardSprite;
    [SerializeField] private Sprite plumCardSprite;
    [SerializeField] private Sprite greenCardSprite;

    // ROOM CARD SPRITES -----
    [SerializeField] private Sprite ballroomCardSprite;
    [SerializeField] private Sprite billiardsCardSprite;
    [SerializeField] private Sprite conservatoryCardSprite;
    [SerializeField] private Sprite diningCardSprite;
    [SerializeField] private Sprite hallCardSprite; 
    [SerializeField] private Sprite kitchenCardSprite;
    [SerializeField] private Sprite libraryCardSprite;
    [SerializeField] private Sprite loungeCardSprite;
    [SerializeField] private Sprite studyCardSprite;

    // ------------------

    private static Player winner;

    /// <summary>
    /// Unity Start() method
    /// </summary>
    void Start()
    {
        diceController.OnRollComplete += HandleDiceResult;

        InitialiseGame();
        StartGame();
    }

    /// <summary>
    /// Sets up all of the game components and prepares the game.
    /// </summary>
    private void InitialiseGame()
    {
        players = new List<Player>();
        
        murderEnvelope = new MurderEnvelope();

        currentPlayerIndex = 0;
        gameOver = false;

        // Decks:
        weaponsDeck = new Deck();
        charactersDeck = new Deck();
        roomsDeck = new Deck();
        deck = new Deck();  
       
        // Call helper function to load the cards into three separate Unshuffled decks.
        LoadCards();

        // Call helper func. to setup the Murder Envelope
        SetupMurderEnvelope();

        // Call helper func. to setup main deck
        SetupMainDeck();

        // TODO:
        // - Create Players

        // Call function to deal the cards amongst the players
        DealCards();

    }

    /// <summary>
    /// Executes the current player's turn
    /// </summary>
    private void TakeTurn()
    {
        if (gameOver)
        {
            return;
        }

        Player currentPlayer = players[currentPlayerIndex];

        
        

        // TODO:
        // - Handle Dice Roll
        // - Handle Movement
        // - Handle Accusation
        // - Handle Suggestion
        // - End Turn

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
    /// A helper function that initialises the cards. The cards are put into three separate decks that are unshuffled.
    /// </summary>
    private void LoadCards()
    {
        // Weapons:
        weaponsDeck.AddCard(new WeaponCard("Candlestick", candlestickCardSprite));
        weaponsDeck.AddCard(new WeaponCard("Knife", knifeCardSprite));
        weaponsDeck.AddCard(new WeaponCard("Lead Pipe", leadpipeCardSprite));
        weaponsDeck.AddCard(new WeaponCard("Revolver", revolverCardSprite));
        weaponsDeck.AddCard(new WeaponCard("Rope", ropeCardSprite));
        weaponsDeck.AddCard(new WeaponCard("Wrench", wrenchCardSprite));

        // Characters:
        charactersDeck.AddCard(new CharacterCard("Colonel Mustard", mustardCardSprite));
        charactersDeck.AddCard(new CharacterCard("Miss Scarlett", scarlettCardSprite));
        charactersDeck.AddCard(new CharacterCard("Mrs Peacock", peacockCardSprite));
        charactersDeck.AddCard(new CharacterCard("Mrs White", whiteCardSprite));
        charactersDeck.AddCard(new CharacterCard("Professor Plum", plumCardSprite));
        charactersDeck.AddCard(new CharacterCard("Reverend Green", greenCardSprite));

        // Rooms:
        roomsDeck.AddCard(new RoomCard("Ballroom", ballroomCardSprite));
        roomsDeck.AddCard(new RoomCard("Billiards Room", billiardsCardSprite));
        roomsDeck.AddCard(new RoomCard("Dining Room", diningCardSprite));
        roomsDeck.AddCard(new RoomCard("Hall", hallCardSprite));
        roomsDeck.AddCard(new RoomCard("Kitchen", kitchenCardSprite));
        roomsDeck.AddCard(new RoomCard("Library", libraryCardSprite));
        roomsDeck.AddCard(new RoomCard("Lounge", loungeCardSprite));
        roomsDeck.AddCard(new RoomCard("Study", studyCardSprite));


    }

    /// <summary>
    /// A helper function that shuffles the three decks, and draws one card from each,
    /// and setting the contents of the murderEnvelope
    /// </summary>
    private void SetupMurderEnvelope()
    {   
        // Shuffle the decks
        weaponsDeck.Shuffle();
        roomsDeck.Shuffle();
        charactersDeck.Shuffle();

        // Draw one card from each
        CharacterCard character = (CharacterCard)charactersDeck.DrawCard();
        WeaponCard weapon = (WeaponCard)weaponsDeck.DrawCard();
        RoomCard room = (RoomCard)roomsDeck.DrawCard();

        murderEnvelope.SetCards(character, weapon, room);

    }

    /// <summary>
    /// Helper function to setup the main deck. Should only be called AFTER setting up Murder Envelope.
    /// </summary>
    private void SetupMainDeck()
    {
        Debug.Log("Character Deck: " + charactersDeck.ToString());

        AddAllCards(deck, charactersDeck);
        AddAllCards(deck, weaponsDeck);
        AddAllCards(deck, roomsDeck);

        deck.Shuffle();
    }

    private void DealCards()
    {
    
        int playerIndex = 0;

        Debug.Log("Deck: " + deck.ToString());

        if (players.Count == 0)
        {
            Debug.Log("No players!!");
            return;
        }

        while (deck.Count() > 0)
        {
        
            Card card = deck.DrawCard();  

            players[playerIndex].AddCard(card);


            playerIndex = (playerIndex + 1) % players.Count; // wraps around once end reached

        }

        Debug.Log("Cards dealt to players");
    }

    /// <summary>
    /// Helper function to add all cards from one deck into another.
    /// </summary>
    /// <param name="target"> The destination of the cards. </param>
    /// <param name="source"> The source of the cards. </param>
    private void AddAllCards(Deck target, Deck source)
    {

        while (source.Count() > 0)
        {
            target.AddCard(source.DrawCard());
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
    /// Moves to the next player, and starts the next turn. 
    /// </summary>
    private void EndTurn()
    {
        NextPlayer();
        TakeTurn();
    }

    /// <summary>
    /// Ends the game and announces the winner.
    /// </summary>
    /// <param name="winnerIn">The winning Player.</param>
    private void EndGame(Player winnerIn)
    {
        gameOver = true;
        SetWinner(winnerIn);

        Debug.Log("Game Over! Winner: " + winnerIn);
    }

    private void HandleDiceResult(int roll)
    {
        

        Debug.Log("rolled " + roll);

        //HandleMovement(currentPlayer, roll);


        // move to the next player, and start their turn.
        EndTurn();
    }

    public void OnRollDicePressed()
    {
        diceController.RollDice();
    }

    public static void SetWinner(Player player)
    {
        winner = player;
    }

    public static Player GetWinner()
    {   
        return winner;
    }

}