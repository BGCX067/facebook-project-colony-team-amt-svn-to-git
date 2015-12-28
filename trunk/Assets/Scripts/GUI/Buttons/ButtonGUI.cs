using UnityEngine;
using System.Collections;

public class ButtonGUI : MonoBehaviour 
{
	// Position between 0-1 in both X and Y
	public Vector2 v2Position;
	// Size between 0-1 in both X and Y
	public Vector2 v2Size;

	// Area of the screen that the button will take up
	Rect rButtonArea;

	// Button Texture
	public Texture2D t2Texture;

	// Button Text
	public string sText = "";
	// Tooltip Text
	public string sTooltipText = "";

	// Tooltip Offset from the Mouse
	public static Vector2 v2TooltipOffset = new Vector2(15, 15);
	// Margin between the Tooltip Text and Box Edges
	public static int fTooltipMargin = 4;

	// Base Screen Size
	public static Vector2 v2BaseScreenSize = new Vector2(800, 600);

	// GUI Skin
	public GUISkin GUIButtonSkin = null;

	// Active flag
	public static bool bActive = true;

	// Size Ration
	float fSizeRatio;
	// Size Multiplier
	float fSizeMultiplier;

	// AudioClip used when clicking
	public AudioClip ClickButtonSound;

	// Initialization
	void Start()
	{

	}

	// Resizes GUI
	void ResizeGUI()
	{
		// Draws and resizes GUI every frame
		fSizeRatio = Screen.height / v2BaseScreenSize.y;
		fSizeMultiplier = v2BaseScreenSize.x * fSizeRatio;
		
		// Setup the button area based on the screen size
		rButtonArea = new Rect(v2Position.x * Screen.width,
		                       v2Position.y * Screen.height,
		                       v2Size.x * Screen.width,
		                       v2Size.y * Screen.height);
	}

	// GUI
	void OnGUI()
	{
		// Sets skin
		GUI.skin = GUIButtonSkin;

		// Calls resize function
		ResizeGUI();

		// If there is a button texture
		if (t2Texture)
		{
			// Create a button
			if (GUI.Button(rButtonArea, new GUIContent(t2Texture, sTooltipText)))
			{
				if (bActive)
				{
					// Button Effect
					ButtonEffect();
				}
			}
		}
		else // Otherwise
		{
			// Create a button
			if (GUI.Button(rButtonArea, new GUIContent(sText, sTooltipText)))
			{
				if (bActive)
				{
					// Button Effect
					ButtonEffect();
				}
			}
		} 

		if (bActive)
		{
			// If there is a tooltip given from the button then create the tooltip background box
			if (GUI.tooltip != "")
			{
				GUI.Box (rGetTooltipRect(), new Texture());
			}

			// Create the Tooltip Label
			CreateTooltip();

			// Checks if muted
			if (SoundManager.bMute == false)
			{
				// Plays sound clip
				audio.volume = SoundManager.fVolume;
				audio.clip = ClickButtonSound;
				audio.Play();
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

		if (tooltipPosition.x > (Screen.width * 0.8))
		{
			tooltipPosition.x -= textSize.x + v2TooltipOffset.x* 2;
		}

		// Tooltip Area
		Rect tooltipRect = new Rect(tooltipPosition.x, tooltipPosition.y,
		                            textSize.x + fTooltipMargin * 2,
		                            textSize.y + fTooltipMargin * 2);

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

	// Effect of the Button when Clicked - Will be Overridden by the Child of this Object
	public virtual void ButtonEffect()
	{
		Debug.Log ("Button Click");
	}
}
