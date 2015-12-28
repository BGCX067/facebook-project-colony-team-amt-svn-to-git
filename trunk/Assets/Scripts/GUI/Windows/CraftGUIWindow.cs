using UnityEngine;
using System.Collections;

public class CraftGUIWindow : WindowGUI
{
	// Resource Icon Textures
	public Texture2D[] resourceIcons;

	// Int array of CraftAmount
	int[] aiCraftAmount;

	// Second scroll bar vector2
	Vector2 v2ScrollPosition2;

	int iResourceSelect;

	// AudioClip used when crafting
	public AudioClip CraftButtonSound;

	// Initialization
	void Start ()
	{
		aiCraftAmount = new int[ResourceTypeData.iNoOfTypes];
	}
	
	// GUI
	void OnGUI()
	{
		// If the Craft Window is open then draw it
		if (CraftButtonGUI.bButtonPressed)
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
		                            windowArea.x * 0.6f, windowArea.y * 0.6f);
		
		// Scrollable Area
		Rect scrollViewArea = new Rect (0, 0, scrollArea.width - 100, scrollArea.height * 2);
		
		// Label GUI Style
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleCenter;
		
		// If the Close Button is clicked then close the window
		if (GUI.Button (new Rect(windowArea.x - 30, 5, 20, 20), "X"))
		{
			EnableControls();
			CraftButtonGUI.bButtonPressed = false;
		}
		
		// Create the Scroll View and Update the Scroll Bar Position
		v2ScrollPosition = GUI.BeginScrollView(scrollArea, v2ScrollPosition,
		                                       scrollViewArea, false, true);

		// Space out and display each resource
		for (int i = 0; i < CraftResourceTypeData.iNoOfCraftTypes; i++)
		{
			// Grid Position
			int positionX = (i % 3) * (int)windowArea.x / 5;
			int positionY = (i / 3) * 200;

			Rect resourceRect = new Rect(positionX + 30, positionY + 50, 80, 80);

			// Draw the Resource Icon
			GUI.DrawTexture (resourceRect, resourceIcons[i]);

			// Changes selection based on texture
			if (resourceRect.Contains(Event.current.mousePosition))
			{
				print (i);
				iResourceSelect = i;
			}

			// Draw the Resource Name
			GUI.Label (new Rect(positionX + 15, positionY + 130, 120, 25), CraftResourceTypeData.aCraftResourceTypes[i].sName +
			           			" - " + InventoryManager.GetResource(i + 6).ToString(), style);

			// Draws Craft Button on the inventory
			if (GUI.Button (new Rect (positionX, positionY + 160, 140, 30), "Craft"))
			{
				// Takes resources with amount specified, craft and gain XP amount by value of resource
				InventoryManager.TakeResource(CraftResourceTypeData.aCraftResourceTypes[i].iResource1ID, aiCraftAmount[i] * CraftResourceTypeData.aCraftResourceTypes[i].iResource1Cost);
				InventoryManager.TakeResource(CraftResourceTypeData.aCraftResourceTypes[i].iResource2ID, aiCraftAmount[i] * CraftResourceTypeData.aCraftResourceTypes[i].iResource2Cost);
				InventoryManager.AddResource(i + 6,aiCraftAmount[i]);
				LevelManager.iAddXP(CraftResourceTypeData.aCraftResourceTypes[i].iXP * aiCraftAmount[i]);
				aiCraftAmount[i] = 0;

				// Checks if muted
				if (SoundManager.bMute == false)
				{
					// Plays sound clip
					audio.volume = SoundManager.fVolume;
					audio.clip = CraftButtonSound;
					audio.Play();
				}
			}
			
			// Draws box that displays aiCraftAmount
			GUI.Box(new Rect(positionX + 40, positionY + 195, 60, 25), aiCraftAmount[i].ToString());
			
			// Draws - button
			if(GUI.Button(new Rect (positionX, positionY + 195, 35, 25), "-"))
			{
				if (aiCraftAmount[i] > 0)
				{
					aiCraftAmount[i] --;
				}
			}
			
			// Draws + button
			if(GUI.Button(new Rect (positionX + 105, positionY + 195, 35, 25), "+"))
			{
				// Checks amount of increment to how much resources available to craft
				if (aiCraftAmount[i] < InventoryManager.GetResource(CraftResourceTypeData.aCraftResourceTypes[i].iResource1ID) / CraftResourceTypeData.aCraftResourceTypes[i].iResource1Cost)
				{
					if (aiCraftAmount[i] < InventoryManager.GetResource(CraftResourceTypeData.aCraftResourceTypes[i].iResource2ID) / CraftResourceTypeData.aCraftResourceTypes[i].iResource2Cost)
					{
						aiCraftAmount[i] ++;
					}
				}
			}
		}
		GUI.EndScrollView();

		// Craft resource description window
		// Area of the window used for the Scroll View
		Rect scrollArea2 = new Rect (windowArea.x * 0.66f, windowArea.y * 0.25f,
		                             windowArea.x * 0.3f, Screen.height / 5.6f);
		
		// Scrollable Area
		Rect scrollViewArea2 = new Rect (0, 0, scrollArea2.width - 100, scrollArea2.height * 2);
		
		// Create the Scroll View and Update the Scroll Bar Position
		v2ScrollPosition2 = GUI.BeginScrollView(scrollArea2, v2ScrollPosition2,
		                                        scrollViewArea2, false, true);

		// Space out and display each resource
		for (int e = 0; e < 1; e++)
		{
			// Grid Position
			int positionX = (e % 3) * (int)windowArea.x / 8;
			int positionY = (e / 3) * 200;
			
			// Draw the Resource Icon
			GUI.DrawTexture (new Rect(positionX + (scrollArea2.x * 0.2f),
			                          positionY + (scrollArea2.y * 0.28f),
			                          0.1f * Screen.height, 0.1f * Screen.height), resourceIcons[iResourceSelect]);

			// Draw the Resource Name
			GUI.Label (new Rect(positionX + (scrollArea2.x * 0.12f),
			                    positionY,
			                    150,
			                    25), CraftResourceTypeData.aCraftResourceTypes[iResourceSelect].sName, style);

			// Draw label for resource text description
			GUI.Label (new Rect(positionX,
			                    positionY + (scrollArea2.y * 0.5f),
			                    scrollArea2.x * 0.4f,
			                    200), CraftResourceTypeData.aCraftResourceTypes[iResourceSelect].sResourceText, style);
		}
		GUI.EndScrollView();
	}
}