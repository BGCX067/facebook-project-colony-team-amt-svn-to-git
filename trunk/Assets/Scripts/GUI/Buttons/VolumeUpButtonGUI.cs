using UnityEngine;
using System.Collections;

public class VolumeUpButtonGUI : ButtonGUI 
{
	public override void ButtonEffect()
	{
		print ("Volume Up Button");
		SoundManager.volumeUp();
	}
}
