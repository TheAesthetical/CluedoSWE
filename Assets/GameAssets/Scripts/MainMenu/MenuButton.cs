using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainMenuButtons : MonoBehaviour
{
	private Button button;
	private GameObject menuUI;

	private void Start()
	{
		button = GetComponent<Button>();
		menuUI = GameObject.FindWithTag("MenuUI");
	}

	public void StartButtonClick()
	{
		MenuController.NextPrevMenu("MenuCanvas2", true);
	}

	// will only work in a compiled version, not in the unity editor
	public void QuitButtonClick()
	{
		Debug.Log("Closing game");
		Application.Quit();
	}

	public void PlayerButtonClick(int players)
	{
		// opening player cards menu and changing the background to match
		menuUI.GetComponent<MenuUI>().SetBackground(1);
		menuUI.GetComponent<MenuUI>().LoadCards(players);
		MenuController[] controllers = GameObject.FindWithTag("MenuCanvas3").GetComponents<MenuController>();
		controllers[1].OpenMenu();
		MenuUI.ToggleElement(menuUI.transform.GetChild(3).transform.GetChild(0).transform.GetChild(7).gameObject, true);
	}

	public void BackButtonClick()
	{
		// change back to previous menu and change background
		MenuController.NextPrevMenu("MenuCanvas3", false);
		MenuController[] controllers = GameObject.FindWithTag("MenuCanvas3").GetComponents<MenuController>();
		controllers[1].CloseMenu();
		menuUI.GetComponent<MenuUI>().SetBackground(0);
	}

	public void ToggleButtonClick()
	{
		Sprite currentSprite = button.GetComponent<Image>().sprite;
		int currentToggleNum = currentSprite.name[7] - '0';

		if (currentToggleNum > 10)
		{
			currentToggleNum = 0;
		}
		else if (currentToggleNum == 6)
		{
			currentToggleNum = -1;
		}

		List<string> toggleList = new List<string> { "0" };
		for (int i = 1; i < menuUI.GetComponent<MenuUI>().getPlayerNum(); i++)
		{
			toggleList.Add(menuUI.transform.GetChild(3).transform.GetChild(1).transform.GetChild(i-1).transform.GetChild(0).GetComponent<Image>().sprite.name);
		}

		GameObject targetToggle = menuUI.transform.GetChild(3).transform.GetChild(1).transform.GetChild(currentToggleNum).transform.GetChild(0).gameObject;
		while (toggleList.Contains(targetToggle.GetComponent<Image>().sprite.name))
		{
			currentToggleNum++;
			
			if (currentToggleNum > menuUI.GetComponent<MenuUI>().getPlayerNum())
			{
				currentToggleNum = 0;
			}
		}
		
		button.GetComponent<Image>().sprite = menuUI.GetComponent<MenuUI>().GetToggleSprite(currentToggleNum + 1);
	}

	public void BeginButtonClick()
	{
		SceneManager.LoadScene(1);
	}
}
