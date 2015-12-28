using UnityEngine;
using System.Collections;

public class ZoomInButtonGUI : ButtonGUI 
{
	CameraControl cameraComponent;

	void Start()
	{
		cameraComponent = Camera.main.GetComponent<CameraControl> ();
	}

	public override void ButtonEffect()
	{
		cameraComponent.ChangeZoom (cameraComponent.fGUIZoomIncrement);
		print ("Zoom In Button");
	}
}
