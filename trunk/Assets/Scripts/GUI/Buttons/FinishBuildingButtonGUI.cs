using UnityEngine;
using System.Collections;

public class FinishBuildingButtonGUI : ButtonGUI 
{
	// Use this for initialization
	void Start () 
	{
	
	}

	public override void ButtonEffect()
	{
		Debug.Log ("Finish Building Button");

		// Activate the Main GUI Buttons
		BuildButtonGUI.bButtonActive = true;
		MoveButtonGUI.bButtonActive = true;
		CraftButtonGUI.bButtonActive = true;
		InventoryButtonGUI.bButtonActive = true;

		MoveButtonGUI.bButtonPressed = false;
	
		// Stop Building phase
		Tile.bReadyToBuild = false;
		// Stop Moving phase
		Building.bMovingBuilding = false;
		// Deactivate this script
		enabled = false;
	}
}
