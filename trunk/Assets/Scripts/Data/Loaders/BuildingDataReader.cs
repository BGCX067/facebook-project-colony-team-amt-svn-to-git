using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Building Data - Stores Tile Position, Size, ID, StartTime and ResourceReady flag
public struct BuildingData
{
	// Tile X
	public int iTileX;
	// Tile Y
	public int iTileY;
	// Width
	public int iWidth;
	// Height
	public int iHeight;
	// Object ID
	public int iObjectID;
	// Time the resource started mining
	public DateTime resourceStartTime;
	// Resource Ready flag
	public bool bResourceReady;
	// Building Inactive flag
	public bool bInActive;

	// Set Values for building data
	public void SetValues(int tileX, int tileY, int width, int height,
	int id, DateTime startTime, bool ready, bool inactive)
	{
		iTileX = tileX;
		iTileY = tileY;
		iWidth = width;
		iHeight = height;
		iObjectID = id;
		
		resourceStartTime = startTime;

		bResourceReady = ready;
		bInActive = inactive;
	}
}

public class BuildingDataReader : DataReader
{
	// Initialization
	void Start () 
	{
		DecodeData();
	}

	void DecodeData()
	{
		// List of Building Data structs
		List<BuildingData> buildingDataList = new List<BuildingData>();
	
		// Read data
		string dataTxt = sLoadData("BuildingData", sFileName);
		
		// Check data to see if empty
		if (dataTxt == "" || dataTxt.Contains("NewUser") || dataTxt == null)
		{
			Debug.Log ("No User Building Data");
		}
		else
		{
			// Split the data
			string[] buildingTxt = dataTxt.Split('|');

			// If the string was split successfully then loop through
			// each piece and apply it to the correct variable
			if (buildingTxt != null)
			{
				foreach (string building in buildingTxt)
				{
					string[] attributes = building.Split(',');

					int tileX = int.Parse (attributes[1]);
					int tileY = int.Parse (attributes[2]);
					int width = int.Parse (attributes[3]);
					int height = int.Parse (attributes[4]);
					int id = int.Parse (attributes[5]);

					DateTime startTime = DateTime.Parse(attributes[6].Replace("-", " "));

					bool resourceReady = bool.Parse(attributes[7]);
					bool inactive = bool.Parse(attributes[8]);

					BuildingData buildingStruct = new BuildingData();
					buildingStruct.SetValues(tileX, tileY, width, height,
					                         id, startTime, resourceReady, inactive);

					print (buildingStruct.resourceStartTime);

					// Add to the Building Data list
					buildingDataList.Add(buildingStruct);
				}
			}
		}

		// Start Creating the Buildings
		GameObject.Find ("WorldManager").GetComponent<WorldManager>().CreateBuildings(buildingDataList);

		Debug.Log("Building Reading finished");
	}
}
