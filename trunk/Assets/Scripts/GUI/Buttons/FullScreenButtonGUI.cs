using UnityEngine;
using System.Collections;

public class FullScreenButtonGUI : ButtonGUI 
{
	public override void ButtonEffect()
	{
		// Converts screen to opposite of current state
		Screen.fullScreen = !Screen.fullScreen;

		print ("Full Screen Button");
	}
}
