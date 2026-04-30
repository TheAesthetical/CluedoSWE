using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>MenuUI</c> represents the highest level of the menu UI, containing attributes to be used across the main menu.
/// </summary>
[RequireComponent(typeof(Image))]
public class MenuUI : MonoBehaviour
{
	private int playerNum = 0;
    [SerializeField] Sprite[] backgroundSprites;
    [SerializeField] Sprite[] toggleSprites;
	[SerializeField] Sprite[] cardSprites;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		GameObject cardPanel = transform.GetChild(3).gameObject.transform.GetChild(1).gameObject;
		ResetCards(cardPanel);
		ResetToggles(cardPanel);
		ToggleElement(transform.GetChild(3).transform.GetChild(0).transform.GetChild(7).gameObject, false);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	public int getPlayerNum()
	{
		return playerNum;
	}

    /// <summary>
    /// Returns the background sprite corresponding to the int passed in.
    /// </summary>
    /// <param name="spriteNum">An int representing the background sprite to be returned.</param>
    /// <returns>The background sprite corresponding to the int passed in.</returns>
    public Sprite GetBackgroundSprite(int spriteNum)
    {
        return backgroundSprites[spriteNum];
    }

	public void SetBackground(int spriteNum)
	{
		Sprite currentBackgroundSprite = GetComponent<Image>().sprite;
		Sprite newBackgroundSprite = backgroundSprites[spriteNum];
		GetComponent<Image>().sprite = newBackgroundSprite;
	}

	/// <summary>
	/// Returns the toggle sprite corresponding to the int passed in.
	/// </summary>
	/// <param name="spriteNum">An int representing the toggle sprite to be returned.</param>
	/// <returns>The toggle sprite corresponding to the int passed in.</returns>
	public Sprite GetToggleSprite(int spriteNum)
	{
		return toggleSprites[spriteNum];
	}

    public void SetToggleSprite(int spriteNum)
    {
        GetComponent<Image>().sprite = toggleSprites[spriteNum];
    }

    public void LoadCards(int cardNum)
    {
		GameObject cardPanel = transform.GetChild(3).gameObject.transform.GetChild(1).gameObject;
		ResetCards(cardPanel);
		for (int i = 0; i < cardNum; i++)
		{
            GameObject cardCanvas = cardPanel.transform.GetChild(i).gameObject;
			ToggleElement(cardCanvas, true);
			cardCanvas.transform.GetChild(1).GetComponent<Image>().sprite = cardSprites[i];
		}
		playerNum = cardNum;
	}

	private void ResetCards(GameObject cardPanel)
	{
		for (int i = 0; i < 6; i++)
		{
			GameObject cardCanvas = cardPanel.transform.GetChild(i).gameObject;
			ToggleElement(cardCanvas, false);
			cardCanvas.transform.GetChild(1).GetComponent<Image>().sprite = null;
		}
		playerNum = 0;
	}

	public static void ToggleElement(GameObject element, bool state)
	{
		element.GetComponent<CanvasGroup>().alpha = state ? 1 : 0;
		element.GetComponent<CanvasGroup>().blocksRaycasts = state;
		element.GetComponent<CanvasGroup>().interactable = state;
	}

	private void ResetToggles(GameObject cardPanel)
	{
		for (int i = 0; i < 6; i++)
		{
			GameObject cardCanvas = cardPanel.transform.GetChild(i).gameObject;
			cardCanvas.transform.GetChild(0).GetComponent<Image>().sprite = toggleSprites[0];
		}
	}
}
