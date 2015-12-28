using UnityEngine;
using System.Collections;

public class ZoomOutButtonGUI : ButtonGUI 
{
	CameraControl cameraComponent;

	void Start()
	{
		cameraComponent = Camera.main.GetComponent<CameraControl> ();
	}

	public override void ButtonEffect()
	{
		cameraComponent.ChangeZoom (-cameraComponent.fGUIZoomIncrement);
		print ("Zoom Out Button");
	}
}
