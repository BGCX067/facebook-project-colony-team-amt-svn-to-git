using UnityEngine;
using System.Collections;

public class FBFriendsGUI : MonoBehaviour 
{
	// Position between 0-1 in both X and Y
	public Vector2 v2Position;
	// Size between 0-1 in both X and Y
	public Vector2 v2Size;

	// Window Rect
	Rect rWindowRect;

	public GUISkin windowSkin;

	// Initialization
	void Start () 
	{

	}

	// GUI
	void OnGUI()
	{
		GUI.skin = windowSkin;

		// Draws and resizes GUI every frame
		rWindowRect = new Rect(v2Position.x * Screen.width,
		                       v2Position.y * Screen.height, 
		                       v2Size.x * Screen.width,
		                       v2Size.y * Screen.height);

		GUI.Window(0, rWindowRect, WindowFunction, "Friends");
	}

	// Window Function
	void WindowFunction(int windowID)
	{

	}
}
