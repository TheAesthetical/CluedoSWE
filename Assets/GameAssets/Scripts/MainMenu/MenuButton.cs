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
	/// <summary>
	/// Cycles the assignment of this toggle button between AI and valid players. 
	/// Ensures that:
	/// - No duplicate human players are assigned
	/// - Player indices remain sequential
	/// - Ai can be selected multiple times. 
	/// </summary>
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

	/// <summary>
	/// Updates the sprite displayed on the toggle button based on the current assignment.
	/// AI is represented by index -1, whilst human players use indices starting from 0.
	/// </summary>
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

	/// <summary>
	/// Checks whether a given player index is already assigned to another toggle button. 
	/// AI (-1) is excluded from this check and can be assigned multiple times.
	/// </summary>
	/// <param name="playerIndex"> The player index to check. </param>
	/// <returns>True if the player is already assigned, false otherwise. </returns>
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

	/// <summary>
	/// Determines whether assigning a given player index would maintain a valid selection state.
	/// 
	/// A valid state requires that all selected human players form a continuous sequence.
	/// Starting from player 1 (index 0) with no gaps. 
	/// 
	/// AI (-1) is ignored in this validation.
	/// </summary>
	/// <param name="proposedIndex">The player index being tested </param>
	/// <returns>True if the resulting selection would be valid, false otherwise. </returns>
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
