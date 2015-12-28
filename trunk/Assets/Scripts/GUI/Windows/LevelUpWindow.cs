using UnityEngine;
using System.Collections;

public class LevelUpWindow : WindowGUI 
{
	public static bool bWindowActive = false;

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnGUI()
	{
		if (bWindowActive)
		{
			DrawWindow();
		}
	}

	protected override void WindowFunction(int windowID)
	{
		// Window Area
		Vector2 windowArea = new Vector2(v2Size.x * Screen.width, v2Size.y * Screen.height);

		GUIStyle style = new GUIStyle();
		style.fontSize = 24;
		style.alignment = TextAnchor.MiddleCenter;

		style.normal.textColor = Color.white;

		// If the Close Button is clicked then close the window
		if (GUI.Button (new Rect(windowArea.x - 30, 5, 20, 20), "X"))
		{
			EnableControls();
			bWindowActive = false;
		}

		GUI.Label (new Rect(windowArea.x * 0.2f, windowArea.y * 0.075f, windowArea.x * 0.6f, windowArea.y * 0.2f),
		           "Level Up!", style);

		GUI.Label (new Rect(windowArea.x * 0.2f, windowArea.y * 0.3f, windowArea.x * 0.6f, windowArea.y * 0.2f),
		           "You are now Level " + LevelManager.iGetLevel().ToString() + "!", style);

		style.fontSize = 18;

		GUI.Label(new Rect(windowArea.x * 0.3f, windowArea.y * 0.55f, windowArea.x * 0.4f, windowArea.y * 0.15f),
		          "Gold: " + XPLevelData.aiGoldRewards[LevelManager.iGetLevel() - 1].ToString(), style);

		GUI.Label(new Rect(windowArea.x * 0.3f, windowArea.y * 0.65f, windowArea.x * 0.4f, windowArea.y * 0.15f),
		          "Credits: " + XPLevelData.aiCreditRewards[LevelManager.iGetLevel() - 1].ToString(), style);

		if (GUI.Button(new Rect(windowArea.x * 0.4f, windowArea.y * 0.85f, windowArea.x * 0.2f, windowArea.y * 0.1f),
		               "Close"))
		{
			EnableControls();
			bWindowActive = false;
		}
	}
}
