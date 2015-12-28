using UnityEngine;
using System.Collections;

public class InventoryGUIWindow : WindowGUI
{
	// Resource Icon Textures
	public Texture2D[] resourceIcons;

	// int array of SellAmount
	int[] aiSellAmount;

	// AudioClip used when clicking
	public AudioClip SellButtonSound;

	// Initialization
	void Start ()
	{
		aiSellAmount = new int[ResourceTypeData.iNoOfTypes];
	}
	
	// GUI
	void OnGUI()
	{
		// If the Inventory Window is open then draw it
		if (InventoryButtonGUI.bButtonPressed)
		{
			DrawWindow();
		}
	}
	
	// Window Function
	protected override void WindowFunction(int windowID)
	{
		// Window Area
		Vector2 windowArea = new Vector2(v2Size.x * Screen.width, v2Size.y * Screen.height);

		// Area of the window used for the Scroll View
		Rect scrollArea = new Rect (windowArea.x * 0.05f, windowArea.y * 0.25f,
		                            windowArea.x * 0.85f, windowArea.y * 0.70f);

		// Scrollable Area
		Rect scrollViewArea = new Rect (0, 0, scrollArea.width - 100, scrollArea.height * 3.5f);

		// Label GUI Style
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleCenter;

		// If the Close Button is clicked then close the window
		if (GUI.Button (new Rect(windowArea.x - 30, 5, 20, 20), "X"))
		{
			EnableControls();
			InventoryButtonGUI.bButtonPressed = false;
		}
		
		// Create the Scroll View and Update the Scroll Bar Position
		v2ScrollPosition = GUI.BeginScrollView(scrollArea, v2ScrollPosition,
		                                       scrollViewArea, false, true);

		// Space out and display each resource
		for (int i = 0; i < ResourceTypeData.iNoOfTypes; i++)
		{
			// Grid Position
			int positionX = (i % 3) * (int)windowArea.x / 3;
			int positionY = (i / 3) * 200;

			// Draw the Resource Icon
			GUI.DrawTexture (new Rect(positionX + 30, positionY + 50, 80, 80), resourceIcons[i]);

			// Draw the Resource Name
			GUI.Label (new Rect(positionX + 15, positionY + 130, 120, 25), ResourceTypeData.aResourceTypes[i].sName +
			           			" - " + InventoryManager.GetResource(i).ToString(), style);

			// Draws Sell Button on the inventory
			if (GUI.Button (new Rect (positionX, positionY + 160, 140, 30), "Sell" + " - " + ResourceTypeData.aResourceTypes[i].iValue * aiSellAmount[i]))
			{
				// Takes resources with amount specified and sell amount by value of resource
				InventoryManager.TakeResource(i, aiSellAmount[i]);
				InventoryManager.AddGold(aiSellAmount[i] * ResourceTypeData.aResourceTypes[i].iValue);
				aiSellAmount[i] = 0;

				// Checks if muted
				if (SoundManager.bMute == false)
				{
					// Plays sound clip
					audio.volume = SoundManager.fVolume;
					audio.clip = SellButtonSound;
					audio.Play();
				}
			}

			// Draws box that displays aiCraftAmount
			GUI.Box(new Rect(positionX + 40, positionY + 195, 60, 25), aiSellAmount[i].ToString());

			// Draws - button
			if(GUI.Button(new Rect (positionX, positionY + 195, 35, 25), "-"))
			{
				// Checks amount in aiSellAmount in inventory to stay on 0 and above
				if (aiSellAmount[i] > 0)
				{
					aiSellAmount[i] --;
				}
			}

			// Draws + button
			if(GUI.Button(new Rect (positionX + 105, positionY + 195, 35, 25), "+"))
			{
				// Checks amount in aiSellAmount to max in inventory
				if (aiSellAmount[i] < InventoryManager.GetResource(i))
				{
					aiSellAmount[i] ++;
				}
			}
		}
		GUI.EndScrollView();
	}
}
