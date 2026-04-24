using UnityEngine;
/// <summary>
/// Class <c>MurderEnvelope</c> represents the hidden solution to the game.
/// Contains one <c>CharacterCard</c>, <c>WeaponCard</c>, and <c>RoomCard</c>
/// </summary>
public class MurderEnvelope
{
    private CharacterCard characterCard;
    private WeaponCard weaponCard;
    private RoomCard roomCard;

    /// <summary>
    /// Sets the cards that form the solution.
    /// </summary>
    /// <param name="character"> The <c>CharacterCard</c> in the MurderEnvelope. </param>
    /// <param name="weapon"> The <c>WeaponCard</c> in the MurderEnvelope.</param>
    /// <param name="room"> The <c>RoomCard</c> in the MurderEnvelope.</param>
    public void SetCards(CharacterCard character, WeaponCard weapon, RoomCard room)
    {
        this.characterCard = character;
        this.weaponCard = weapon;
        this.roomCard = room;
    }

    /// <summary>
    /// Checks whether the player's accusation matches the solution contained in the MurderEnvelope.
    /// </summary>
    /// <param name="character"> The character in the accusation. </param>
    /// <param name="weapon"> The weapon in the accusation. </param>
    /// <param name="room"> The room in the accusation. </param>
    /// <returns> True if the accusation is correct, false otherwise. </returns>
    public bool CheckAccusation(CharacterCard character, WeaponCard weapon, RoomCard room)
    {
        return (
            character.ToString() == this.characterCard.ToString() && // using the names of the cards
            weapon.ToString() == this.weaponCard.ToString() &&
            room.ToString() == this.roomCard.ToString()
            );
    }

}
