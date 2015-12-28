using UnityEngine;
using System.Collections;

public class WindowGUI : MonoBehaviour 
{	
	// Window ID
	public int iWindowID;
	// Window Title
	public string sWindowTitle;

	// Position between 0-1 in both X and Y
	public Vector2 v2Position;
	// Size between 0-1 in both X and Y
	public Vector2 v2Size;

	// Window Rect
	Rect rWindowRect;

	// Scroll Bar Position
	protected Vector2 v2ScrollPosition = Vector2.zero;
	
	// Sets GUI Skin
	public GUISkin GUIButtonSkin = null;

	// Initialization
	void Start ()
	{
		
	}

	protected void SetPosition(float x, float y)
	{
		v2Position.x = x / Screen.width - v2Size.x / 2;
		v2Position.y = 1 - (y / Screen.height + v2Size.y / 2);
	}

	// Draw Window
	protected void DrawWindow()
	{
		// Sets skin
		GUI.skin = GUIButtonSkin;

		// Draws and resizes GUI every frame
		rWindowRect = new Rect(v2Position.x * Screen.width,
		                       v2Position.y * Screen.height, 
		                       v2Size.x * Screen.width,
		                       v2Size.y * Screen.height);

		// Create the window
		GUI.Window(iWindowID, rWindowRect, WindowFunction, sWindowTitle);
	}

	// Window Function
	protected virtual void WindowFunction(int windowID)
	{
		
	}

	// Open a window
	public static void DisableControls()
	{
		CameraControl.bActive = false;
		ActiveQuestGUI.bButtonsActive = false;
		ButtonGUI.bActive = false;
	}

	// Close a window
	public static void EnableControls()
	{
		ButtonGUI.bActive = true;
		ActiveQuestGUI.bButtonsActive = true;
		CameraControl.bActive = true;
	}
}
