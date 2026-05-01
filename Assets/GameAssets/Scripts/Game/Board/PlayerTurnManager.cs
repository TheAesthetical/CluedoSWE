using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach to: Character Panel
///
/// No GameManager needed. Completely self-contained.
///
/// Highlights the active player's portrait with a bright yellow outline.
/// Uses Unity's built-in Outline component — added automatically to each
/// character GameObject so you don't need to add anything manually.
///
/// SETUP:
///   1. Select "Character Panel" in the Hierarchy.
///   2. Add Component → PlayerTurnManager.
///   3. Set the Size of the Characters array to however many players are
///      in your game (up to 6), then drag each character GameObject in:
///        [0] MissScarlett
///        [1] ReverendGreen
///        [2] ColonelMustard
///        [3] MrsWhite
///        [4] MrsPeacock
///        [5] ProfessorPlum
///   4. Drag "End Turn Button" (inside Options Panel) into the End Turn Button slot.
///      The button stays where it is — this is just a reference so this script
///      can listen for clicks.
///   5. Set Player Count to how many players are in this game (1-6).
///      Only that many portraits will be shown; the rest are hidden.
///
/// TO CHANGE ACTIVE PLAYER FROM ANOTHER SCRIPT:
///   Find the component and call SetActivePlayer(index) or NextTurn():
///   FindObjectOfType&lt;PlayerTurnManager&gt;().NextTurn();
/// </summary>
public class PlayerTurnManager : MonoBehaviour
{
    [Header("Character GameObjects (drag from Hierarchy in turn order)")]
    public GameObject[] characters = new GameObject[6];
    // Suggested order matching your hierarchy:
    // [0] MissScarlett
    // [1] ReverendGreen
    // [2] ColonelMustard
    // [3] MrsWhite
    // [4] MrsPeacock
    // [5] ProfessorPlum

    [Header("End Turn Button")]
    [Tooltip("Drag in: Canvas > Background > Options Panel > End Turn Button")]
    public Button endTurnButton;

    [Header("Player Count")]
    [Tooltip("How many players are in this game. Only this many portraits are shown.")]
    [Range(1, 6)]
    public int playerCount = 6;

    [Header("Outline Settings")]
    public Color  activeColour    = new Color(1.00f, 0.92f, 0.00f, 1.00f); // bright yellow
    public Color  inactiveColour  = new Color(0.00f, 0.00f, 0.00f, 0.00f); // transparent
    public Vector2 outlineDistance = new Vector2(4f, -4f);

    // ------------------------------------------------------------------
    private Outline[] _outlines;
    private int       _activeIndex = 0;

    void Awake()
    {
        _outlines = new Outline[characters.Length];

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null) continue;

            // Show only the slots being used
            bool inUse = i < playerCount;
            characters[i].SetActive(inUse);

            if (!inUse) continue;

            // Add Outline component automatically if not already present
            _outlines[i] = characters[i].GetComponent<Outline>();
            if (_outlines[i] == null)
                _outlines[i] = characters[i].AddComponent<Outline>();

            _outlines[i].effectColor    = inactiveColour;
            _outlines[i].effectDistance = outlineDistance;
        }

        // Highlight first player
        SetActivePlayer(0);

        // Wire up End Turn button
        if (endTurnButton != null)
            endTurnButton.onClick.AddListener(NextTurn);
        else
            Debug.LogWarning("[PlayerTurnManager] endTurnButton is not assigned.");
    }

    // ------------------------------------------------------------------
    /// <summary>Highlight the player at this index with a yellow outline.</summary>
    public void SetActivePlayer(int index)
    {
        _activeIndex = Mathf.Clamp(index, 0, playerCount - 1);

        for (int i = 0; i < playerCount; i++)
        {
            if (_outlines[i] == null) continue;
            _outlines[i].effectColor = (i == _activeIndex) ? activeColour : inactiveColour;
        }

        Debug.Log($"[PlayerTurnManager] Active player: {characters[_activeIndex]?.name}");
    }

    /// <summary>Advance to the next player. Called automatically by End Turn button.</summary>
    public void NextTurn()
    {
        SetActivePlayer((_activeIndex + 1) % playerCount);
    }

    public int    ActiveIndex         => _activeIndex;
    public string ActiveCharacterName => characters[_activeIndex]?.name ?? "Unknown";
}
