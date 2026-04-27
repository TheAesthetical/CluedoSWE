using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Builds the detective sheet UI by creating a row object
/// for the cards
/// Also adds section gaps between each group
/// </summary>
public class DetectiveSheetBuilder : MonoBehaviour
{
    [SerializeField] private DetectiveRowUI rowPrefab;
    [SerializeField] private Transform rowsContainer;

    /// <summary>
    /// Clears existing rows and builds the full detective
    /// shett when the scene starts
    /// </summary>
    private void Start()
    {
        ClearRows();

        BuildRows(new string[]
        {
            "Miss Scarlett",
            "Colonel Mustard",
            "Mrs White",
            "Reverend Green",
            "Mrs Peacock",
            "Professor Plum"
        });

        AddGap(49);

        BuildRows(new string[]
        {
            "Candlestick",
            "Dagger",
            "Lead Pipe",
            "Revolver",
            "Rope",
            "Wrench"
        });

        AddGap(49);

        BuildRows(new string[]
        {
            "Kitchen",
            "Ballroom",
            "Conservatory",
            "Dining Room",
            "Billiard Room",
            "Library",
            "Lounge",
            "Hall",
            "Study"
        });
    }

    /// <summary>
    /// Creates a detective sheet row for each card name
    /// </summary>
    /// <param name="cardNames">Array of card names to display as row</param>
    private void BuildRows(string[] cardNames)
    {
        foreach (string cardName in cardNames)
        {
            DetectiveRowUI row = Instantiate(rowPrefab, rowsContainer);
            row.Setup(cardName);
        }
    }

    /// <summary>
    /// Adds vertical spacing between detecive sheet sections
    /// Where the headers can be seen
    /// </summary>
    /// <param name="height">The height of the gap</param>
    private void AddGap(float height)
    {
      GameObject gap = new GameObject("SectionGap", typeof(RectTransform));

        gap.transform.SetParent(rowsContainer, false);

        LayoutElement layout = gap.AddComponent<LayoutElement>();
        layout.minHeight = height;
        layout.preferredHeight = height;
        layout.flexibleHeight = 0;
    }

    /// <summary>
    /// Removes any existing generated rows from the rows
    /// container
    /// </summary>
    private void ClearRows()
    {
        foreach (Transform child in rowsContainer)
        {
            Destroy(child.gameObject);
        }
    }
}