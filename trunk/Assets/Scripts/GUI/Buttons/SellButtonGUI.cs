using UnityEngine;
using System.Collections;

public class SellButtonGUI : ButtonGUI 
{
	// Button Pressed flag
	public static bool bButtonPressed = false;
	// Button Active flag
	public static bool bButtonActive = true;
	
	public override void ButtonEffect()
	{
		// If this button is active then toggle the Sell state
		if (bButtonActive)
		{
			bButtonPressed = !bButtonPressed;
				
			print ("Sell Button");
		}
	}
}
