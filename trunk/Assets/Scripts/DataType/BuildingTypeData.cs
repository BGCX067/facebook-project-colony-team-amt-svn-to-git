using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Net;

// Building Type - Stores ID, Name, Size, Cost, Resource Time and Required Level
public struct BuildingType
{
	public int iID;
	public string sName;
	public int iWidth;
	public int iHeight;
	public int iCost;
	public TimeSpan resourceTime;
	public int iRequiredLevel;

	public void SetValues(int id, string name, int width, int height, int cost, TimeSpan time, int level)
	{
		iID = id;
		sName = name;
		iWidth = width;
		iHeight = height;
		iCost = cost;
		resourceTime = time;
		iRequiredLevel = level;
	}
}

public class BuildingTypeData : MonoBehaviour
{
	// Array of Building Types
	public static BuildingType[] aBuildingTypes;
	// Number of Building Types
	public static int iNoOfTypes;

	// File Name
	public string sFileName;
	// File Path
	string sFilePath;

	// Use this for initialization
	void Awake () 
	{
		sFilePath = Application.dataPath + "/Data/" + sFileName;

		LoadData();
	}

	// Load Data
	void LoadData()
	{
		// File Reader
		StreamReader reader;
		Stream stream = default(Stream);
		
		if (DataReader.bOnlineLoad)
		{
			WebClient client = new WebClient();
			stream = client.OpenRead("http://studentnet.cst.beds.ac.uk/~1201561/Project%20Colony/BuildingTypeData.txt");
			reader = new StreamReader(stream);
		}
		else
		{
			reader = new StreamReader(sFilePath);
		}

		// If the file couldn't be read then post an error
		if (reader == null)
		{
			Debug.LogError ("Can't load Building Type Data file");
		}
		else
		{
			Debug.Log("Start Reading Building Data");

			// Set the number of building types
			iNoOfTypes = int.Parse (reader.ReadLine());
			// Create a new array of Building Types
			aBuildingTypes = new BuildingType[iNoOfTypes];

			// Read the data, split it and assign the values for each building type
			for (int i = 0; i < iNoOfTypes; i++)
			{
				string dataTxt = reader.ReadLine();
				string[] buildingDataTxt = dataTxt.Split(',');

				int id = int.Parse(buildingDataTxt[0]);
				string name = buildingDataTxt[1];
				int width = int.Parse(buildingDataTxt[2]);
				int height = int.Parse(buildingDataTxt[3]);
				int cost = int.Parse(buildingDataTxt[4]);
				TimeSpan time = TimeSpan.Parse(buildingDataTxt[5]);
				int level = int.Parse(buildingDataTxt[6]);

				aBuildingTypes[i].SetValues(id, name, width, height, cost, time, level);

				Debug.Log (aBuildingTypes[i].resourceTime);
			}

			Debug.Log("Building Data Loaded");
		}

		// Close the reader
		reader.Close();

		if (DataReader.bOnlineLoad)
		{
			stream.Close ();
		}
	}
}
