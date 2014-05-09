//Align Tool
//By Curtis Williams

using UnityEngine;
using UnityEditor;

public class AlignTool : EditorWindow {
		
	GameObject inObj;
	GameObject outObj;
	GameObject[] selObj;
	int objNumber = 0;
	string objName = "";
	Vector3 locEnd;
	Vector3 rotEnd;
	Vector3 scaleEnd;
	bool objInScene;
    Vector3 locOffset = Vector3.zero;
	bool locX = true;
	bool locY = true;
	bool locZ = true;	
	Vector3 rotOffset = Vector3.zero;
	bool rotX = true;
	bool rotY = true;
	bool rotZ = true;
	Vector3 scaleOffset = Vector3.zero;
	bool scaleX = true;
	bool scaleY = true;
	bool scaleZ = true;
	Quaternion rotation = Quaternion.identity;
	bool alignMult = false;
	
	[MenuItem("Edit/Object Placement/Align Tool")]
	public static void Init (){
		var window = GetWindow (typeof(AlignTool)) as AlignTool;
		window.wantsMouseMove = true;
		
		Object.DontDestroyOnLoad( window );
	}
    
    void OnGUI(){
		// Input objects, aligning first(outObj) with the second(inObj)
		outObj = EditorGUILayout.ObjectField("Align This", outObj, typeof(GameObject), true) as GameObject;
		if(!alignMult){
			inObj = EditorGUILayout.ObjectField("With This", inObj, typeof(GameObject), true) as GameObject;
		}
		else{
			selObj = Selection.gameObjects;
			GUILayout.Label("With selected objects.", EditorStyles.label);
		}
		
		alignMult = EditorGUILayout.Toggle("Align Multiple Copies", alignMult);
		
		objName = EditorGUILayout.TextField("Object Name", objName);
		
		if(outObj != null){
			if(inObj != null){
				// Sets default end location, rotation, and scale
				locEnd = inObj.transform.position;
				rotEnd = inObj.transform.rotation.eulerAngles;
				scaleEnd = inObj.transform.localScale;
			}
			
			// Check if the object to be aligned is in the scene
			if(outObj.activeSelf){
				objInScene = true;
			}
			else{
				objInScene = false;
			}
			
			// Checkboxes for alignment axes to be used
			GUILayout.Label ("Align Axes", EditorStyles.boldLabel);
			locX = EditorGUILayout.Toggle ("X position", locX);
			locY = EditorGUILayout.Toggle ("Y position", locY);
			locZ = EditorGUILayout.Toggle ("Z position", locZ);
			
			GUILayout.Label ("", EditorStyles.boldLabel);
			rotX = EditorGUILayout.Toggle ("X rotation", rotX);
			rotY = EditorGUILayout.Toggle ("Y rotation", rotY);
			rotZ = EditorGUILayout.Toggle ("Z rotation", rotZ);
			
			GUILayout.Label ("", EditorStyles.boldLabel);
			scaleX = EditorGUILayout.Toggle ("X scale", scaleX);
			scaleY = EditorGUILayout.Toggle ("Y scale", scaleY);
			scaleZ = EditorGUILayout.Toggle ("Z scale", scaleZ);
			
			// Menu spacer
			GUILayout.Label ("", EditorStyles.boldLabel);
			
			// Modify the end location and end rotation based on used alignment axes
			if(objInScene){
				// location
				if(!locX){
					locEnd.x = outObj.transform.position.x;
				}
				if(!locY){
					locEnd.y = outObj.transform.position.y;
				}
				if(!locZ){
					locEnd.z = outObj.transform.position.z;
				}
				// rotation
				if(!rotX){
					rotEnd.x = outObj.transform.rotation.eulerAngles.x;
				}
				if(!rotY){
					rotEnd.y = outObj.transform.rotation.eulerAngles.y;
				}
				if(!rotZ){
					rotEnd.z = outObj.transform.rotation.eulerAngles.z;
				}
				// scale
				if(!scaleX){
					scaleEnd.x = outObj.transform.localScale.x;
				}
				if(!scaleY){
					scaleEnd.y = outObj.transform.localScale.y;
				}
				if(!scaleZ){
					scaleEnd.z = outObj.transform.localScale.z;
				}
			}
			else{
				// location
				if(!locX){
					locEnd.x = 0;
				}
				if(!locY){
					locEnd.y = 0;
				}
				if(!locZ){
					locEnd.z = 0;
				}
				// rotation
				if(!rotX){
					rotEnd.x = 0;
				}
				if(!rotY){
					rotEnd.y = 0;
				}
				if(!rotZ){
					rotEnd.z = 0;
				}
				// scale
				if(!scaleX){
					scaleEnd.x = 1;
				}
				if(!scaleY){
					scaleEnd.y = 1;
				}
				if(!scaleZ){
					scaleEnd.z = 1;
				}
			}
			
			// Offset Vecter inputs
			GUILayout.Label ("Offset", EditorStyles.boldLabel);
			locOffset = EditorGUILayout.Vector3Field ("Position Offset", locOffset);
			rotOffset = EditorGUILayout.Vector3Field ("Rotation Offset", rotOffset);
			scaleOffset = EditorGUILayout.Vector3Field ("Scale Offset", scaleOffset);
			
			// Apply offsets to location, rotation, and scale
			locEnd += locOffset;
			rotEnd += rotOffset;
			scaleEnd += scaleOffset;
			
			// Translate rotation
			rotation.eulerAngles = rotEnd;
			
			// Buttons
			if(!alignMult){
				if(inObj != null){
					// Aligns first object to second object, or creates a new one
					if(GUILayout.Button("Align", EditorStyles.miniButton)){
						Undo.RegisterSceneUndo("Align Object");
						if(outObj.activeSelf){
							outObj.transform.position = locEnd;
							outObj.transform.rotation = rotation;
							outObj.transform.localScale = scaleEnd;
							if(objName.Length != 0){
								outObj.name = objName;
							}
						}
						else{
							GameObject go = Instantiate(outObj, locEnd, rotation) as GameObject;
							go.transform.localScale = scaleEnd;
							if(objName.Length != 0){
								go.name = objName;
							}
						}
					}
					// Aligns a copy of first object to second object
					if(GUILayout.Button("Align Copy", EditorStyles.miniButton)){
						Undo.RegisterSceneUndo("Align Object");
						if(!alignMult){
							GameObject go = Instantiate(outObj, locEnd, rotation) as GameObject;
							go.transform.localScale = scaleEnd;
							if(objName.Length != 0){
								go.name = objName;
							}
						}
						else{
							for(objNumber = 0; objNumber < selObj.Length; objNumber++){
								rotEnd = selObj[objNumber].transform.rotation.eulerAngles;
								rotEnd += rotOffset;
								rotation.eulerAngles = rotEnd;
								GameObject go = Instantiate(outObj, selObj[objNumber].transform.position, rotation) as GameObject;
								go.transform.position = selObj[objNumber].transform.position + locOffset;
								go.transform.localScale = selObj[objNumber].transform.localScale + scaleOffset;
								if(objName.Length != 0){
									go.name = objName;
								}
							}
						}
					}
					// Aligns a first object to second object, then deletes second object
					if(GUILayout.Button("Replace", EditorStyles.miniButton)){
						Undo.RegisterSceneUndo("Align Object");
						if(outObj.activeSelf){
							outObj.transform.position = locEnd;
							outObj.transform.rotation = rotation;
							outObj.transform.localScale = scaleEnd;
							if(objName.Length != 0){
								outObj.name = objName;
							}
						}
						else{
							GameObject go = Instantiate(outObj, locEnd, rotation) as GameObject;
							go.transform.localScale = scaleEnd;
							if(objName.Length != 0){
								go.name = objName;
							}
						}
						DestroyImmediate(inObj);
					}
					// Aligns a copy of first object to second object, then deletes second object
					if(GUILayout.Button("Replace with Copy", EditorStyles.miniButton)){
						Undo.RegisterSceneUndo("Align Object");
						if(!alignMult){
							GameObject go = Instantiate(outObj, locEnd, rotation) as GameObject;
							go.transform.localScale = scaleEnd;
							if(objName.Length != 0){
								go.name = objName;
							}
							DestroyImmediate(inObj);
						}
						else{
							for(objNumber = 0; objNumber < selObj.Length; objNumber++){
								rotEnd = selObj[objNumber].transform.rotation.eulerAngles;
								rotEnd += rotOffset;
								rotation.eulerAngles = rotEnd;
								GameObject go = Instantiate(outObj, selObj[objNumber].transform.position, rotation) as GameObject;
								go.transform.position = selObj[objNumber].transform.position + locOffset;
								go.transform.localScale = selObj[objNumber].transform.localScale + scaleOffset;
								if(objName.Length != 0){
									go.name = objName;
								}
								DestroyImmediate(selObj[objNumber]);
							}
						}
					}
				}
			}
			else{
				if(selObj.Length > 0){
					// Aligns a copy of first object to second object
					if(GUILayout.Button("Align Copy", EditorStyles.miniButton)){
						Undo.RegisterSceneUndo("Align Object");
						if(!alignMult){
							GameObject go = Instantiate(outObj, locEnd, rotation) as GameObject;
							go.transform.localScale = scaleEnd;
							if(objName.Length != 0){
								go.name = objName;
							}
						}
						else{
							for(objNumber = 0; objNumber < selObj.Length; objNumber++){
								rotEnd = selObj[objNumber].transform.rotation.eulerAngles;
								rotEnd += rotOffset;
								rotation.eulerAngles = rotEnd;
								GameObject go = Instantiate(outObj, selObj[objNumber].transform.position, rotation) as GameObject;
								go.transform.position = selObj[objNumber].transform.position + locOffset;
								go.transform.localScale = selObj[objNumber].transform.localScale + scaleOffset;
								if(objName.Length != 0){
									go.name = objName;
								}
							}
						}
					}
					// Aligns a copy of first object to second object, then deletes second object
					if(GUILayout.Button("Replace with Copy", EditorStyles.miniButton)){
						Undo.RegisterSceneUndo("Align Object");
						if(!alignMult){
							GameObject go = Instantiate(outObj, locEnd, rotation) as GameObject;
							go.transform.localScale = scaleEnd;
							if(objName.Length != 0){
								go.name = objName;
							}
							DestroyImmediate(inObj);
						}
						else{
							for(objNumber = 0; objNumber < selObj.Length; objNumber++){
								rotEnd = selObj[objNumber].transform.rotation.eulerAngles;
								rotEnd += rotOffset;
								rotation.eulerAngles = rotEnd;
								GameObject go = Instantiate(outObj, selObj[objNumber].transform.position, rotation) as GameObject;
								go.transform.position = selObj[objNumber].transform.position + locOffset;
								go.transform.localScale = selObj[objNumber].transform.localScale + scaleOffset;
								if(objName.Length != 0){
									go.name = objName;
								}
								DestroyImmediate(selObj[objNumber]);
							}
						}
					}
				}
			}
		}
	}
}