using UnityEngine;
using System.Collections;

public class QuestProgressWindow : WindowGUI 
{
	static int iQuestID;
	static string sQuestName;

	static int iQuestProgress;
	static int iNoOfConditions;

	static string[] asConditionNameList;
	static int[] aiConditionRequiredList;
	static int[] aiConditionProgressList;

	static int iGold;
	static int iXP;

	// Use this for initialization
	void Start () 
	{
	
	}

	public static void SetQuestValues(int questID)
	{
		iQuestID = questID;
		sQuestName = QuestTypeData.aQuests[questID].sName;

		iQuestProgress = QuestManager.aQuestProgress[questID].iQuestProgress;
		iNoOfConditions = QuestTypeData.aQuests[questID].iNoOfConditions;

		asConditionNameList = new string[iNoOfConditions];
		aiConditionRequiredList = new int[iNoOfConditions];
		aiConditionProgressList = new int[iNoOfConditions];

		for (int i = 0; i < iNoOfConditions; i++)
		{
			asConditionNameList[i] = QuestTypeData.aQuests[questID].aConditionList[i].sName;
			aiConditionRequiredList[i] = QuestTypeData.aQuests[questID].aConditionList[i].iNumberRequired;
			aiConditionProgressList[i] = QuestManager.aQuestProgress[questID].aiConditionsProgress[i];
		}

		iGold = QuestTypeData.aQuests[questID].iGoldReward;
		iXP = QuestTypeData.aQuests[questID].iXPReward;
	}

	void OnGUI()
	{
		if (ActiveQuestGUI.bButtonPressed)
		{
			DrawWindow();
		}
	}

	protected override void WindowFunction(int windowID)
	{
		// Window Area
		Vector2 windowArea = new Vector2(v2Size.x * Screen.width, v2Size.y * Screen.height);

		GUIStyle style = new GUIStyle();
		style.fontSize = 18;
		style.alignment = TextAnchor.MiddleCenter;

		style.normal.textColor = Color.white;

		// If the Close Button is clicked then close the window
		if (GUI.Button (new Rect(windowArea.x - 30, 5, 20, 20), "X"))
		{
			EnableControls();
			ActiveQuestGUI.bButtonPressed = false;
		}

		GUI.Label (new Rect(windowArea.x * 0.4f, windowArea.y * 0.1f, windowArea.x * 0.2f, windowArea.y * 0.15f),
		           sQuestName + "\n" + iQuestProgress.ToString() + "% Progress", style); 

		for (int i = 0; i < iNoOfConditions; i++)
		{
			GUI.Label(new Rect(windowArea.x * 0.15f, windowArea.y * (0.325f + (i * 0.15f)), windowArea.x * 0.2f, windowArea.y * 0.1f),
			         asConditionNameList[i], style);

			if (QuestManager.aQuestProgress[iQuestID].abConditionsComplete[i])
			{
				GUI.Label(new Rect(windowArea.x * 0.65f, windowArea.y * (0.325f + (i * 0.15f)), windowArea.x * 0.2f, windowArea.y * 0.1f),
				          "Complete", style);
			}
			else
			{
				GUI.Label(new Rect(windowArea.x * 0.65f, windowArea.y * (0.325f + (i * 0.15f)), windowArea.x * 0.2f, windowArea.y * 0.1f),
			    	      aiConditionProgressList[i].ToString() + " / " + aiConditionRequiredList[i].ToString(), style);
			}
		}

		GUI.Label (new Rect(windowArea.x * 0.3f, windowArea.y * 0.825f, windowArea.x * 0.4f, windowArea.y * 0.1f), 
		           "Reward:       " + iGold.ToString() + " Gold       " + iXP.ToString() + " XP", style);
	}
}
