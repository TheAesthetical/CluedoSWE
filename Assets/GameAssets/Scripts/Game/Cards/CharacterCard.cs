using UnityEngine;
/// <summary>
///  Class <c>CharacterCard</c> represents a Character card in the game. 
/// </summary>
public class CharacterCard : Card
{
    /// <summary>
    /// Instantiates a new instance of the <c>CharacterCard</c> class.
    /// </summary>
    /// <param name="CardNameIn"> The name of the character. </param>
    public CharacterCard(string CardNameIn, Sprite spriteIn) : base(CardNameIn, spriteIn)
    {
    }

}
