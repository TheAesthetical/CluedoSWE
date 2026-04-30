using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CardTests
{

    [Test]
    public void WeaponCard_StoresCorrectName()
    {
        WeaponCard card = new WeaponCard("Knife", null);

        Assert.AreEqual("Knife", card.CardName);
    }

    [Test]
    public void RoomCard_StoresCorrectName()
    {
        RoomCard card = new RoomCard("Kitchen", null);
        Assert.AreEqual("Kitchen", card.CardName);
    }

    [Test]
    public void CharacterCard_StoresCorrectName()
    {
        CharacterCard card = new CharacterCard("Miss Scarlett", null);
        Assert.AreEqual("Miss Scarlett", card.CardName);
    }

    [Test]
    public void Card_ToString_ReturnsName()
    {
        WeaponCard card = new WeaponCard("Rope", null);
        Assert.AreEqual("Rope", card.ToString());
    }
}
