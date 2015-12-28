using UnityEngine;
using System.Collections;

public class InventoryButtonGUI : ButtonGUI 
{
	// Button Pressed flag
	public static bool bButtonPressed = false;
	// Button Active flag
	public static bool bButtonActive = true;

	public override void ButtonEffect()
	{
		// If this button is active then open the Inventory Window
		if (bButtonActive)
		{
			WindowGUI.DisableControls();
			bButtonPressed = !bButtonPressed;

			print ("Inventory Button");
		}
	}
}
