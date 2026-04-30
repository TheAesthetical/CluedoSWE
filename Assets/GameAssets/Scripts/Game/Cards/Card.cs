using UnityEngine;
/// <summary>
/// Class <c> Card </c> is an abstract class that represents a generic card. It provides the base for each card type. 
/// </summary>
public abstract class Card : MonoBehaviour
{
    public string CardName {get; private set;}

    [SerializeField] private Sprite sprite;

    /// <summary>
    /// Initialises a new instance of the Card class. Set to protected as this should not be instantiated directly!
    /// </summary>
    /// <param name="CardNameIn"> The name of the card </param>
    protected Card(string CardNameIn, Sprite spriteIn)
    {
        CardName = CardNameIn;
        sprite = spriteIn;
    }

    /// <summary>
    /// Returns the name of the card.
    /// </summary>
    /// <returns>Returns a string representing the card name. </returns>
    public override string ToString()
    {
        return CardName;
    }
    /// <summary>
    /// Returns the sprite for this card.
    /// </summary>
    /// <returns>A sprite. </returns>
    public Sprite GetSprite()
    {
        return sprite;
    }

}
