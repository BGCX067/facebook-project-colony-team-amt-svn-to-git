using UnityEngine;
using System.Collections;

public class LevelDataReader : DataReader 
{
	// Initialization
	void Start () 
	{
		DecodeData();
	}

	public void DecodeData()
	{
		// Level
		int level = 1;
		// XP
		int xp = 0;

		// Read Data
		string dataTxt = sLoadData("LevelData", sFileName);
		
		// Check data to see if empty
		if (dataTxt == "" || dataTxt.Contains("NewUser") || dataTxt == null)
		{
			Debug.Log ("No User Level Data");
		}
		else
		{
			// Split the data and assign the values
			string[] levelTxt = dataTxt.Split(',');

			level = int.Parse (levelTxt[0]);
			xp = int.Parse (levelTxt[1]);
		}

		// Set up the initial values
		LevelManager.Init(level, xp);

		Debug.Log("Level Reading finished");
	}
}
