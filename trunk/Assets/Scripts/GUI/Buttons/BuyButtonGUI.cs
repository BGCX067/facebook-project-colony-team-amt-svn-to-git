using UnityEngine;
using System.Collections;

public class BuyButtonGUI : ButtonGUI 
{
	// Button Pressed flag
	public static bool bButtonPressed = false;
	// Button Active flag
	public static bool bButtonActive = true;

	public override void ButtonEffect()
	{
		// If this button is active then open the Buy Window
		if (bButtonActive)
		{
			WindowGUI.DisableControls();
			bButtonPressed = !bButtonPressed;
			
			print ("Buy Button");
		}
	}
}
