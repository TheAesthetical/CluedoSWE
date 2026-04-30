using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DeckTests
{

    [Test]
    public void AddCard_IncreasesDeckCount()
    {
        Deck deck = new Deck();

        deck.AddCard(new WeaponCard("Knife", null));

        Assert.AreEqual(1, deck.Count());
    }

    [Test]
    public void DrawCard_ReturnsAddedCard()
    {
        Deck deck = new Deck();

        WeaponCard card = new WeaponCard("Knife", null);

        deck.AddCard(card);

        Card drawnCard = deck.DrawCard();

        Assert.AreEqual(card, drawnCard);
    }
    
    [Test]
    public void DrawCard_DecreasesDeckCount()
    {
        Deck deck = new Deck();

        deck.AddCard(new WeaponCard("Knife", null));
        deck.AddCard(new WeaponCard("Rope", null));

        deck.DrawCard();

        Assert.AreEqual(1, deck.Count());
    }

    [Test]
    public void DrawCard_EmptyDeck_ReturnsNull()
    {
        Deck deck = new Deck();

        Card drawnCard = deck.DrawCard();

        Assert.IsNull(drawnCard);
    }

    [Test]
    public void Shuffle_DoesNotChangeDeckCount()
    {
        Deck deck = new Deck();

        deck.AddCard(new WeaponCard("Knife", null));
        deck.AddCard(new WeaponCard("Rope", null));
        deck.AddCard(new RoomCard("Kitchen", null));

        int beforeShuffle = deck.Count();

        deck.Shuffle();

        Assert.AreEqual(beforeShuffle, deck.Count());
    }

}
