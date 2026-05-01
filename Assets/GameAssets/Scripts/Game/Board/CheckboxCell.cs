using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Attach to every cell Button inside the detective sheet grid.
/// Click once  → shows X
/// Click again → clears X
/// State is saved to PlayerPrefs so it persists across sessions.
///
/// SETUP:
///   Each cell Button GameObject must have a UNIQUE name, e.g:
///   "Cell_Scarlett_Knife", "Cell_Plum_Revolver", etc.
///   Each cell needs a child TextMeshProUGUI for the X display.
/// </summary>
public class CheckboxCell : MonoBehaviour
{
    [Header("X Visual Settings")]
    public string xCharacter = "X";
    public Color  xColor     = Color.red;
    public float  xFontSize  = 20f;

    [Header("References (auto-found if blank)")]
    public TextMeshProUGUI xLabel;

    private string SaveKey   => "Cluedo_Cell_" + gameObject.name;
    private bool   _isChecked;
    private Button _btn;

    void Awake()
    {
        _btn = GetComponent<Button>();
        if (_btn == null)
            Debug.LogError($"[CheckboxCell] '{name}' needs a Button component.");

        if (xLabel == null)
            xLabel = GetComponentInChildren<TextMeshProUGUI>(true);

        if (xLabel != null)
        {
            xLabel.text      = "";
            xLabel.fontSize  = xFontSize;
            xLabel.color     = xColor;
            xLabel.alignment = TextAlignmentOptions.Center;
        }

        _btn?.onClick.AddListener(Toggle);
    }

    void OnEnable() => Refresh();

    void Toggle()
    {
        _isChecked = !_isChecked;
        Apply();
        PlayerPrefs.SetInt(SaveKey, _isChecked ? 1 : 0);
        PlayerPrefs.Save();
    }

    void Refresh()
    {
        _isChecked = PlayerPrefs.GetInt(SaveKey, 0) == 1;
        Apply();
    }

    void Apply()
    {
        if (xLabel != null)
            xLabel.text = _isChecked ? xCharacter : "";
    }

    public void ResetCell()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        _isChecked = false;
        Apply();
    }
}
