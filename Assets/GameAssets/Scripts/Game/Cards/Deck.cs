using UnityEngine;
using System;
using System.Collections.Generic;
using Random = System.Random;
/// <summary>
/// Class <c>Deck</c> represents a deck of cards used in the game. The deck supports adding cards, removing them,
/// and shuffling the deck.
/// </summary>
public class Deck
{
    private List<Card> cards;
    private Random random; 

    /// <summary>
    /// Creates a new instance of the <c> Deck </c>
    /// </summary>
    public Deck()
    {
        cards = new List<Card>();
        random = new Random();

    }

    /// <summary>
    /// Adds a new <c>Card</c> to the deck.
    /// </summary>
    /// <param name="newCard">The new <c>Card</c> to be added. </param>
    public void AddCard(Card newCard)
    {
        cards.Add(newCard);
    }

    /// <summary>
    /// Randomises the order of the cards in the deck using the Fisher-Yates shuffle.
    /// </summary>
    public void Shuffle()
    {
        int n = cards.Count;

        while (n > 1)
        {
            n--;
            int k = random.Next(n+1);

            Card temp = cards[k]; 
            cards[k] = cards[n]; 
            cards[n] = temp;
        }

    }

    /// <summary>
    /// Draws the top card from the deck. Where top is index 0.
    /// </summary>
    /// <returns> The <c>Card</c> drawn from the deck, or null if the deck is empty. </returns>
    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            return null;
        }

        Card top = cards[0];
        cards.RemoveAt(0);
        return top;
    }
    
    /// <summary>
    /// Gets the number of cards currently in the deck.
    /// </summary>
    /// <returns> An <c>int</c> representing the number of cards in the deck. </returns>
    public int Count()
    {
        return cards.Count;
    }
}