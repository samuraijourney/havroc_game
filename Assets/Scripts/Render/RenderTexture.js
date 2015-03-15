#pragma strict

static var renderedCameraTexture : Texture2D; // Allows global access to this by scripts

public var materialCameraView : Material;

function Awake() // This happens once only
{
	renderedCameraTexture = new Texture2D(Screen.width, Screen.height); // Creates a blank texture at screen size

	materialCameraView.mainTexture = renderedCameraTexture; // Connects this 2D texture to a material
}

function OnPostRender() // This happens every frame
{
	renderedCameraTexture.ReadPixels(Rect(0, 0, Screen.width, Screen.height), 0, 0); // Reads screen after all rendering

	renderedCameraTexture.Apply(); // Paste the image
}