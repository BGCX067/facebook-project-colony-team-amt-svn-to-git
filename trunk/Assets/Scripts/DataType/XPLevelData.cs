using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;

public class XPLevelData : MonoBehaviour
{
	// File Name
	public string sFileName;
	// File Path
	string sFilePath;

	// Array of Experience Levels
	public static int[] aiXPLevels;
	// Array of Gold Rewards
	public static int[] aiGoldRewards;
	// Array of Credit Rewards
	public static int[] aiCreditRewards;
	// Number of XP Levels
	public static int iNoOfLevels;

	// Initialization
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
			stream = client.OpenRead("http://studentnet.cst.beds.ac.uk/~1201561/Project%20Colony/XPLevelsData.txt");
			reader = new StreamReader(stream);
		}
		else
		{
			reader = new StreamReader(sFilePath);
		}

		// If the file couldn't be read then post an error
		if (reader == null)
		{
			Debug.LogError ("Can't load XP Level Type Data file");
		}
		else
		{
			Debug.Log("Start Reading XP Level Data");

			// Set the number of levels
			iNoOfLevels = int.Parse (reader.ReadLine());
			// Create new arrays
			aiXPLevels = new int[iNoOfLevels];
			aiGoldRewards = new int[iNoOfLevels];
			aiCreditRewards = new int[iNoOfLevels];

			// Read the XP level data and split it
			string dataTxt = reader.ReadLine ();
			string[] levelsTxt = dataTxt.Split('|');

			// Set the value for each XP level
			for (int i = 0; i < iNoOfLevels; i++)
			{
				string[] valueTxt = levelsTxt[i].Split(',');

				aiXPLevels[i] = int.Parse (valueTxt[0]);
				aiGoldRewards[i] = int.Parse(valueTxt[1]);
				aiCreditRewards[i] = int.Parse(valueTxt[2]);
			}

			Debug.Log("XP Level Data Loaded");
		}

		// Close the file reader
		reader.Close ();

		if (DataReader.bOnlineLoad)
		{
			stream.Close ();
		}
	}
}
