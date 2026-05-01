using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Menu Setup")]
    [SerializeField] private GameObject menu;

    [SerializeField] private RectTransform menuRectTransform;
    [SerializeField] private CanvasGroup menuCanvasGroup;

    private bool isOpen;

    //public static event Action OnOpenMenu;
    //public static event Action OnCloseMenu;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		if (menu.CompareTag("Menu1"))
		{
            OpenMenu();
		}
		if (!menu.CompareTag("Menu1"))
        {
            CloseMenu();
        }
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) && GameObject.FindWithTag("MenuCanvas1").GetComponent<MenuController>().menuCanvasGroup.alpha == 1)
		{
            NextPrevMenu("MenuCanvas1", true);
		}
	}

    /// <summary>
    /// Toggles whether the menu is open or closed
    /// </summary>
    [ContextMenu("Toggle Open Close")]
    public void ToggleOpenClose()
    {
        if (isOpen)
            CloseMenu();
        else
            OpenMenu();
    }

    /// <summary>
    /// Opens the menu
    /// </summary>
    [ContextMenu("Open Menu")]
    public void OpenMenu()
    {
        MenuState(true);
	}

    /// <summary>
    /// Closes the menu
    /// </summary>
    [ContextMenu("Close Menu")]
    public void CloseMenu()
    {
        MenuState(false);
	}

    /// <summary>
    /// Sets the menu into either open or closed
    /// </summary>
    /// <param name="state">True = open, False = closed</param>
    private void MenuState(bool state)
    {
		isOpen = state;
		menuCanvasGroup.alpha = state ? 1 : 0;
		menuCanvasGroup.interactable = state;
        menuCanvasGroup.blocksRaycasts = state;
	}

    /// <summary>
    /// Changes the menu to either the next or previous one
    /// </summary>
    /// <param name="tag">The tag of the current menu</param>
    /// <param name="direction">True = next menu, False = previous menu</param>
    public static void NextPrevMenu(string tag, bool direction)
    {
        GameObject currentMenu = GameObject.FindWithTag(tag);

		currentMenu.GetComponent<MenuController>().CloseMenu();

        int nextNum = tag[^1] - '0';
        if (direction)
            nextNum++;
        else
            nextNum--;
		GameObject.FindWithTag(tag[..(tag.Length - 1)] + nextNum).GetComponent<MenuController>().OpenMenu();
    }
}
