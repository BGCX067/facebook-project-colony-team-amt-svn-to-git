using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Quest Progress Data - Stores Quest ID, Quest Complete flag, 
// 						Condition Complete flag array and Condition Progress array
public struct QuestProgressData
{
	public int iID;
	public bool bQuestComplete;
	public bool[] abConditionsComplete;
	public int[] aiConditionsProgress;

	public int iQuestProgress;

	// Set Initial Values
	public void SetValues(int id, bool questComplete, 
	                      bool[] conditionsComplete, int[] conditionsProgress)
	{
		iID = id;
		bQuestComplete = questComplete;
		abConditionsComplete = conditionsComplete;
		aiConditionsProgress = conditionsProgress;
	}

	// Retrieve quest save data string
	public string sGetQuestSaveString()
	{
		string questString = "";

		questString += iID.ToString () + ",";
		questString += bQuestComplete.ToString () + ",";

		for (int i = 0; i < aiConditionsProgress.Length; i++) 
		{
			questString += abConditionsComplete[i].ToString() + "#"
				+ aiConditionsProgress[i].ToString();

			if (i < aiConditionsProgress.Length - 1)
			{
				questString += "/";
			}
		}

		return questString;
	}
}

public class QuestManager : MonoBehaviour 
{
	// Quest Progress array
	public static QuestProgressData[] aQuestProgress;
	// Active Quests list
	public static List<int> liActiveQuests;

	static QuestDataSaver questSave;

	// Use this for initialization
	void Start () 
	{
		questSave = gameObject.GetComponent<QuestDataSaver>();
	}

	// Initial values
	public static void Init(QuestProgressData[] questProgress, List<int> activeQuests)
	{
		aQuestProgress = questProgress;
		liActiveQuests = activeQuests;

		for (int i = 0; i < aQuestProgress.Length; i++)
		{
			for (int j = 0; j < activeQuests.Count; j++)
			{
				if (i == activeQuests[j])
				{
					QuestManager.CalculateQuestProgress(i);
					continue;
				}
			}
		}
	}

	// Update
	void Update()
	{
		ManageActiveQuests();
		// Check Condition Progress
		CheckConditionProgress();
		// Check Quest Progress
		CheckQuestProgress();
	}

	void ManageActiveQuests()
	{
		if (liActiveQuests.Count < 3)
		{
			for (int i = 0; i < QuestTypeData.iNoOfQuests; i++)
			{
				bool alreadyActive = false;

				if (liActiveQuests.Count > 0)
				{
					for (int j = 0; j < liActiveQuests.Count; j++)
					{
						if (i == liActiveQuests[j])
						{
							alreadyActive = true;
							break;
						}
					}
				}

				if (!alreadyActive)
				{
					if (!aQuestProgress[i].bQuestComplete)
					{
						print ("Add Quest: " + i.ToString());
						liActiveQuests.Add(i);
						break;	
					}
				}
			}
		}
	}

	public static void CheckQuestCondition(string conditionType, int id)
	{
		for (int i = 0; i < liActiveQuests.Count; i++)
		{
			for (int j = 0; j < QuestTypeData.aQuests[liActiveQuests[i]].iNoOfConditions; j++)
			{
				if (QuestTypeData.aQuests[liActiveQuests[i]].aConditionList[j].sConditionType == conditionType)
				{
					if (liActiveQuests[i] == id)
					{
						aQuestProgress[liActiveQuests[i]].aiConditionsProgress[j]++;
						questSave.SaveData();

						break;
					}
				}
			}
		}
	}

	// Check Condition Progress
	void CheckConditionProgress()
	{
		// If there are active quests
		if (liActiveQuests.Count > 0)
		{
			// Loop through the active quests
			for (int i = 0; i < liActiveQuests.Count; i++)
			{
				QuestProgressData currentQuest = aQuestProgress[liActiveQuests[i]];

				print (currentQuest.abConditionsComplete.Length);

				// Loop through each condition
				for (int j = 0; j < currentQuest.aiConditionsProgress.Length; j++)
				{
					// If the condition hasn't been fulfilled 
					if (!currentQuest.abConditionsComplete[j])
					{
						// If the condition progress is higher than the required amount, 
						// then set the condition complete to true
						if (currentQuest.aiConditionsProgress[j] >= 
						    QuestTypeData.aQuests[liActiveQuests[i]].aConditionList[j].iNumberRequired)
						{
							currentQuest.abConditionsComplete[j] = true;
						}
					}
				}
			}
		}
	}

	// Check Quest Progress
	void CheckQuestProgress()
	{
		// If there are active quests
		if (liActiveQuests.Count > 0)
		{
			print (liActiveQuests.Count);

			// Loop through each quest
			for (int i = 0; i < liActiveQuests.Count; i++)
			{
				QuestProgressData currentQuest = aQuestProgress[liActiveQuests[i]];

				// Number of complete conditions
				int conditionsComplete = 0;

				// Loop through each condition
				for (int j = 0; j < currentQuest.abConditionsComplete.Length; j++)
				{
					// If the condition is complete then increment conditionsComplete
					if (currentQuest.abConditionsComplete[j])
					{
						conditionsComplete++;
					}
				}

				// If the number of complete conditions matches the number of conditions 
				// then set the quest to complete and remove it from the active quests list
				if (conditionsComplete == QuestTypeData.aQuests[currentQuest.iID].iNoOfConditions)
				{
					aQuestProgress[liActiveQuests[i]].bQuestComplete = true;

					InventoryManager.AddGold(QuestTypeData.aQuests[currentQuest.iID].iGoldReward);
					LevelManager.iAddXP(QuestTypeData.aQuests[currentQuest.iID].iXPReward);

					liActiveQuests.RemoveAt(i);

					QuestCompleteWindow.iQuestID = liActiveQuests[i];
					QuestCompleteWindow.bWindowActive = true;

					questSave.SaveData();
				}
			}
		}
	}

	public static void CalculateQuestProgress(int questID)
	{
		int noOfConditions = QuestTypeData.aQuests[questID].iNoOfConditions;
		int completeConditions = 0;
		float questPercentage = 0;

		for (int i = 0; i < noOfConditions; i++)
		{
			questPercentage += ((
				(float)QuestManager.aQuestProgress[questID].aiConditionsProgress[i] / 
				QuestTypeData.aQuests[questID].aConditionList[i].iNumberRequired) * 100) / noOfConditions;
		}

		QuestManager.aQuestProgress[questID].iQuestProgress = Mathf.RoundToInt(questPercentage);
	}
}
