// Create a public variable where we can assign the GUISkin
var customSkin : GUISkin;

// Apply the Skin in our OnGUI() function
function OnGUI () {
	GUI.skin = customSkin;

	// Now create any Controls you like, and they will be displayed with the custom Skin
	GUILayout.Button ("I am a re-Skinned Button");

	// You can change or remove the skin for some Controls but not others
	//GUI.skin = null;

	// Any Controls created here will use the default Skin and not the custom Skin
	GUILayout.Button ("This Button uses the default UnityGUI Skin");
}