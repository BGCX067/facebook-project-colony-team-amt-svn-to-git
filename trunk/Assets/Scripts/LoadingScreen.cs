using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
	public string levelToLoad;

	public GameObject background;
	public GameObject text;
	public GameObject loadPercent;

	private int loadProgress = 0;

	string sIDString = "";
	string sNameString = "";

	bool bStartLoggingin;

	// Use this for initialization
	void Start ()
	{
		text.SetActive (false);
		loadPercent.SetActive (false);
	}

	void OnGUI()
	{
		if (!bStartLoggingin)
		{
			GUI.Label(new Rect(0.45f * Screen.width, 0.6f * Screen.height, 0.05f * Screen.width, 0.05f * Screen.height),
			          "User ID:");

			string initialText = sIDString;

			GUIStyle style = GUI.skin.GetStyle ("TextField");
			style.alignment = TextAnchor.MiddleLeft;

			sIDString = GUI.TextField (new Rect (0.5f * Screen.width, 0.605f * Screen.height, 
			                         0.075f * Screen.width, 0.04f * Screen.height),
			              sIDString, 10, style);

			int temp = 0;

			if (!int.TryParse(sIDString, out temp))
			{
				sIDString = initialText;
			}

			GUI.Label(new Rect(0.45f * Screen.width, 0.645f * Screen.height, 0.05f * Screen.width, 0.05f * Screen.height),
			          "Name:");

			sNameString = GUI.TextField (new Rect (0.5f * Screen.width, 0.65f * Screen.height, 
			                                    0.075f * Screen.width, 0.04f * Screen.height),
			                          sNameString, 26, style);

			if (GUI.Button (new Rect(0.45f * Screen.width, 0.75f * Screen.height, 0.1f * Screen.width, 0.05f * Screen.height),
			                "Log In"))
			{
				bStartLoggingin = true;

				FBManager.iFacebookID = int.Parse (sIDString);
				FBManager.sProfileName = sNameString;

				if (Application.CanStreamedLevelBeLoaded(levelToLoad))
				{
					Application.LoadLevel(levelToLoad);
				}
			}
		}
	}

	// Loads the level asynchronously and updates the load progress
	IEnumerator DisplayLoadingScreen (string level)
	{
		Debug.Log ("Loading Level");
		// Selects level to load
		AsyncOperation async = Application.LoadLevelAsync (level);

		// Updates progress counter
		while (!async.isDone)
		{
			loadProgress = (int) (async.progress * 100);
			loadPercent.guiText.text = "Load Progress " + loadProgress + "%";

			yield return null;
		}
		Debug.Log ("Load Complete");
	}
}
