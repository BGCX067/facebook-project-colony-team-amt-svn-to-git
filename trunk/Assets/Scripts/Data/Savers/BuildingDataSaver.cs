using UnityEngine;
using System.Collections;
using System.IO;

public class BuildingDataSaver : MonoBehaviour 
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

	void SaveOfflineData()
	{
		// Set up the Text File Path
		string sTextLinkString = sFilePath + sFileName;
		
		// World Data
		string worldData = "";
		
		for (int i = 0; i < WorldManager.lBuildingList.Count; i++)
		{
			string buildingData = WorldManager.lBuildingList[i].sGetObjectSaveData();

			worldData += buildingData;

			if (i < WorldManager.lBuildingList.Count - 1)
			{
				worldData += "|";
			}
		}
		
		// Text Writer
		StreamWriter writer = new StreamWriter(sTextLinkString, false);
		// Overwrite map data
		writer.WriteLine(worldData);
		
		// Close the writer
		writer.Close ();
		
		Debug.Log("Building Save Complete");
	}
	
	// Saves the world data to the Game Server
	IEnumerator SaveOnlineData()
	{
		Debug.Log("Saving");
		
		// World Data
		string worldData = "";
	
		print (WorldManager.lBuildingList.Count);

		// Loop through each grid space 
		for (int i = 0; i < WorldManager.lBuildingList.Count; i++)
		{
			string buildingData = WorldManager.lBuildingList[i].sGetObjectSaveData();

			print (buildingData);

			worldData += buildingData;

			print (worldData);
			
			if (i < WorldManager.lBuildingList.Count - 1)
			{
				worldData += "|";
			}
		}
		
		print (worldData);
		
		Debug.Log ("Start Building Saving");

		// Send the data to the server
		WWW webRequest = new WWW (sWorldLinkString + FBManager.iFacebookID.ToString () + "&Column=BuildingData&Data=" + worldData);
		print (sWorldLinkString + FBManager.iFacebookID.ToString () + "&Column=BuildingData&Data=" + worldData);
		yield return webRequest;
		
		Debug.Log (webRequest.text);
		
		Debug.Log("Building Save Complete");
	}
}
