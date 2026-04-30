using UnityEngine;
/// <summary>
/// Class <c>RoomCard</c> represents a Room Card in the game.
/// </summary>
public class RoomCard : Card
{
    /// <summary>
    /// Instantiates a new instance of the <c> RoomCard</c> class. 
    /// </summary>
    /// <param name="CardNameIn"> The name of the room. </param>
    public RoomCard(string CardNameIn, Sprite spriteIn) : base(CardNameIn, spriteIn)
    {
    }

}