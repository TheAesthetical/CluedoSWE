using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Builds the detective sheet UI by creating a row object
/// for the cards
/// Also adds section gaps between each group
/// </summary>
public class DetectiveSheetBuilder : MonoBehaviour
{
    [Header("Prefabs / References")]
    [SerializeField] private DetectiveRowUI rowPrefab;
    [SerializeField] private Transform rowsContainer;

    [Header("Game Setup")]
    [Tooltip("Number of real players this game (3 to 6). Used until a sheet is supplied externally.")]
    [SerializeField] private int activePlayerCount = 3;

    [Header("Layout")]
    [Tooltip("Vertical gap between sections (suspects / weapons / rooms).")]
    [SerializeField] private float sectionGapHeight = 49f;

    private DetectiveSheet sheet;
    private List<DetectiveRowUI> rows = new List<DetectiveRowUI>();

    /// <summary>
    /// The shared DetectiveSheet driving this UI
    /// </summary>
    public DetectiveSheet Sheet { get { return sheet; } }

    /// <summary>
    /// Clears existing rows and builds the full detective
    /// shett when the scene starts
    /// </summary>
    private void Start()
    {
        // If nothing has supplied a sheet yet, create a default one so the UI
        // works standalone for testing
        if (sheet == null)
        {
            sheet = new DetectiveSheet(activePlayerCount);
        }

        Build();
    }

    /// <summary>
    /// Supply an externally created sheet (e.g. from GameController) before Start runs
    /// if you call this after Start, also call Build() yourself to rebuild
    /// </summary>
    public void SetSheet(DetectiveSheet externalSheet)
    {
        sheet = externalSheet;
    }

    /// <summary>
    /// Tells every row to redraw from the data
    /// Call when the sheets contents cahnge from the outside the UI e.g auto crossed
    /// </summary>
    public void RefreshAll()
    {
        foreach (DetectiveRowUI row in rows)
        {
            if (row != null) row.Refresh();
        }
    }

    public void Build()
    {
        ClearRows();

        BuildSection(new string[]
        {
            "Miss Scarlett",
            "Colonel Mustard",
            "Mrs White",
            "Reverend Green",
            "Mrs Peacock",
            "Professor Plum"
        });

        AddGap(sectionGapHeight);

        BuildSection(new string[]
        {
            "Candlestick",
            "Knife",
            "Lead Pipe",
            "Revolver",
            "Rope",
            "Wrench"
        });

        AddGap(sectionGapHeight);

        BuildSection(new string[]
        {
            "Kitchen",
            "Ballroom",
            "Conservatory",
            "Dining Room",
            "Billiards Room",
            "Library",
            "Lounge",
            "Hall",
            "Study"
        });
    }

    /// <summary>
    /// Creates a row for each card name and registers it
    /// </summary>
    private void BuildSection(string[] cardNames)
    {
        foreach (string cardName in cardNames)
        {
            DetectiveRowUI row = Instantiate(rowPrefab, rowsContainer);
            row.Setup(sheet, cardName);
            rows.Add(row);
        }
    }

    /// <summary>
    /// Adds vertical spacing between detecive sheet sections
    /// Where the headers sit
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