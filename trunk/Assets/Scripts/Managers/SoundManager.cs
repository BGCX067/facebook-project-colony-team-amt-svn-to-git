using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	public static bool bMute = false;
	public static float fVolume = 0.3f;	

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Camera.main.GetComponent<AudioSource>().mute = bMute;
		Camera.main.GetComponent<AudioSource>().volume = fVolume;
	}

	public static void muteSounds()
	{
		bMute = !bMute;
	}

	public static void volumeUp()
	{
		if (fVolume < 1)
		{
			fVolume += 0.1f;
		}
	}

	public static void volumeDown()
	{
		if (fVolume > 0)
		{
			fVolume -= 0.1f;
		}
	}
}
