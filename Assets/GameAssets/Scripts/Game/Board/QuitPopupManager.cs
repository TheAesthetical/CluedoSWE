using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach to: Quit Button  (Canvas > Quit Button)
///
/// No GameManager needed. Completely self-contained.
///
/// SETUP:
///   1. First, build the popup panel in your scene (see steps below).
///   2. Select "Quit Button" in the Hierarchy.
///   3. Add Component → QuitPopupManager.
///   4. Drag your popup panel into the Quit Popup Panel slot.
///   5. Drag the Yes and No buttons (inside the popup) into their slots.
///   6. Type your main menu scene name into Main Menu Scene Name.
///      Must match exactly what's in File > Build Settings.
///
/// HOW TO BUILD THE POPUP PANEL:
///   a. Right-click Canvas → UI → Panel. Rename it "Quit Popup Panel".
///   b. Give it a dark semi-transparent background colour.
///   c. Resize it to a small centred box (e.g. 400 x 200).
///   d. Inside it, add:
///        • UI > Text (TMP) — write "Do you want to leave the game?"
///        • UI > Button (TMP) — rename "Yes Button", set label text to "Yes"
///        • UI > Button (TMP) — rename "No Button", set label text to "No"
///   e. The panel will hide itself automatically on Play via CanvasGroup —
///      you do NOT need to set it inactive in the Inspector.
/// </summary>
public class QuitPopupManager : MonoBehaviour
{
    [Header("Popup Panel")]
    [Tooltip("The confirmation popup panel you created. Hides itself automatically on Play.")]
    public GameObject quitPopupPanel;

    [Tooltip("The Yes button inside the popup panel.")]
    public Button yesButton;

    [Tooltip("The No button inside the popup panel.")]
    public Button noButton;

    [Header("Main Menu")]
    [Tooltip("Exact scene name as listed in File > Build Settings.")]
    public string mainMenuSceneName = "MainMenu";

    [Header("Fade Animation")]
    public float fadeDuration = 0.18f;

    // ------------------------------------------------------------------
    private Button      _quitButton;
    private CanvasGroup _cg;

    void Awake()
    {
        // The Quit Button is the GameObject this script is attached to
        _quitButton = GetComponent<Button>();
        if (_quitButton == null)
            Debug.LogError("[QuitPopupManager] This script must be attached to the Quit Button.");

        // Set up CanvasGroup on the popup for fading
        _cg = quitPopupPanel.GetComponent<CanvasGroup>();
        if (_cg == null)
            _cg = quitPopupPanel.AddComponent<CanvasGroup>();

        // Hide popup at start
        _cg.alpha          = 0f;
        _cg.interactable   = false;
        _cg.blocksRaycasts = false;

        _quitButton.onClick.AddListener(ShowPopup);
        yesButton  .onClick.AddListener(OnYes);
        noButton   .onClick.AddListener(OnNo);
    }

    // ------------------------------------------------------------------
    void ShowPopup()
    {
        StopAllCoroutines();
        _cg.interactable   = true;
        _cg.blocksRaycasts = true;
        StartCoroutine(Fade(0f, 1f));
    }

    void OnNo()
    {
        StopAllCoroutines();
        StartCoroutine(FadeAndHide());
    }

    void OnYes()
    {
        PlayerPrefs.Save(); // ensure checkbox data is saved before leaving
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // ------------------------------------------------------------------
    IEnumerator FadeAndHide()
    {
        yield return Fade(1f, 0f);
        _cg.interactable   = false;
        _cg.blocksRaycasts = false;
    }

    IEnumerator Fade(float from, float to)
    {
        float t   = 0f;
        _cg.alpha = from;
        while (t < 1f)
        {
            t        += Time.deltaTime / fadeDuration;
            _cg.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(t));
            yield return null;
        }
        _cg.alpha = to;
    }
}
