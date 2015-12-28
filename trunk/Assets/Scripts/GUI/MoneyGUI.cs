using UnityEngine;
using System.Collections;

public class MoneyGUI : MonoBehaviour 
{
	// Position between 0-1 in both X and Y
	public Vector2 v2Position;
	// Size between 0-1 in both X and Y
	public Vector2 v2Size;

	// Money Text
	string sMoneyText;
	// Credit Money Text
	string sCreditMoneyText;

	// Space between the left-edge of the background and the text
	public float fTextMargin;

	// Background Rect
	Rect rBackgroundRect;
	// Money Text Rect
	Rect rMoneyTextRect;

	// Initialization
	void Start()
	{

	}

	void Update()
	{
		// Update Money Text
		sMoneyText = "Gold: " + InventoryManager.GetGold().ToString();
		sCreditMoneyText = "Credits: " + InventoryManager.GetCredits().ToString();
	}

	// Draws and resizes GUI every frame
	void ResizeGUI()
	{
		rBackgroundRect = new Rect(v2Position.x * Screen.width,
		                           v2Position.y * Screen.height, 
		                           v2Size.x * Screen.width,
		                           v2Size.y * Screen.height);
		
		rMoneyTextRect = new Rect((v2Position.x + fTextMargin) * Screen.width,
		                          v2Position.y * Screen.height, 
		                          v2Size.x * Screen.width / 2,
		                          v2Size.y * Screen.height);
	}

	// GUI
	void OnGUI()
	{
		// Calls resize function
		ResizeGUI();

		// Draw Background
		GUI.Box (rBackgroundRect, new Texture());

		// Set text alignment to Middle-Left
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleLeft;

		// Draw Text
		GUI.Label (rMoneyTextRect, sMoneyText + "    " + sCreditMoneyText, style);
	}
}
