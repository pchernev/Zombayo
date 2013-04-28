//Move to Surface
//By Curtis Williams

using UnityEngine;
using UnityEditor;

public class MoveToSurface : EditorWindow {
	
	GameObject[] selObj;
	int objNumber = 0;
	Vector3 direction;
	float dirX;
	float dirY;
	float dirZ;
	bool rotAdjust;
	
	[MenuItem("Edit/Object Placement/Move to Surface")]
	public static void Init (){
		var window = GetWindow (typeof(MoveToSurface)) as MoveToSurface;
		window.wantsMouseMove = true;
		
		Object.DontDestroyOnLoad( window );
	}
    void OnGUI () {
		// selected objects
		selObj = Selection.gameObjects;
				
		// choose direction to project
		GUILayout.Label ("Choose Direction", EditorStyles.boldLabel);
		dirX = EditorGUILayout.Slider("X", dirX, -1, 1);
		dirY = EditorGUILayout.Slider("Y", dirY, -1, 1);
		dirZ = EditorGUILayout.Slider("Z", dirZ, -1, 1);
		
		if(dirX > 0 && dirX < 0.5f){
			GUILayout.Label ("A little Right", EditorStyles.label);
		}
		else if(dirX > 0.5f && dirX <= 1){
			GUILayout.Label ("Right", EditorStyles.label);
		}
		else if(dirX < 0 && dirX > -0.5f){
			GUILayout.Label ("A little Left", EditorStyles.label);
		}
		else if(dirX < -0.5f && dirX >= -1){
			GUILayout.Label ("Left", EditorStyles.label);
		}
		else{
			GUILayout.Label (" ", EditorStyles.label);
		}
		
		if(dirY > 0 && dirY < 0.5f){
			GUILayout.Label ("A little Upward", EditorStyles.label);
		}
		else if(dirY > 0.5f && dirY <= 1){
			GUILayout.Label ("Up", EditorStyles.label);
		}
		else if(dirY < 0 && dirY > -0.5f){
			GUILayout.Label ("A little Downward", EditorStyles.label);
		}
		else if(dirY < -0.5f && dirY >= -1){
			GUILayout.Label ("Down", EditorStyles.label);
		}
		else{
			GUILayout.Label (" ", EditorStyles.label);
		}

		if(dirZ > 0 && dirZ < 0.5f){
			GUILayout.Label ("A little Forward", EditorStyles.label);
		}
		else if(dirZ > 0.5f && dirZ <= 1){
			GUILayout.Label ("Forward", EditorStyles.label);
		}
		else if(dirZ < 0 && dirZ > -0.5f){
			GUILayout.Label ("A little Backward", EditorStyles.label);
		}
		else if(dirZ < -0.5f && dirZ >= -1){
			GUILayout.Label ("Backward", EditorStyles.label);
		}
		else{
			GUILayout.Label (" ", EditorStyles.label);
		}
		
		direction = new Vector3(dirX, dirY, dirZ);
		
		rotAdjust = EditorGUILayout.Toggle("Rotate to Surface", rotAdjust);

		if(GUILayout.Button("Move to Surface", EditorStyles.miniButton)){
			Undo.RegisterSceneUndo("Random Offsetting");
			for(objNumber = 0; objNumber < selObj.Length; objNumber++){
				RaycastHit surfaceHit = new RaycastHit();
				if(Physics.Raycast(selObj[objNumber].transform.position, direction, out surfaceHit)){
					selObj[objNumber].transform.position = surfaceHit.point;
					if(rotAdjust){
						selObj[objNumber].transform.up = surfaceHit.normal;
						//selObj[objNumber].transform.rotation = rotation;
					}
				}
				else
					Debug.Log("No Surface Detected for " + selObj[objNumber].name);
			}
		}
	}
}