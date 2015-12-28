using UnityEngine;
using System.Collections;

public class BuildGUIWindow : WindowGUI
{
	// Building Texture
	public Texture2D[] buildingIcons;

	// Finish Building Button Script
	FinishBuildingButtonGUI finishButtonScript;

	// Initialization
	void Start () 
	{
		finishButtonScript = GameObject.Find ("FinishBuildingButton").GetComponent<FinishBuildingButtonGUI>();
	}

	// GUI
	void OnGUI()
	{
		// If the Build Window is open then draw it
		if (BuildButtonGUI.bButtonPressed)
		{
			DrawWindow();
		}
	}

	// Window Function
	protected override void WindowFunction(int WindowID)
	{
		// Window Area
		Vector2 windowArea = new Vector2(v2Size.x * Screen.width, v2Size.y * Screen.height);

		// Area of the window used for the Scroll View
		Rect scrollArea = new Rect (windowArea.x * 0.05f, windowArea.y * 0.2f,
		                            windowArea.x * 0.85f, windowArea.y * 0.6f);

		// Scrollable Area
		Rect scrollViewArea = new Rect (0, 0, scrollArea.width - 100, scrollArea.height * 2);
			
		// Label GUI Style
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleCenter;

		// If the Close Button is clicked then close the window
		if (GUI.Button (new Rect(windowArea.x - 30, 5, 20, 20), "X"))
		{
			EnableControls();
			BuildButtonGUI.bButtonPressed = false;
		}

		// Create the Scroll View and Update the Scroll Bar Position
		v2ScrollPosition = GUI.BeginScrollView(scrollArea, v2ScrollPosition,
		                                       scrollViewArea, false, true);

		for (int i = 0; i < BuildingTypeData.iNoOfTypes; i++)
		{
			int positionX = (i % 3) * (int)windowArea.x / 3;
			int positionY = (i / 3) * 200;

			// Draw the Building Icon
			GUI.DrawTexture (new Rect(positionX + 30, positionY + 50, 80, 80), buildingIcons[i]);
			// Draw the Building Name
			GUI.Label (new Rect(positionX + 15, positionY + 130, 120, 25), BuildingTypeData.aBuildingTypes[i].sName, style); 

			// If the Buy Building Button is clicked then close the window 
			// and enter the Building phase
			if (GUI.Button(new Rect(positionX, positionY + 160, 140, 30), BuildingTypeData.aBuildingTypes[i].iCost.ToString() + "G"))
			{
				// Deactivate GUI Menu Buttons
				BuildButtonGUI.bButtonActive = false;
				MoveButtonGUI.bButtonActive = false;
				//CraftButtonGUI.bButtonActive = false;
				InventoryButtonGUI.bButtonActive = false;

				// Allow buildings to be built
				Tile.bReadyToBuild = true;
				// Activate Finish Building Button 
				finishButtonScript.enabled = true;

				// Set Building Values for Selected Building Types
				Tile.iBuildingID = BuildingTypeData.aBuildingTypes[i].iID;
				Tile.iBuildingWidth = BuildingTypeData.aBuildingTypes[i].iWidth;
				Tile.iBuildingHeight = BuildingTypeData.aBuildingTypes[i].iHeight;
				Tile.iBuildingCost = BuildingTypeData.aBuildingTypes[i].iCost;

				// Enable other GUI buttons
				EnableControls();
				// Close Build Window
				BuildButtonGUI.bButtonPressed = false;
			}
		}

		// End Scroll View
		GUI.EndScrollView();
	}
}