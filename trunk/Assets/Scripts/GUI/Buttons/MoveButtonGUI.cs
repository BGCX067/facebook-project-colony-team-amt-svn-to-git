using UnityEngine;
using System.Collections;

public class MoveButtonGUI : ButtonGUI 
{
	// Button Pressed flag
	public static bool bButtonPressed = false;
	// Button Active flag
	public static bool bButtonActive = true;

	// Finish Building Button 
	FinishBuildingButtonGUI finishButtonScript;

	void Start()
	{
		finishButtonScript = GameObject.Find ("FinishBuildingButton").GetComponent<FinishBuildingButtonGUI>();
	}

	public override void ButtonEffect()
	{
		// If this button is active then open the Move Window
		if (bButtonActive)
		{
			bButtonPressed = !bButtonPressed;
				
			// Set MovingBuilding to true
			Building.bMovingBuilding = true;
			
			// Deactivate the GUI Buttons
			BuildButtonGUI.bButtonActive = false;
			MoveButtonGUI.bButtonActive = false;
			CraftButtonGUI.bButtonActive = false;
			InventoryButtonGUI.bButtonActive = false;
			
			// Activate the Finish Building Button
			finishButtonScript.enabled = true;

			print ("Move Button");
		}
	}
}
