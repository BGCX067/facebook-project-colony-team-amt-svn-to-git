using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestDataReader : DataReader
{
	// Use this for initialization
	void Start () 
	{
		DecodeData();
	}

	// Load Data
	void DecodeData()
	{
		// Quest array
		QuestProgressData[] quests = new QuestProgressData[QuestTypeData.iNoOfQuests];
		// Active Quest list
		List<int> activeQuests = new List<int>();

		// Read data
		string dataTxt = sLoadData("QuestData", sFileName);

		dataTxt = dataTxt.Replace (" ", "");

		// Check data to see if empty
		if (dataTxt == "" || dataTxt.Contains("NewUser") || dataTxt == null)
		{
			Debug.Log ("No User Quest Data");

			// Create empty quest data for each quest in the game
			for (int i = 0; i < QuestTypeData.iNoOfQuests; i++)
			{
				bool[] conditionsComplete = new bool[QuestTypeData.aQuests[i].iNoOfConditions];
				int[] conditionsProgress = new int[QuestTypeData.aQuests[i].iNoOfConditions];

				for (int j = 0; j < QuestTypeData.aQuests[i].iNoOfConditions; j++)
				{
					conditionsComplete[j] = false;
					conditionsProgress[j] = 0;
				}

				quests[i].SetValues(i, false, conditionsComplete, conditionsProgress);
			}
		}
		else
		{
			// Split the data
			string[] questTxt = dataTxt.Split('|');

			// Active Quest data
			// If there are active quests then retrieve the active quest ids
			if (questTxt[0] != "")
			{
				string[] activeQuestData = questTxt[0].Split(',');

				for (int i = 0; i < activeQuestData.Length; i++)
				{
					print ("Active Quest: " + activeQuestData[i]);
					int questID = int.Parse (activeQuestData[i]);

					activeQuests.Add(questID);
				}
			}

			// Loop through each quest and retrieve the progress data for each
			for (int j = 1; j < questTxt.Length; j++)
			{
				string[] questData = questTxt[j].Split(',');

				int id = int.Parse (questData[0]);
				bool achieved = bool.Parse(questData[1]);

				string[] conditionTxt = questData[2].Split('/');

				bool[] conditionsComplete = new bool[conditionTxt.Length];
				int[] conditionsProgress = new int[conditionTxt.Length];

				for (int k = 0; k < conditionTxt.Length; k++)
				{
					string[] conditionData = conditionTxt[k].Split('#');

					conditionsComplete[k] = bool.Parse(conditionData[0]);
					conditionsProgress[k] = int.Parse (conditionData[1]);
				}

				// Add the data for each quest 
				quests[j - 1].SetValues(id, achieved, conditionsComplete, conditionsProgress);
			}
		}

		// Initialise the quest data
		QuestManager.Init(quests, activeQuests);
	}
}
