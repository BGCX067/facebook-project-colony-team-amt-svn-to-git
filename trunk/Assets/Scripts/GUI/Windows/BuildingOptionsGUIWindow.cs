using UnityEngine;
using System;
using System.Collections;

public class BuildingOptionsGUIWindow : WindowGUI 
{
	// Building ID
	int iBuildingID;

	// Window Activated flag
	public bool bWindowActivated = false;

	// Building this window is linked to
	Building linkedBuilding;
	// Timer for the linked building
	BuildingTimer linkedBuildingTimer;

	// Initialization
	void Start () 
	{
	}

	// Update
	void Update()
	{
		// Check Other GUI Windows
		CheckOtherWindows();
	}

	// Checks for Other GUI Windows and Closes This Window If Another Open Window is Found
	void CheckOtherWindows()
	{
		// If this window is open and another GUI Window is open then deactivate this window
		if (bWindowActivated)
		{
			if (BuildButtonGUI.bButtonPressed ||
			    InventoryButtonGUI.bButtonPressed)
			{
				DeactivateWindow();
			}
		}
	}

	// Activates This Window
	public void ActivateWindow(Building building)
	{
		// Set the linked building and its timer
		linkedBuilding = building;
		linkedBuildingTimer = linkedBuilding.GetBuildingTimer();

		// Get the on-screen building position and place the window above it 
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(linkedBuilding.v3GetWorldPosition());
		SetPosition(screenPosition.x, screenPosition.y + 80);

		iBuildingID = linkedBuilding.iGetObjectID ();

		// Set Window Activated to true
		bWindowActivated = true;
	}

	// Deactivate This Window
	void DeactivateWindow()
	{
		bWindowActivated = false;
	}

	// GUI
	void OnGUI()
	{
		// If this window is activated then draw the window
		if (bWindowActivated)
		{
			DrawWindow();
		}
	}

	// Window Function
	protected override void WindowFunction(int windowID)
	{
		// Set the GUI Label Style to Middle-Center Alignment
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleCenter;

		// Window Area
		Vector2 windowArea = new Vector2(v2Size.x * Screen.width, v2Size.y * Screen.height);

		// Resource Status
		string resourceStatus = "Ready in: ";

		sWindowTitle = BuildingTypeData.aBuildingTypes[iBuildingID].sName;

		// If the close button is clicked then close the window
		if (GUI.Button (new Rect(windowArea.x - 30, 5, 20, 20), "X"))
		{
			DeactivateWindow();
		}

		// If the resource is ready
		if (linkedBuildingTimer.bGetIsResourceReady())
		{
			resourceStatus = "Resource Ready!";
		}
		else if (linkedBuildingTimer.bInActive)
		{
			int payAmount = Mathf.FloorToInt(BuildingTypeData.aBuildingTypes[iBuildingID].iCost * 0.25f);
			resourceStatus = "Building In-Active \n Cost: " + payAmount + " Gold";

			// Create the Activate Button
			if (GUI.Button (new Rect( windowArea.x * 0.2f, 70, windowArea.x * 0.6f, 30), "Activate"))
			{
				if (InventoryManager.TakeGold(payAmount))
				{
					linkedBuildingTimer.ActivateTimer();
				}
			}
		}
		else // Otherwise 
		{
			// Find the time remaining and set the resource status
			resourceStatus = sConvertTimeToString(resourceStatus);

			// Create the Speed-Mine Button
			if (GUI.Button (new Rect( windowArea.x * 0.2f, 70, windowArea.x * 0.6f, 30), "Speed-Mine"))
			{
				if (InventoryManager.TakeCredits(1))
				{
					linkedBuildingTimer.FinishTimerEarly();
				}
			}
		}

		// Draw the Resource Status Label
		GUI.Label(new Rect(windowArea.x * 0.1f, 20, windowArea.x * 0.8f, 40), resourceStatus, style);
	}

	// Convert TimeSpan to string and add it to the input string
	string sConvertTimeToString(string input)
	{
		// Get the Time Remaining
		TimeSpan timeRemaining = linkedBuildingTimer.GetTimeRemaining();

		// If the number of days is above 0, add the days string
		if (timeRemaining.Days > 0)
		{
			input += timeRemaining.Days.ToString() + ".";
		}

		// If the number of hours is above 0, add the hours string
		if (timeRemaining.Hours > 0)
		{
			input += timeRemaining.Hours.ToString() + ":";
		}

		// If the number of minutes is above 0, add the minutes string
		if (timeRemaining.Minutes > 0)
		{
			// If the number of hours is above 0 and the number of minutes is less than 10
			// then add a second minute digit
			if (timeRemaining.Hours > 0 && timeRemaining.Minutes < 10)
			{
				input += "0";
			}
			
			input += timeRemaining.Minutes.ToString() + ":";
		}

		// If the number of seconds is above 0
		if (timeRemaining.Seconds < 10)
		{
			// If the number of hours and minutes are above 0 then add a second seconds digit
			if (timeRemaining.Hours > 0 || timeRemaining.Minutes > 0)
			{
				input += "0";
			}
		}

		// Add the seconds string
		input += timeRemaining.Seconds.ToString();

		// If the time remaining is less than a minute then add 'seconds' to the end of the string
		if (timeRemaining.Hours == 0 && timeRemaining.Minutes == 0)
		{
			input += " seconds";
		}

		// Return the string
		return input;
	}
}

