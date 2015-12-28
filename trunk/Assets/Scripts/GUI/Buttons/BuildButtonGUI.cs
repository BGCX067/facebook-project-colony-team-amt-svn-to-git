using UnityEngine;
using System.Collections;

public class BuildButtonGUI : ButtonGUI 
{
	// Button Pressed flag
	public static bool bButtonPressed = false;
	// Button Active flag
	public static bool bButtonActive = true;

	public override void ButtonEffect()
	{
		// If this button is active then open the Build Window
		if (bButtonActive)
		{
			WindowGUI.DisableControls();
			bButtonPressed = !bButtonPressed;

			print ("Build Button");
		}
	}
}
