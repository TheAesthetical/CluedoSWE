using UnityEngine;
using Random = System.Random;

/// <summary>
/// Class <c>Dice</c> represents a dice used in the game. 
/// </summary>
public class Dice
{   
    private int sides;

    private Random random;
    

    /// <summary>
    /// Creates a new instance of the <c>Dice</c> class. 
    /// </summary>
    /// <param name="sidesInput">The number of sides the dice should have.</param>
    public Dice(int sidesInput = 6) // 6 unless otherwise specified
    {
        sides = sidesInput;
        random = new Random();
    }

    /// <summary>
    /// Simulates a roll of the dice. 
    /// </summary>
    /// <returns> Returns a random integer from 1 up to the number of sides. </returns>
    public int Roll()
    {
        return random.Next(1, sides+1); //sides+1 as upper bound is not inclusive
    }

}