using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuButton : MonoBehaviour
{
	private Button button;
	private GameObject menuUI;

	// -1 represents AI
	private int assignedPlayerIndex = -1;

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
		int playerCount = menuUI.GetComponent<MenuUI>().getPlayerNum();
		int attempts = 0;

		do
		{
			assignedPlayerIndex++;

			if (assignedPlayerIndex >= playerCount)
			{
				assignedPlayerIndex = -1; // CPU
			}

			attempts++;

		} while (
			attempts <= playerCount + 1 &&
			(
				IsPlayerAlreadyAssigned(assignedPlayerIndex) ||
				!WouldPlayerSelectionBeValid(assignedPlayerIndex)
			)
		);

		UpdateToggleSprite();
	}

	private void UpdateToggleSprite()
	{
		// update sprite

		if (assignedPlayerIndex == -1)
		{
			button.GetComponent<Image>().sprite = menuUI.GetComponent<MenuUI>().GetToggleSprite(0);
		} else
		{
			button.GetComponent<Image>().sprite = menuUI.GetComponent<MenuUI>().GetToggleSprite(assignedPlayerIndex+1);
		}
	}

	private bool IsPlayerAlreadyAssigned(int playerIndex)
	{
		if (playerIndex == -1)
		{
			return false; // AI can be reused
		}

		MenuButton[] toggles = FindObjectsOfType<MenuButton>();

		foreach (MenuButton toggle in toggles)
		{
			if (toggle != this && toggle.assignedPlayerIndex == playerIndex)
			{
				return true;
			}
		}

		return false;
	}

	private bool WouldPlayerSelectionBeValid(int proposedIndex)
	{
		int playerCount = menuUI.GetComponent<MenuUI>().getPlayerNum();
		bool[] selectedPlayers = new bool[playerCount];

		MenuButton[] toggles = FindObjectsOfType<MenuButton>();

		foreach (MenuButton toggle in toggles)
		{
			int index = toggle == this ? proposedIndex : toggle.GetAssignedPlayerIndex();

			// Ignore CPU
			if (index == -1)
			{
				continue;
			}

			selectedPlayers[index] = true;
		}

		// Once we find a missing player, no later player can be selected
		bool foundGap = false;

		for (int i = 0; i < selectedPlayers.Length; i++)
		{
			if (!selectedPlayers[i])
			{
				foundGap = true;
			}
			else if (foundGap)
			{
				return false;
			}
		}

		return true;
	}

	public int GetAssignedPlayerIndex()
	{
		return assignedPlayerIndex;
	}
	public void BeginButtonClick()
	{
		SceneManager.LoadScene(1);
	}
}
