using UnityEngine;
using System.Collections;

public class MuteButtonGUI : ButtonGUI 
{
	// Textures for sound off and on
	public Texture2D tSoundOn;
	public Texture2D tSoundOff;
	
	public override void ButtonEffect()
	{
		print ("Mute Button");
		SoundManager.muteSounds();

		// changes textures when sound is off/on
		if(SoundManager.bMute == false)
		{
			t2Texture = tSoundOn;
		}
		else
		{
			t2Texture = tSoundOff;
		}
	}
}
