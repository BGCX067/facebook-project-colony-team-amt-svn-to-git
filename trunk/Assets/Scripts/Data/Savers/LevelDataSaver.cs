using UnityEngine;
using System.Collections;
using System.IO;

public class LevelDataSaver : MonoBehaviour 
{
	// File path
	string sFilePath;
	// File name + extension
	public string sFileName;

	// Link to the Database
	string sWorldLinkString = "http://studentnet.cst.beds.ac.uk/~1201561/Project%20Colony/SubmitGameData.php?UserID=";

	// Use this for initialization
	void Start () 
	{
	}

	public void SaveData()
	{
		if (DataReader.bOnlineSave)
		{
			StartCoroutine(SaveOnlineData());
		}
		else
		{
			sFilePath = Application.dataPath + "/Data/";
			SaveOfflineData();
		}
	}

	// Offline Save
	public void SaveOfflineData()
	{
		string sTextLinkString = sFilePath + sFileName;

		// Inventory Data
		string levelData = "";
		
		// Append the data into the string
		// Level and XP Data
		levelData += LevelManager.iGetLevel().ToString() + "," +
			LevelManager.iGetXP().ToString();
		
		// Text Writer
		StreamWriter writer = new StreamWriter(sTextLinkString, false);
		// Overwrite map data
		writer.WriteLine(levelData);
		
		// Close the writer
		writer.Close ();
		
		Debug.Log("Level Save Complete");
	}

	// Online save
	IEnumerator SaveOnlineData()
	{
		// Inventory Data
		string levelData = "";
		
		// Append the data into the string
		// Level and XP Data
		levelData += LevelManager.iGetLevel().ToString() + "," +
			LevelManager.iGetXP().ToString();

		Debug.Log ("Start Level Saving");
		
		// Send the data to the server
		WWW webRequest = new WWW (sWorldLinkString + FBManager.iFacebookID.ToString () + "&Column=LevelData&Data=" + levelData);
		yield return webRequest;
		
		Debug.Log (webRequest.text);
		
		Debug.Log("Level Save Complete");
	}
}
