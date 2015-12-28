using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

public struct QuestData
{
	public int iID;
	public string sName;
	public int iNoOfConditions;
	public ConditionData[] aConditionList;
	public int iGoldReward;
	public int iXPReward;

	public void SetValues(int id, string name, int noOfConditions,
	                      ConditionData[] conditionList, int gold, int xp)
	{
		iID = id;
		sName = name;
		iNoOfConditions = noOfConditions;
		aConditionList = conditionList;
		iGoldReward = gold;
		iXPReward = xp;
	}
}

public struct ConditionData
{
	public string sName;
	public string sConditionType;
	public int iTrackID;
	public int iNumberRequired;
}

public class QuestTypeData : MonoBehaviour 
{
	// Array of Quests
	public static QuestData[] aQuests;
	// Number of Quests
	public static int iNoOfQuests;
	
	// File Name
	public string sFileName;
	// File Path
	string sFilePath;

	// Use this for initialization
	void Awake() 
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
			stream = client.OpenRead("http://studentnet.cst.beds.ac.uk/~1201561/Project%20Colony/QuestTypeData.txt");
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
			Debug.Log("Start Quest Resource Data");

			// Set the number of quests
			iNoOfQuests = int.Parse (reader.ReadLine());
			aQuests = new QuestData[iNoOfQuests];

			for (int i = 0; i < iNoOfQuests; i++)
			{
				string dataTxt = reader.ReadLine();
				string[] questTxt = dataTxt.Split('|');

				string[] basicDataTxt = questTxt[0].Split (',');

				int id = int.Parse(basicDataTxt[0]);
				string name = basicDataTxt[1];
				int noOfConditions = int.Parse(basicDataTxt[2]);

				string[] conditionTxt = questTxt[1].Split('/');
				ConditionData[] conditionArray = new ConditionData[conditionTxt.Length];

				for (int j = 0; j < conditionTxt.Length; j++)
				{
					string[] conditionDataTxt = conditionTxt[j].Split(',');

					conditionArray[j].sName = conditionDataTxt[0];
					conditionArray[j].sConditionType = conditionDataTxt[1];
					conditionArray[j].iTrackID = int.Parse (conditionDataTxt[2]);
					conditionArray[j].iNumberRequired = int.Parse(conditionDataTxt[3]);
				}

				string[] rewardTxt = questTxt[2].Split(',');

				int gold = int.Parse(rewardTxt[0]);
				int xp = int.Parse(rewardTxt[1]);

				aQuests[i].SetValues(id, name, noOfConditions, conditionArray, gold, xp);
			}

			Debug.Log("Quest Data Loaded");
		}

		reader.Close();

		if (DataReader.bOnlineLoad)
		{
			stream.Close ();
		}
	}
}
