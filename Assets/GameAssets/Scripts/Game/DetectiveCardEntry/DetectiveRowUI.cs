using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles a single row in the deticive sheet UI
/// Each Row represents one card (suspect, weapon or room) per player column
/// Clicking button toggles the manual cross out
/// </summary>
public class DetectiveRowUI : MonoBehaviour
{

    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private Button[] crossButtons;
    [SerializeField] private GameObject[] xImages;
    private DetectiveSheet sheet;
    private DetectiveCardEntry entry;

    /// <summary>
    /// Initialise this row for a specific card in a specific sheet
    /// </summary>
    /// <param name="sheet">The DetectiveSheet this row reads from / writes to</param>
    /// <param name="cardName">Name of the card this row represents</param>
    public void Setup(DetectiveSheet sheet, string cardName)
    {
        if (crossButtons.Length != xImages.Length)
        {
            Debug.LogError("DetectiveRowUI: crossButtons and xImages length mismatch on " + cardName);
            return;
        }
        this.sheet = sheet;
        this.entry = sheet.GetEntryByName(cardName);

        if (entry == null)
        {
            Debug.LogError("DetectiveRowUI: no entry found for card " + cardName);
            return;
        }

        cardNameText.text = cardName;

        //Hook up each button to toggle the manual cross for its column
        //Inactive columns get disabled so the user can't click them
        int columnsToShow = Mathf.Min(crossButtons.Length, entry.GetColumnCount());
        for (int i = 0; i < columnsToShow; i++)
        {
            int columnIndex = i; // capture for closure
            Button button = crossButtons[columnIndex];

            button.onClick.RemoveAllListeners();

            if (entry.IsColumnActive(columnIndex))
            {
                button.interactable = true;
                button.onClick.AddListener(() => OnButtonClicked(columnIndex));
            }
            else
            {
                button.interactable = false;
            }
        }
        Refresh();
    }

    public void Refresh()
    {
        if (entry == null) return;
        int coloumToShow = Mathf.Min(xImages.Length, entry.GetColumnCount());
        for (int i = 0; i < coloumToShow; i++)
        {
            xImages[i].SetActive(entry.ShouldDisplayCross(i));
        }
    }

    /// <summary>
    /// The card name this row represents
    /// </summary>
    public string GetCardName()
    {
        return entry != null ? entry.GetCardName() : null;
    }

    private void OnButtonClicked(int columnIndex)
    {
        if (sheet == null || entry == null) return;

        sheet.ToggleManualCrossOut(entry, columnIndex);
        Refresh();
    }

}