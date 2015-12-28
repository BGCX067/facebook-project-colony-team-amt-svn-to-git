using UnityEngine;
using System;
using System.Collections;

public class BuildingTimer : MonoBehaviour 
{
	// Building Data Saver
	BuildingDataSaver buildingSave;
	// Inventory Data Saver
	InventoryDataSaver inventorySave;

	// Interval of time until the resource is ready
	TimeSpan resourceTimeInterval;

	// Time the resource started 
	DateTime resourceStartTime;
	// Time the resource is ready
	DateTime resourceEndTime;

	// Time Remaining until the Resource is Ready
	TimeSpan timeRemaining;

	// Resource ID
	int iResourceID;

	// Resource Ready flag
	bool bIsResourceReady = false;
	// Building Inactive
	public bool bInActive = false;

	// Initialization
	void Awake () 
	{
		buildingSave = GameObject.Find ("WorldManager").GetComponent<BuildingDataSaver>();
		inventorySave = GameObject.Find ("InventoryManager").GetComponent<InventoryDataSaver>();
	}

	// Set Resource ID
	public void SetResourceID(int id)
	{
		iResourceID = id;
	}

	// Get Time Remaining
	public TimeSpan GetTimeRemaining()
	{
		return timeRemaining;
	}

	// Get Resource Read flag
	public bool bGetIsResourceReady()
	{
		return bIsResourceReady;
	}

	// Set Start Time
	public void SetStartTime(DateTime startTime)
	{
		this.resourceStartTime = startTime;
		print (startTime);
	}

	// Set Time Interval (TimeSpan)
	public void SetTimeInterval(TimeSpan time)
	{
		this.resourceTimeInterval = time;

		// Set End Time for Resource
		SetEndTime();
	}

	// Set Time Interval (Days, Hours, Minutes, Seconds)
	public void SetTimeInterval(int days, int hours, int minutes, int seconds)
	{
		this.resourceTimeInterval = new TimeSpan(days, hours, minutes, seconds);

		// Set End Time for Resource
		SetEndTime();
	}

	// Set End Time
	void SetEndTime()
	{
		print ("Time: " + resourceTimeInterval);
		this.resourceEndTime = resourceStartTime + resourceTimeInterval;
		
		bIsResourceReady = false;
	}

	public void FinishTimerEarly()
	{
		resourceEndTime = DateTime.Now;
	}

	public void ActivateTimer()
	{
		// Set new start and end times
		SetStartTime(DateTime.Now);
		SetEndTime();

		bInActive = false;

		buildingSave.SaveData ();
	}

	// Update is called once per frame
	void Update () 
	{
		CheckResourceTime();
		UpdateTimeRemainingTimer();
	}

	// Check current time against the end time to see if resource is ready
	void CheckResourceTime()
	{
		if (!bInActive && !bIsResourceReady)
		{
			if (DateTime.Now > resourceEndTime)
			{
				bIsResourceReady = true;
			}
		}
	}

	// Update Time Remaining Timer
	void UpdateTimeRemainingTimer()
	{
		if (!bIsResourceReady)
		{
			timeRemaining = resourceEndTime - DateTime.Now;
		}
	}
	
	// Mouse Down event
	void OnMouseDown()
	{
		// If not in sell mode, then give a resource
		if (!MoveButtonGUI.bButtonPressed)
		{
			GiveResource();
		}
	}

	// Gives the resource if one is ready
	void GiveResource()
	{
		if (bIsResourceReady)
		{
			// Add resource and xp
			InventoryManager.AddResource(iResourceID, 1);
			LevelManager.iAddXP(ResourceTypeData.aResourceTypes[iResourceID].iXP);

			QuestManager.CheckQuestCondition("Resource", iResourceID);

			bInActive = true;
			bIsResourceReady = false;

			// Save building and inventory data
			buildingSave.SaveData();
			inventorySave.SaveData();

			Debug.Log("--- Resource Given");
			Debug.Log ("Resources: " + InventoryManager.GetResource(iResourceID).ToString());
		}
	}

	// Get Timer Save Data
	public string sGetTimerSaveData()
	{
		string dataString = resourceStartTime.ToString() + ","
			+ bIsResourceReady.ToString() + ","
			+ bInActive.ToString();

		dataString = dataString.Replace(" ", "-");

		return dataString;
	}
}


