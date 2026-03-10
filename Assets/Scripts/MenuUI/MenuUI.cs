using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuUI : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/MenuUI")]
    public static void ShowExample()
    {
        MenuUI wnd = GetWindow<MenuUI>();
        wnd.titleContent = new GUIContent("MenuUI");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }

	void Update()
	{
        Debug.Log("AAAAAAAAAAAAA");
		if (Input.GetKeyDown(KeyCode.A))
		{
			//ActivateMenu1();
		}
	}

	public static void PageChange(VisualElement oldPage, VisualElement newPage)
    {
		oldPage.AddToClassList("PageUp");
		newPage.RemoveFromClassList("PageDown");
	}
}
