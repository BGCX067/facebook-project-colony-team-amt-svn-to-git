using UnityEngine;
using System.Collections;
using System.IO;

public class QuestDataSaver : MonoBehaviour 
{
	// File path
	string sFilePath;
	// File name + extension
	public string sFileName;
	
	// Link to the Database
	string sWorldLinkString = "http://studentnet.cst.beds.ac.uk/~1201561/Project%20Colony/SubmitGameData.php?UserID=";

	// Use this for initialization
	void Start () 
	{
	}

	public void SaveData()
	{
		if (DataReader.bOnlineSave)
		{
			StartCoroutine(SaveOnlineData());
		}
		else
		{
			sFilePath = Application.dataPath + "/Data/";
			SaveOfflineData();
		}
	}

	// Offline save
	public void SaveOfflineData()
	{
		// Data string
		string sTextLinkString = sFilePath + sFileName;
		
		// Quest Data
		string questData = "";
		
		// Append the data into the string
		// Quest Data

		// Loop through each active quest 
		for (int i = 0; i < QuestManager.liActiveQuests.Count; i++) 
		{
			questData += QuestManager.liActiveQuests[i].ToString();

			if (i < QuestManager.liActiveQuests.Count - 1)
			{
				questData += ",";
			}
		}
	
		questData += "|";

		// Loop through each quest 
		for (int j = 0; j < QuestTypeData.iNoOfQuests; j++) 
		{
			string data = QuestManager.aQuestProgress[j].sGetQuestSaveString();

			questData += data;

			if (j < QuestTypeData.iNoOfQuests - 1)
			{
				questData += "|";
			}
		}

		// Text Writer
		StreamWriter writer = new StreamWriter(sTextLinkString, false);
		// Overwrite map data
		writer.WriteLine(questData);
		
		// Close the writer
		writer.Close ();
		
		Debug.Log("Level Save Complete");
	}

	// Online save
	IEnumerator SaveOnlineData()
	{
		// Data string
		string sTextLinkString = sFilePath + sFileName;
		
		// Quest Data
		string questData = "";
		
		// Append the data into the string
		// Quest Data

		// Loop through each active quest
		for (int i = 0; i < QuestManager.liActiveQuests.Count; i++) 
		{
			questData += QuestManager.liActiveQuests[i].ToString();
			
			if (i < QuestManager.liActiveQuests.Count - 1)
			{
				questData += ",";
			}
		}
		
		questData += "|";

		// Loop through each quest
		for (int j = 0; j < QuestTypeData.iNoOfQuests; j++) 
		{
			string data = QuestManager.aQuestProgress[j].sGetQuestSaveString();
			
			questData += data;
			
			if (j < QuestTypeData.iNoOfQuests - 1)
			{
				questData += "|";
			}
		}

		Debug.Log (questData);
		
		Debug.Log ("Start Quest Saving");
		
		// Send the data to the server
		WWW webRequest = new WWW (sWorldLinkString + FBManager.iFacebookID.ToString () + "&Column=QuestData&Data=" + questData);
		yield return webRequest;
		
		Debug.Log (webRequest.text);
		
		Debug.Log("Quest Save Complete");
	}
}