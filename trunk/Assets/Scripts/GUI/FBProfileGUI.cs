using UnityEngine;
using System.Collections;

public class FBProfileGUI : MonoBehaviour 
{
	// Image Position between 0-1 in both X and Y
	public Vector2 v2ImagePosition;
	// Image size 
	float fImageSize;

	// Image Position between 0-1 in both X and Y
	public Vector2 v2LevelTextPosition;
	// Image Size between 0-1 in both X and Y
	public Vector2 v2LevelTextSize;

	// Image Position between 0-1 in both X and Y
	public Vector2 v2XPTextPosition;
	// Image Size between 0-1 in both X and Y
	public Vector2 v2XPTextSize;

	// Image Position between 0-1 in both X and Y
	public Vector2 v2NameTextPosition;
	// Image Size between 0-1 in both X and Y
	public Vector2 v2NameTextSize;

	// Distance from the Edge of the Textboxes to the Text
	public static int iTooltipMargin = 4;

	Texture2D t2ProfileImage;

	// Textbox text
	string sLevelText;
	string sXPText;
	string sNameText;

	string sXPTooltipText;
	// Tooltip Offset from the Mouse
	public Vector2 v2TooltipOffset = new Vector2(15, -15);

	// Image Rect
	Rect rProfileImageRect;

	// Text Background Rects
	Rect rLevelTextRect;
	Rect rXPTextRect;
	Rect rNameTextRect;

	// Use this for initialization
	void Start () 
	{
		sNameText = FBManager.sProfileName;
		t2ProfileImage = FBManager.t2ProfileImage;
	}

	// Draws and resizes GUI every frame
	void ResizeGUI()
	{
		// Text Background Rects
		rLevelTextRect = new Rect(v2LevelTextPosition.x * Screen.width,
		                          v2LevelTextPosition.y * Screen.height,
		                          v2LevelTextSize.x * Screen.width,
		                          v2LevelTextSize.y * Screen.height);
		
		rXPTextRect = new Rect(v2XPTextPosition.x * Screen.width,
		                       v2XPTextPosition.y * Screen.height,
		                       v2XPTextSize.x * Screen.width,
		                       v2XPTextSize.y * Screen.height);
		
		rNameTextRect = new Rect(v2NameTextPosition.x * Screen.width,
		                         v2NameTextPosition.y * Screen.height,
		                         v2NameTextSize.x * Screen.width,
		                         v2NameTextSize.y * Screen.height);
		
		// Image Size - Height of the area taken up by the 
		fImageSize = ((rNameTextRect.y + rNameTextRect.height) - rLevelTextRect.y);
		
		// Image Rect
		rProfileImageRect = new Rect(v2ImagePosition.x * Screen.width,
		                             v2ImagePosition.y * Screen.height,
		                             fImageSize,
		                             fImageSize);
	}

	// GUI
	void OnGUI()
	{
		// Calls resize function
		ResizeGUI();

		sLevelText = "Level: " + LevelManager.iGetLevel().ToString();
		sXPText = "XP: " + LevelManager.iGetXP().ToString ();

		int XPLeft = XPLevelData.aiXPLevels[LevelManager.iGetLevel() + 1] - LevelManager.iGetXP();
		sXPTooltipText = "XP to Next Level: " + XPLeft.ToString();

		// Draw FB Image
		GUI.DrawTexture(rProfileImageRect, t2ProfileImage);

		// Text Background
		GUI.Box (rLevelTextRect, new Texture());
		GUI.Box (rXPTextRect, new Texture());
		GUI.Box (rNameTextRect, new Texture());

		// Set text alignment to Middle-Left
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleCenter;
		
		// Draw Text
		GUI.Label (rLevelTextRect, sLevelText, style);
		GUI.Label (rXPTextRect, new GUIContent(sXPText, sXPTooltipText), style);
		GUI.Label (rNameTextRect, sNameText, style);

		// If there is a tooltip given from the button then create the tooltip background box
		if (GUI.tooltip != "")
		{
			GUI.Box (rGetTooltipRect(), new Texture());
		}
		
		// Create the Tooltip Label
		CreateTooltip();
	}
	
	// Setup the Tooltip Area and Return it
	Rect rGetTooltipRect()
	{
		// Tooltip Position - Mouse Position + Tooltip Offset
		Vector2 tooltipPosition = new Vector2(Input.mousePosition.x, -Input.mousePosition.y + Screen.height)
			+ v2TooltipOffset;
		
		// Size of the Tooltip Text
		Vector2 textSize = GUI.skin.GetStyle("label").CalcSize(new GUIContent(sXPTooltipText));
		
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
		
		GUI.Label(rGetTooltipRect(), GUI.tooltip, style);
	}
}