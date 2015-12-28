using UnityEngine;
using System.Collections;
using System.IO;

public class MapDataSaver : MonoBehaviour 
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

	public void SaveOfflineData()
	{
		// Set up the Text File Path
		string sTextLinkString = sFilePath + sFileName;
		
		// World Data
		string worldData = "";
		
		// Array of Tile IDs
		int[,] tileIDArray = WorldManager.aiTileIDArray;
		
		// Loop through each grid space 
		for (int y = 0; y < tileIDArray.GetLength(0); y++) // Rows
		{
			string row = "";
			
			for (int x = 0; x < tileIDArray.GetLength(1); x++) // Columns
			{
				// Create a new column element
				string column = "";
				
				// Set the Tile ID data for the new element
				column = tileIDArray[y, x].ToString();
				
				// Break up each tile with a ','
				if (x < tileIDArray.GetLength(1) - 1)
				{
					column += ",";
				}
				
				// Add the column element into the row element
				row += column;
			}
			
			// Break up each row with a '|'
			if (y < tileIDArray.GetLength(0) - 1)
			{
				row += '|';
			}
			
			// Add the row element into the tiles node
			worldData += row;
		}
		
		Debug.Log (worldData);
		
		Debug.Log ("Start Saving");
		
		// Text Writer
		StreamWriter writer = new StreamWriter(sTextLinkString, false);
		// Overwrite map data
		writer.WriteLine(worldData);
		
		// Close the writer
		writer.Close ();
		
		Debug.Log("Save Complete");
	}
	
	// Saves the world data to the Game Server
	IEnumerator SaveOnlineData()
	{
		Debug.Log("Saving");
		
		// World Data
		string worldData = "";
		
		// Array of Tile IDs
		int[,] tileIDArray = WorldManager.aiTileIDArray;
		
		// Loop through each grid space 
		for (int y = 0; y < tileIDArray.GetLength(0); y++) // Rows
		{
			string row = "";
			
			for (int x = 0; x < tileIDArray.GetLength(1); x++) // Columns
			{
				// Create a new column element
				string column = "";
				
				// Set the Tile ID data for the new element
				column = tileIDArray[y, x].ToString();
				
				// Break up each tile with a ','
				if (x < tileIDArray.GetLength(1) - 1)
				{
					column += ",";
				}
				
				// Add the column element into the row element
				row += column;
			}
			
			// Break up each row with a '|'
			if (y < tileIDArray.GetLength(0) - 1)
			{
				row += '|';
			}
			
			// Add the row element into the tiles node
			worldData += row;
		}
		
		print (worldData);
		
		Debug.Log ("Start Saving");

		// Send the data to the server
		WWW webRequest = new WWW (sWorldLinkString + FBManager.iFacebookID.ToString () + "&Column=MapData&Data=" + worldData);
		yield return webRequest;
		
		Debug.Log (webRequest.text);
		
		Debug.Log("Save Complete");
	}
}