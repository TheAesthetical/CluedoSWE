using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
///  Controls the end game screen
/// </summary>
public class EndScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerText;


    /// <summary>
    /// Unity start method.
    /// </summary>
    void Start()
    {
        // Get winner Player class passed from game
        Player winner = GameController.GetWinner();

        if (winner != null && winnerText != null)
        {
            winnerText.text = winner.GetCharacter().ToString() + "Wins!";
        }
    }
    
    /// <summary>
    /// Returns to the Main Menu scene.
    /// </summary>
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}