using UnityEngine;
using System.Collections;
using System.IO;

public class InventoryDataSaver : MonoBehaviour 
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
		// Set up the Text File Path
		string sTextLinkString = sFilePath + sFileName;

		// Inventory Data
		string inventoryData = "";

		// Append the data into the string
		// Money (Ingame and Credit)
		inventoryData += InventoryManager.GetGold().ToString() + ","
			+ InventoryManager.GetCredits().ToString() + "|";

		// Resources
		int[] resources = InventoryManager.GetResources();

		for (int i = 0; i < resources.Length; i++)
		{
			inventoryData += resources[i].ToString();

			if (i < resources.Length - 1)
			{
				inventoryData += ",";
			}
		}

		// Text Writer
		StreamWriter writer = new StreamWriter(sTextLinkString, false);
		// Overwrite map data
		writer.WriteLine(inventoryData);
		
		// Close the writer
		writer.Close ();
		
		Debug.Log("Inventory Save Complete");
	}

	// Online save
	IEnumerator SaveOnlineData()
	{
		// Inventory Data
		string inventoryData = "";

		// Append the data into the string
		// Money (Ingame and Credit)
		inventoryData += InventoryManager.GetGold().ToString() + ","
			+ InventoryManager.GetCredits().ToString() + "|";

		// Resources
		int[] resources = InventoryManager.GetResources();
		
		for (int i = 0; i < resources.Length; i++)
		{
			inventoryData += resources[i].ToString();
			
			if (i < resources.Length - 1)
			{
				inventoryData += ",";
			}
		}

		Debug.Log ("Start Inventory Saving");

		// Send the data to the server
		WWW webRequest = new WWW (sWorldLinkString + FBManager.iFacebookID.ToString () + "&Column=InventoryData&Data=" + inventoryData);
		yield return webRequest;
		
		Debug.Log (webRequest.text);
		
		Debug.Log("Inventory Save Complete");
	}
}
