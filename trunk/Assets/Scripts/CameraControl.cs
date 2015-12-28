using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{
	// Mouse Drag Active flag
	public static bool bActive = true;

	// Minimum Zoom Level
	public int iMinZoomLevel = 1;
	// Maximum Zoom Level
	public int iMaxZoomLevel = 5;

	// Current Zoom Level
	float fCurrentZoomLevel = 1f;
	// Target Zoom Level
	float fTargetZoomLevel;

	// Initial Camera Size
	float fInitialCamOrthoSize;

	// Zoom Speed for the Mouse Wheel
	public float fMouseWheelZoomSpeed;
	// Zoom Speed for the GUI Buttons
	public float fGUIZoomIncrement;

	// Use this for initialization
	void Start () 
	{
		// Set the Target Zoom to the Current Zoom Level
		fTargetZoomLevel = fCurrentZoomLevel;
		// Get the Initial Camera Size
		fInitialCamOrthoSize = camera.orthographicSize;
	}

	// Update is called once per frame
	void Update () 
	{
		if (bActive)
		{
			if (!Building.bDraggingBuilding)
			{
				// Handle the Mouse Zoom variables
				HandleMouseZoom();
			}
		}
	}

	// Handles the Mouse Zoom variables
	void HandleMouseZoom()
	{
		// Keep the zoom level between the min and max zooms
		fCurrentZoomLevel = Mathf.Clamp (fCurrentZoomLevel, iMinZoomLevel, iMaxZoomLevel);

		// Handle the Mouse Wheel Zoom 
		HandleMouseWheelZoom();

		// Set the zoom
		camera.orthographicSize = fInitialCamOrthoSize / fCurrentZoomLevel;
	}

	// Handles the Mouse Wheel Input for the Zoom
	void HandleMouseWheelZoom()
	{
		// If the mouse wheel is scrolled backwards then zoom out
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			ChangeZoom(-fMouseWheelZoomSpeed);
		}
		// Otherwise if the mouse wheel is scrolled forwards then zoom in
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			ChangeZoom(fMouseWheelZoomSpeed);
		}
	}

	// Changes the zoom by the increment
	public void ChangeZoom(float increment)
	{
		fCurrentZoomLevel += increment;
	}
}
