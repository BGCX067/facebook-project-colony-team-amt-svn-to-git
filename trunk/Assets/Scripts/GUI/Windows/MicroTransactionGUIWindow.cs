using UnityEngine;
using System.Collections;

public class MicroTransactionGUIWindow : WindowGUI 
{
	// Use this for initialization
	void Start () 
	{
	
	}

	void OnGUI()
	{
		if (BuyButtonGUI.bButtonPressed) 
		{
			DrawWindow();
		}
	}

	protected override void WindowFunction(int windowID)
	{
		// Window Area
		Vector2 windowArea = new Vector2(v2Size.x * Screen.width, v2Size.y * Screen.height);

		GUIStyle style = new GUIStyle();
		style.fontSize = 14;
		style.alignment = TextAnchor.MiddleLeft;
		
		style.normal.textColor = Color.white;

		// If the Close Button is clicked then close the window
		if (GUI.Button (new Rect(windowArea.x - 30, 5, 20, 20), "X"))
		{
			EnableControls();
			BuyButtonGUI.bButtonPressed = false;
		}

		GUI.Label (new Rect (windowArea.x * 0.2f, windowArea.y * 0.3f, windowArea.x * 0.4f, windowArea.y * 0.15f), 
		          "Buy 10 Credits (£7.99)", style);

		if (GUI.Button (new Rect (windowArea.x * 0.7f, windowArea.y * 0.325f, windowArea.x * 0.1f, windowArea.y * 0.1f), "Buy"))
		{
			InventoryManager.AddCredits(10);
		}

		GUI.Label (new Rect (windowArea.x * 0.2f, windowArea.y * 0.5f, windowArea.x * 0.4f, windowArea.y * 0.15f), 
		           "Buy 1000 Gold (3x Credits)", style);
		
		if (GUI.Button (new Rect (windowArea.x * 0.7f, windowArea.y * 0.525f, windowArea.x * 0.1f, windowArea.y * 0.1f), "Buy"))
		{
			if (InventoryManager.TakeCredits(3))
			{
				InventoryManager.AddGold(1000);
			}
		}

		GUI.Label (new Rect (windowArea.x * 0.2f, windowArea.y * 0.7f, windowArea.x * 0.4f, windowArea.y * 0.15f), 
		           "Buy 10000 Gold: (10x Credits)", style);
		
		if (GUI.Button (new Rect (windowArea.x * 0.7f, windowArea.y * 0.725f, windowArea.x * 0.1f, windowArea.y * 0.1f), "Buy"))
		{
			if (InventoryManager.TakeCredits(10))
			{
				InventoryManager.AddGold(10000);
			}
		}
	}
}
