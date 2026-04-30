using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SuggestionTests
{

    [Test]
    public void Suggestion_StoresCharacter()
    {
        CharacterCard character = new CharacterCard("Miss Scarlett", null);
        WeaponCard weapon = new WeaponCard("Knife", null);
        RoomCard room = new RoomCard("Kitchen", null);

        Suggestion suggestion = new Suggestion(character, room, weapon);

        Assert.AreEqual(character, suggestion.GetCharacter());
        
    }

    [Test]
    public void Suggestion_StoresWeapon()
    {
        CharacterCard character = new CharacterCard("Miss Scarlett", null);
        WeaponCard weapon = new WeaponCard("Knife", null);
        RoomCard room = new RoomCard("Kitchen", null);

        Suggestion suggestion = new Suggestion(character, room, weapon);

        Assert.AreEqual(weapon, suggestion.GetWeapon());
        
    }

    [Test]
    public void Suggestion_StoresRoom()
    {
        CharacterCard character = new CharacterCard("Miss Scarlett", null);
        WeaponCard weapon = new WeaponCard("Knife", null);
        RoomCard room = new RoomCard("Kitchen", null);

        Suggestion suggestion = new Suggestion(character, room, weapon);

        Assert.AreEqual(room, suggestion.GetRoom());
        
    }

}
