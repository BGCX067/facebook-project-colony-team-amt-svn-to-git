using UnityEngine;
using System.Collections;

public class VolumeDownButtonGUI : ButtonGUI 
{
	public override void ButtonEffect()
	{
		print ("Volume Down Button");
		SoundManager.volumeDown();
	}
}
