using UnityEngine;
using System.Collections;

public class PlayerInventoryGUIWindow : MonoBehaviour 
{
	// Position between 0-1 in both X and Y
	public Vector2 v2Position;
	// Size between 0-1 in both X and Y
	public Vector2 v2Size;
	
	// Window Rect
	Rect rWindowRect;

	// Sets GUI Skin
	public GUISkin GUIButtonSkin = null;

	// Initialization
	void Start ()
	{

	}
	
	// GUI
	void OnGUI()
	{
		if(InventoryButtonGUI.bButtonPressed == true)
		{
			// Sets skin
			GUI.skin = GUIButtonSkin;

			// Draws and resizes GUI every frame
			rWindowRect = new Rect(v2Position.x * Screen.width,
			                       v2Position.y * Screen.height, 
			                       v2Size.x * Screen.width,
			                       v2Size.y * Screen.height);
			
			GUI.Window(1, rWindowRect, WindowFunction, "Inventory");
		}
	}
	
	// Window Function
	void WindowFunction(int windowID)
	{
		
	}
}
