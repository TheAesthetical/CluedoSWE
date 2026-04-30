using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MurderEnvelopeTests
{

    [Test]
    public void CheckAccusation_CorrectAccusation_ReturnsTrue()
    {
        MurderEnvelope envelope = new MurderEnvelope();

        CharacterCard character = new CharacterCard("Miss Scarlett", null);
        WeaponCard weapon = new WeaponCard("Knife", null);
        RoomCard room = new RoomCard("Kitchen", null);

        envelope.SetCards(character, weapon, room);

        bool result = envelope.CheckAccusation(character, weapon, room);

        Assert.IsTrue(result);
    }

    [Test]
    public void CheckAccusation_WrongCharacter_ReturnsFalse()
    {
        MurderEnvelope envelope = new MurderEnvelope();

        CharacterCard correctCharacter = new CharacterCard("Miss Scarlett", null);
        WeaponCard weapon = new WeaponCard("Knife", null);
        RoomCard room = new RoomCard("Kitchen", null);

        envelope.SetCards(correctCharacter, weapon, room);

        CharacterCard wrongCharacter = new CharacterCard("Professor Plum", null);

        bool result = envelope.CheckAccusation(wrongCharacter, weapon, room);

        Assert.IsFalse(result);
    }

    [Test]
    public void CheckAccusation_WrongWeapon_ReturnsFalse()
    {
        MurderEnvelope envelope = new MurderEnvelope();

        CharacterCard character = new CharacterCard("Miss Scarlett", null);
        WeaponCard correctWeapon = new WeaponCard("Knife", null);
        RoomCard room = new RoomCard("Kitchen", null);

        envelope.SetCards(character, correctWeapon, room);

        WeaponCard wrongWeapon = new WeaponCard("Rope", null);

        bool result = envelope.CheckAccusation(character, wrongWeapon, room);

        Assert.IsFalse(result);
    }

    [Test]
    public void CheckAccusation_WrongRoom_ReturnsFalse()
    {
        MurderEnvelope envelope = new MurderEnvelope();

        CharacterCard character = new CharacterCard("Miss Scarlett", null);
        WeaponCard weapon = new WeaponCard("Knife", null);
        RoomCard correctRoom = new RoomCard("Kitchen", null);

        envelope.SetCards(character, weapon, correctRoom);

        RoomCard wrongRoom = new RoomCard("Hall", null);

        bool result = envelope.CheckAccusation(character, weapon, wrongRoom);

        Assert.IsFalse(result);
    }

}
