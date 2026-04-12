using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainMenuButtons : MonoBehaviour
{
	private Button button;

	private void Start()
	{
		button = GetComponent<Button>();
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
		GameObject menuUI = GameObject.FindWithTag("MenuUI");
		Sprite currentSprite = menuUI.GetComponent<Image>().sprite;
		Sprite newSprite = menuUI.GetComponent<MenuUI>().GetBackground(1);
		if (currentSprite != newSprite)
		{
			menuUI.GetComponent<Image>().sprite = newSprite;
		}

		MenuController[] controllers = GameObject.FindWithTag("MenuCanvas3").GetComponents<MenuController>();
		controllers[1].OpenMenu();
	}

	public void BackButtonClick()
	{
		MenuController.NextPrevMenu("MenuCanvas3", false);
	}
}
