using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles a single row in the deticive sheet UI
/// Each Row represents one card (suspect, weapon or room)
/// and allows buttons toggle cross show that its been seen
/// </summary>
public class DetectiveRowUI : MonoBehaviour
{

    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private Button[] crossButtons;
    [SerializeField] private GameObject[] xImages;
    private string cardName;

    /// <summary>
    /// Initialise the row with a card name and 
    /// sets up buttons. Also set all crosses to hidden
    /// </summary>
    /// <param name="cardName">The name of the card ("Knife")</param>
    public void Setup(string cardName)
    {
        if (crossButtons.Length != xImages.Length)
        {
            Debug.LogError("Buttons and XImages length mismatch!");
        }
        this.cardName = cardName;
        cardNameText.text = cardName;

        for (int i = 0; i < crossButtons.Length; i++)
        {
            int buttonIndex = i;

            //Stops duplicate listners
            crossButtons[buttonIndex].onClick.RemoveAllListeners();

            //Add click behaviour 
            crossButtons[buttonIndex].onClick.AddListener(() => ToggleCross(buttonIndex));

            //Hide all crosses at the start
            xImages[buttonIndex].SetActive(false);
        }
    }

/// <summary>
/// Toggle the visisbility of the cross for a given player coloum
/// </summary>
/// <param name="index">The index of player column</param>
private void ToggleCross(int index)
{
    Debug.Log(cardName + " button " + index + " clicked");
    xImages[index].SetActive(!xImages[index].activeSelf);
}
}