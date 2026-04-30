using UnityEngine;
using Random = System.Random;

/// <summary>
/// Class <c>Suggestion</c> represents a suggestion/accusation made by a player. 
/// </summary>
public class Suggestion
{   
    private CharacterCard character;
    private WeaponCard weapon;
    private RoomCard room;


    /// <summary>
    /// Creates a new instance of the <c>Suggestion</c> class.
    /// </summary>
    /// <param name="characterIn">The suggested/accused character. </param>
    /// <param name="roomIn"> The suggested/accused room. </param>
    /// <param name="weaponIn"> The suggested/accused weapon. </param>
    public Suggestion(CharacterCard characterIn, RoomCard roomIn, WeaponCard weaponIn) 
    {
        character = characterIn;
        weapon = weaponIn;
        room = roomIn;
    }

    /// <summary>
    /// Gets the Character Card contained. 
    /// </summary>
    /// <returns> The <c>CharacterCard</c>.</returns>
    public CharacterCard GetCharacter()
    {
        return character;
    }

    /// <summary>
    /// Gets the WeaponCard contained.
    /// </summary>
    /// <returns>The <c>WeaponCard</c>.</returns>
    public WeaponCard GetWeapon()
    {
        return weapon;
    }

    /// <summary>
    /// Gets the RoomCard contained.
    /// </summary>
    /// <returns>The <c>RoomCard</c>.</returns>
    public RoomCard GetRoom()
    {
        return room;
    }
}