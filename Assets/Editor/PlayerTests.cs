using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTests
{

    [Test]
    public void HumanPlayer_CanBeCreated()
    {
        CharacterCard character = new CharacterCard("Miss Scarlett", null);
        HumanPlayer player = new HumanPlayer(0, character);

        Assert.IsNotNull(player);
    }

    [Test]
    public void AddCard_AddsCardToPlayerHand()
    {
        CharacterCard character = new CharacterCard("Miss Scarlett", null);

        HumanPlayer player = new HumanPlayer(0, character);

        WeaponCard card = new WeaponCard("Knife", null);

        player.AddCard(card);

        Assert.AreEqual(1, player.GetHand().Count);
    }

    [Test]
    public void AddCard_StoresCorrectCard()
    {
        CharacterCard character = new CharacterCard("Miss Scarlett", null);

        HumanPlayer player = new HumanPlayer(0, character);

        WeaponCard card = new WeaponCard("Knife", null);

        player.AddCard(card);

        Assert.AreEqual(card, player.GetHand()[0]);
    }
}
