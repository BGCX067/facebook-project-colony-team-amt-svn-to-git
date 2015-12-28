using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;

// Resource Type - Stores ID, Name, Value and XP Reward
public struct ResourceType
{
	public int iID;
	public string sName;
	public int iValue;
	public int iXP;

	// Set Values
	public void SetValues(int id, string name, int value, int xp)
	{
		iID = id;
		sName = name;
		iValue = value;
		iXP = xp;
	}
}

public class ResourceTypeData: MonoBehaviour 
{
	// Array of Resource Types
	public static ResourceType[] aResourceTypes;
	// Number of Resource Types
	public static int iNoOfTypes = 1;

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

	void LoadData()
	{
		// File Reader
		StreamReader reader;
		Stream stream = default(Stream);

		if (DataReader.bOnlineLoad)
		{
			WebClient client = new WebClient();
			stream = client.OpenRead("http://studentnet.cst.beds.ac.uk/~1201561/Project%20Colony/ResourceTypeData.txt");
			reader = new StreamReader(stream);
		}
		else
		{
			reader = new StreamReader(sFilePath);
		}

		// If the file couldn't be read then post an error
		if (reader == null)
		{
			Debug.LogError ("Can't load Resource Type Data file");
		}
		else
		{
			Debug.Log("Start Reading Resource Data");

			// Set the number of resource types
			iNoOfTypes = int.Parse (reader.ReadLine());
			// Create a new array of resource types
			aResourceTypes = new ResourceType[iNoOfTypes];

			// Read the data, split it and assign the values for each resource
			for (int i = 0; i < iNoOfTypes; i++)
			{
				string dataTxt = reader.ReadLine();
				string[] resourceTxt = dataTxt.Split(',');

				int id = int.Parse (resourceTxt[0]);
				string name = resourceTxt[1];
				int value = int.Parse (resourceTxt[2]);
				int xp = int.Parse (resourceTxt[3]);

				aResourceTypes[i].SetValues(id, name, value, xp);
			}

			Debug.Log("Resource Data Loaded");
		}

		// Close the reader
		reader.Close();

		if (DataReader.bOnlineLoad)
		{
			stream.Close ();
		}
	}
}
