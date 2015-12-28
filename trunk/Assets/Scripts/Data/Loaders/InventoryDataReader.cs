using UnityEngine;
using System.Collections;

public class InventoryDataReader : DataReader 
{
	// Use this for initialization
	void Start () 
	{
		DecodeData();
	}

	// Decode the data
	void DecodeData()
	{
		// Gold
		int gold = 0;
		// Credits
		int credits = 0;

		// Number of Resource Types
		int noOfResources = ResourceTypeData.iNoOfTypes;
		Debug.Log (noOfResources);
		// Resource Array
		int[] resources = new int[noOfResources];
	
		// Read Data
		string dataTxt = sLoadData("InventoryData", sFileName);

		// Check data to see if empty
		if (dataTxt == "" || dataTxt.Contains("NewUser") || dataTxt == null)
		{
			Debug.Log ("No User Inventory Data");

			gold = 1000;
			credits = 10;

			for (int i = 0; i < noOfResources; i++)
			{
				resources[i] = 0;
			}
		}
		else
		{
			// Split the data
			string[] inventoryTxt = dataTxt.Split ('|');

			// If the data was successfully split then assign the values to the correct variables
			if (inventoryTxt != null)
			{
				// Money
				string[] moneyTxt = inventoryTxt[0].Split(',');

				gold = int.Parse(moneyTxt[0]);
				credits = int.Parse(moneyTxt[1]);

				// Resources
				string[] resourcesTxt = inventoryTxt[1].Split(',');

				print (resourcesTxt.Length);
				print (noOfResources);

				for (int i = 0; i < noOfResources; i++)
				{
					print (i);

					resources[i] = int.Parse (resourcesTxt[i]);
				}
			}
		}

		// Set the Initial Values for the Money and Resources
		InventoryManager.Init (gold, credits, resources);

		Debug.Log("Inventory Reading finished");
	}
}
