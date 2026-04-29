using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
///  Controls running the two dice, and handles the animation.
/// </summary>
public class DiceController : MonoBehaviour
{
    private Dice dice;

    [SerializeField] private Image diceImage1;
    [SerializeField] private Image diceImage2;
    [SerializeField] private Sprite[] diceSprites;

    /// <summary>
    /// Unity Start() method.
    /// </summary>
    void Start()
    {
        dice = new Dice();
    
    }


    /// <summary>
    /// Called when Roll button is pressed.
    /// </summary>
    public void RollDice()
    {   
        StartCoroutine(RollAnimation());
    }
    
    /// <summary>
    /// Animates both dice and shows the final results.
    /// </summary>
    private IEnumerator RollAnimation()
    {
        int dice1Result = dice.Roll();
        int dice2Result = dice.Roll();

        // Animation Loop
        for (int i = 0; i < 10; i++)
        {
            int rand1 = UnityEngine.Random.Range(0, 6);
            int rand2 = UnityEngine.Random.Range(0, 6);

            diceImage1.sprite = diceSprites[rand1];
            diceImage2.sprite = diceSprites[rand2];

            yield return new WaitForSeconds(0.05f + i * 0.02f);
        }


        // Final Result
        diceImage1.sprite = diceSprites[dice1Result-1];
        diceImage2.sprite = diceSprites[dice2Result-1];


        int total =  dice1Result + dice2Result;

        // Output
        Debug.Log("Rolled Dice 1 Result: " + dice1Result);
        Debug.Log("Rolled Dice 2 Result: " + dice2Result);
        Debug.Log("Total Result: " + total);
    }
}