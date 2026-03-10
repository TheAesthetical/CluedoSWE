using UnityEngine;
using UnityEngine.UIElements;

public class MenuTrigger : MonoBehaviour
{

	private void Awake()
	{
		
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Return))
		{
			UIDocument document = GetComponent<UIDocument>();
			VisualElement root = document.rootVisualElement;
			VisualElement oldPage = root.Q("MenuPage1");
			VisualElement newPage = root.Q("MenuPage2");

			MenuUI.PageChange(oldPage, newPage);
		}
	}
}
