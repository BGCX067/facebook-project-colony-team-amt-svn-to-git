using UnityEngine;
using System.Collections;

public class ActiveQuestGUI : MonoBehaviour 
{
	// Position between 0-1 in both X and Y
	public Vector2 v2Position;
	// Size between 0-1 in both X and Y for each Quest Icon
	public Vector2 v2Size;

	// Window Rect
	Rect rButtonRect;

	public float fVertSpacing = 0.1f;

	public Vector2 v2TooltipOffset;
	// Distance from the Edge of the Textboxes to the Text
	public static int iTooltipMargin = 4;

	string sTooltipText;

	public static bool bButtonPressed = false;
	public static bool bButtonsActive = true;

	// Use this for initialization
	void Start () 
	{
	}

	void OnGUI()
	{
		bool hoveredOver = false;

		if (QuestManager.liActiveQuests.Count > 0)
		{
			for (int i = 0; i < QuestManager.liActiveQuests.Count; i++)
			{
				sTooltipText = QuestTypeData.aQuests[QuestManager.liActiveQuests[i]].sName + "\n"
					+ QuestManager.aQuestProgress[i].iQuestProgress.ToString() + "% Complete";

				// Draws and resizes GUI every frame
				rButtonRect = new Rect(v2Position.x * Screen.width,
				                       (v2Position.y + (fVertSpacing * i)) * Screen.height, 
				                       v2Size.x * Screen.width,
				                       v2Size.y * Screen.width);

				if (GUI.Button (rButtonRect, new GUIContent((i + 1).ToString(), sTooltipText)))
				{
					if (bButtonsActive)
					{
						bButtonPressed = !bButtonPressed;
						WindowGUI.DisableControls();

						QuestProgressWindow.SetQuestValues(i);
					}
				}

				if (bButtonsActive)
				{
					if (!hoveredOver)
					{
						// If there is a tooltip given from the button then create the tooltip background box
						if (GUI.tooltip != "")
						{
							GUI.Box (rGetTooltipRect(), new Texture());
							hoveredOver = true;
						}
						
						// Create the Tooltip Label
						CreateTooltip();
					}
				}
			}
		}
	}

	// Setup the Tooltip Area and Return it
	Rect rGetTooltipRect()
	{
		// Tooltip Position - Mouse Position + Tooltip Offset
		Vector2 tooltipPosition = new Vector2(Input.mousePosition.x, -Input.mousePosition.y + Screen.height)
			+ v2TooltipOffset;
		
		// Size of the Tooltip Text
		Vector2 textSize = GUI.skin.GetStyle("label").CalcSize(new GUIContent(sTooltipText));
		
		// Tooltip Area
		Rect tooltipRect = new Rect(tooltipPosition.x, tooltipPosition.y,
		                            textSize.x + iTooltipMargin * 2,
		                            textSize.y + iTooltipMargin * 2);
		
		// Return the Tooltip Rect
		return tooltipRect;
	}
	
	// Creates the Tooltip Label and Centers the Text
	void CreateTooltip()
	{
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleCenter;
		style.fontSize = 12;
		
		GUI.Label(rGetTooltipRect(), GUI.tooltip, style);
	}
}
