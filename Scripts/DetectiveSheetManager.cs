using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this script directly to: Detective Sheet Panel
/// SETUP:
///   1. Select "Detective Sheet Panel" in the Hierarchy.
///   2. Inspector → Add Component → DetectiveSheetManager.
///   3. Leave the sheetPanel field empty — it auto-assigns to this GameObject.
///   4. Drag "Detective Button" (inside Options Panel) into the detectiveButton slot.
///   5. The panel will be hidden automatically on Play — no need to set it inactive.
/// </summary>
public class DetectiveSheetManager : MonoBehaviour
{
    [Header("Button Reference")]
    [Tooltip("Drag in: Canvas > Background > Options Panel > Detective Button")]
    public Button detectiveButton;

    [Header("Animation")]
    [Tooltip("Seconds for the open/close animation.")]
    public float animDuration = 0.22f;

    // ------------------------------------------------------------------
    private RectTransform _rt;
    private CanvasGroup   _cg;
    private bool          _open = false;
    private Coroutine     _anim;

    static readonly Vector3 ScaleOpen   = Vector3.one;
    static readonly Vector3 ScaleClosed = new Vector3(0.02f, 0.02f, 1f);

    // ------------------------------------------------------------------
    void Awake()
    {
        _rt = GetComponent<RectTransform>();

        // CanvasGroup controls visibility without disabling this script
        _cg = GetComponent<CanvasGroup>();
        if (_cg == null) _cg = gameObject.AddComponent<CanvasGroup>();

        // Start hidden
        _rt.localScale     = ScaleClosed;
        _cg.alpha          = 0f;
        _cg.interactable   = false;
        _cg.blocksRaycasts = false;

        if (detectiveButton != null)
            detectiveButton.onClick.AddListener(Toggle);
        else
            Debug.LogWarning("[DetectiveSheetManager] detectiveButton is not assigned.");
    }

    // ------------------------------------------------------------------
    /// <summary>Toggle the sheet open or closed. Called by the Detective Sheet button.</summary>
    public void Toggle()
    {
        if (_anim != null) StopCoroutine(_anim);
        _open = !_open;
        _anim = StartCoroutine(_open ? AnimateOpen() : AnimateClose());
    }

    /// <summary>Close immediately without animation — e.g. when End Turn is pressed.</summary>
    public void ForceClose()
    {
        if (_anim != null) StopCoroutine(_anim);
        _open              = false;
        _rt.localScale     = ScaleClosed;
        _cg.alpha          = 0f;
        _cg.interactable   = false;
        _cg.blocksRaycasts = false;
    }

    public bool IsOpen => _open;

    // ------------------------------------------------------------------
    IEnumerator AnimateOpen()
    {
        _cg.alpha          = 1f;
        _cg.interactable   = true;
        _cg.blocksRaycasts = true;
        yield return Animate(ScaleClosed, ScaleOpen);
    }

    IEnumerator AnimateClose()
    {
        yield return Animate(ScaleOpen, ScaleClosed);
        _cg.alpha          = 0f;
        _cg.interactable   = false;
        _cg.blocksRaycasts = false;
    }

    IEnumerator Animate(Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / animDuration;
            _rt.localScale = Vector3.LerpUnclamped(from, to, EaseOutBack(Mathf.Clamp01(t)));
            yield return null;
        }
        _rt.localScale = to;
    }

    // Slight overshoot gives a satisfying pop-in
    static float EaseOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);
    }

    // ------------------------------------------------------------------
    /// <summary>Wipe all checkbox data in this sheet. Call on New Game.</summary>
    public void ResetAllCells()
    {
        var cells = GetComponentsInChildren<CheckboxCell>(includeInactive: true);
        foreach (var c in cells) c.ResetCell();
    }
}
