using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Attach to: Roll Button  (Canvas > Background > Options Panel > Roll Button)
///
/// No GameManager needed. Completely self-contained.
///
/// Rolls 2 dice (each 1-6) giving a total of 2-12 moves.
/// Shows a tumbling animation before settling on the final result.
/// The Roll button is disabled after rolling and re-enabled when
/// PlayerTurnManager advances to the next turn.
///
/// SETUP:
///   1. Select "Roll Button" in the Hierarchy.
///   2. Add Component → DiceRollManager.
///   3. Create a small "Dice Panel" somewhere on the Canvas to display results:
///        • Two Image GameObjects for the die faces (Die 1, Die 2)
///            - Optionally assign 6 die-face sprites (index 0 = face 1, index 5 = face 6)
///            - Or leave dieSprites empty: each Image needs a child TMP Text
///              and numbers will be shown as text instead
///        • One TextMeshProUGUI showing the total, e.g. "Moves: 9"
///   4. Drag those objects into the slots below.
///   5. Drag "End Turn Button" into the End Turn Button slot so this script
///      knows when to re-enable rolling for the next turn.
///
/// NOTE: End Turn button click is shared between this script and
/// PlayerTurnManager — both listen to the same button independently,
/// so there is no conflict.
/// </summary>
public class DiceRollManager : MonoBehaviour
{
    [Header("Dice Display")]
    [Tooltip("Image showing the first die face.")]
    public Image die1Image;

    [Tooltip("Image showing the second die face.")]
    public Image die2Image;

    [Tooltip("TMP text that shows the total, e.g. 'Moves: 7'")]
    public TextMeshProUGUI totalLabel;

    [Header("Die Face Sprites (optional)")]
    [Tooltip("6 sprites: index 0 = face 1, index 5 = face 6. Leave empty to use number text.")]
    public Sprite[] dieSprites = new Sprite[6];

    [Header("End Turn Button")]
    [Tooltip("Drag in: Canvas > Background > Options Panel > End Turn Button")]
    public Button endTurnButton;

    [Header("Roll Animation")]
    [Tooltip("How many frames the dice tumble before settling.")]
    public int   tumbleFrames   = 12;
    [Tooltip("Seconds between each tumble frame.")]
    public float tumbleInterval = 0.055f;

    [Header("Rules")]
    [Tooltip("Player can only roll once per turn. Roll button greys out after rolling.")]
    public bool oneRollPerTurn = true;

    // ------------------------------------------------------------------
    /// <summary>The total of both dice after the last roll. 0 if not yet rolled.</summary>
    public int TotalMoves { get; private set; }
    public int Die1Value  { get; private set; }
    public int Die2Value  { get; private set; }

    private Button _rollButton;
    private bool   _hasRolledThisTurn = false;
    private bool   _rolling           = false;

    // ------------------------------------------------------------------
    void Awake()
    {
        _rollButton = GetComponent<Button>();
        if (_rollButton == null)
            Debug.LogError("[DiceRollManager] This script must be attached to the Roll Button.");

        _rollButton.onClick.AddListener(AttemptRoll);

        if (endTurnButton != null)
            endTurnButton.onClick.AddListener(ResetForNewTurn);
        else
            Debug.LogWarning("[DiceRollManager] endTurnButton is not assigned — rolling won't auto-reset each turn.");

        ClearDisplay();
    }

    // ------------------------------------------------------------------
    void AttemptRoll()
    {
        if (_rolling)                             return;
        if (oneRollPerTurn && _hasRolledThisTurn) return;

        StartCoroutine(RollRoutine());
    }

    void ResetForNewTurn()
    {
        _hasRolledThisTurn      = false;
        TotalMoves              = 0;
        _rollButton.interactable = true;
        ClearDisplay();
    }

    // ------------------------------------------------------------------
    IEnumerator RollRoutine()
    {
        _rolling                 = true;
        _rollButton.interactable = false;

        // Tumble animation
        for (int i = 0; i < tumbleFrames; i++)
        {
            ShowDie(die1Image, Random.Range(1, 7));
            ShowDie(die2Image, Random.Range(1, 7));
            if (totalLabel != null) totalLabel.text = "Rolling...";
            yield return new WaitForSeconds(tumbleInterval);
        }

        // Final values
        Die1Value  = Random.Range(1, 7);
        Die2Value  = Random.Range(1, 7);
        TotalMoves = Die1Value + Die2Value;

        ShowDie(die1Image, Die1Value);
        ShowDie(die2Image, Die2Value);
        if (totalLabel != null)
            totalLabel.text = $"Moves: {TotalMoves}";

        _hasRolledThisTurn       = true;
        _rolling                 = false;
        _rollButton.interactable = false; // greyed out for rest of this turn

        Debug.Log($"[Dice] {Die1Value} + {Die2Value} = {TotalMoves} moves");
    }

    // ------------------------------------------------------------------
    void ShowDie(Image img, int face)
    {
        if (img == null) return;

        bool hasSprites = dieSprites != null
                          && dieSprites.Length == 6
                          && dieSprites[face - 1] != null;

        if (hasSprites)
        {
            img.sprite  = dieSprites[face - 1];
            img.enabled = true;
            var tmp = img.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = "";
        }
        else
        {
            // Fallback: show number as text in a child TMP
            img.enabled = false;
            var tmp = img.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = face.ToString();
        }
    }

    void ClearDisplay()
    {
        if (die1Image != null) die1Image.enabled = false;
        if (die2Image != null) die2Image.enabled = false;

        var t1 = die1Image?.GetComponentInChildren<TextMeshProUGUI>();
        var t2 = die2Image?.GetComponentInChildren<TextMeshProUGUI>();
        if (t1 != null) t1.text = "";
        if (t2 != null) t2.text = "";
        if (totalLabel != null) totalLabel.text = "";
    }
}
