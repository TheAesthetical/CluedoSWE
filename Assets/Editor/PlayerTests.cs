using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTests
{

    [Test]
    public void HumanPlayer_CanBeCreated()
    {
        HumanPlayer player = new HumanPlayer();

        Assert.IsNotNull(player);
    }

    [Test]
    public void AddCard_AddsCardToPlayerHand()
    {
        HumanPlayer player = new HumanPlayer();

        WeaponCard card = new WeaponCard("Knife", null);

        player.AddCard(card);

        Assert.AreEqual(1, player.GetHand().Count);
    }

    [Test]
    public void AddCard_StoresCorrectCard()
    {
        HumanPlayer player = new HumanPlayer();

        WeaponCard card = new WeaponCard("Knife", null);

        player.AddCard(card);

        Assert.AreEqual(card, player.GetHand()[0]);
    }
}
