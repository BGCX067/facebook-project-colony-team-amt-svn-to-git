using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;

// Resource Type - Stores ID, Name, Value, XP Reward, Resource1 and Resourc2 Cost and Resource Description
public struct CraftResourceType
{
	public int iID;
	public string sName;
	public int iXP;
	public int iResource1ID;
	public int iResource2ID;
	public int iResource1Cost;
	public int iResource2Cost;
	public string sResourceText;
	
	// Set Values
	public void SetValues(int id, string name, int xp, int resource1ID, int resource2ID, int resource1, int resource2, string resourceText)
	{
		iID = id;
		sName = name;
		iXP = xp;
		iResource1ID = resource1ID;
		iResource2ID = resource2ID;
		iResource1Cost = resource1;
		iResource2Cost = resource2;
		sResourceText = resourceText;
	}
}

public class CraftResourceTypeData: MonoBehaviour 
{
	// Array of Resource Types
	public static CraftResourceType[] aCraftResourceTypes;
	// Number of Resource Types
	public static int iNoOfCraftTypes = 1;
	
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
			stream = client.OpenRead("http://studentnet.cst.beds.ac.uk/~1201561/Project%20Colony/CraftResourceTypeData.txt");
			reader = new StreamReader(stream);
		}
		else
		{
			reader = new StreamReader(sFilePath);
		}
		
		// If the file couldn't be read then post an error
		if (reader == null)
		{
			Debug.LogError ("Can't load Craft Resource Type Data file");
		}
		else
		{
			Debug.Log("Start Reading Craft Resource Data");
			
			// Set the number of resource types
			iNoOfCraftTypes = int.Parse (reader.ReadLine());
			// Create a new array of resource types
			aCraftResourceTypes = new CraftResourceType[iNoOfCraftTypes];
			
			// Read the data, split it and assign the values for each resource
			for (int i = 0; i < iNoOfCraftTypes; i++)
			{
				string dataTxt = reader.ReadLine();
				string[] craftResourceTxt = dataTxt.Split(',');
				
				int id = int.Parse (craftResourceTxt[0]);
				string name = craftResourceTxt[1];
				int xp = int.Parse (craftResourceTxt[2]);
				int resource1ID =int.Parse (craftResourceTxt[3]);
				int resource2ID = int.Parse (craftResourceTxt[4]);
				int resource1 = int.Parse (craftResourceTxt[5]);
				int resource2 = int.Parse (craftResourceTxt[6]);
				string resourceText = craftResourceTxt[7];
				
				aCraftResourceTypes[i].SetValues(id, name, xp, resource1ID, resource2ID, resource1, resource2, resourceText);
			}
			
			Debug.Log("Craft Resource Data Loaded");
		}
		
		// Close the reader
		reader.Close();

		if (DataReader.bOnlineLoad)
		{
			stream.Close ();
		}
	}
}
