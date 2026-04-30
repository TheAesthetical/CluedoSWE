using UnityEngine;
/// <summary>
/// Class <c>WeaponCard</c> represents a Weapon Card in the game.
/// </summary>
public class WeaponCard : Card
{
    /// <summary>
    /// Instantiates a new instance of the <c>WeaponCard</c> class. 
    /// </summary>
    /// <param name="CardNameIn"> The name of the weapon. </param>
    public WeaponCard(string CardNameIn, Sprite spriteIn) : base(CardNameIn, spriteIn)
    {
    }

}