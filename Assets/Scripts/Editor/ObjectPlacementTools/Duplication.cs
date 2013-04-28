//Duplication Tool
//By Curtis Williams

using UnityEngine;
using UnityEditor;

public class Duplication : EditorWindow {
	
	GameObject[] selObj;
	bool byGroup = true;
	bool byLocal = false;
	string objName = "";
	bool objNumering;
	int objNumber = 0;
	
	int duplicates = 0;
	int dupCounter = 0;
	Vector3 locOffset;
	Vector3 rotOffset;
	Vector3 scaleOffset;
	Quaternion rotation;
	Transform dupTransform;

	[MenuItem("Edit/Object Placement/Duplication")]
	public static void Init (){
		var window = GetWindow (typeof(Duplication)) as Duplication;
		window.wantsMouseMove = true;
		
		Object.DontDestroyOnLoad( window );
	}
	
    void OnGUI () {
		// get selected object
		selObj = Selection.gameObjects;

		// number of duplicates
		duplicates = EditorGUILayout.IntField("Number of Duplicates", duplicates);
		objName = EditorGUILayout.TextField("Name", objName);
		objNumering = EditorGUILayout.Toggle("Number Duplicates", objNumering);
		byLocal = EditorGUILayout.Toggle("By Local Orientations", byLocal);
		
		// duplicate as group
		if(selObj.Length > 1)
			byGroup = EditorGUILayout.Toggle("As Group", byGroup);
		else
			byGroup = false;
			
		// offsets inputs
		locOffset = EditorGUILayout.Vector3Field("Location Offset", locOffset);
		rotOffset = EditorGUILayout.Vector3Field("Rotation Offset", rotOffset);
		scaleOffset = EditorGUILayout.Vector3Field("Scale Offset", scaleOffset);
		
		// duplicate button
		if(GUILayout.Button("Duplicate", EditorStyles.miniButton)){
			Undo.RegisterSceneUndo("Duplicating");
			if(byGroup){
				// create a parent gameobject for the group of selection
				GameObject groupParent = new GameObject("Duplication Group");
				for(objNumber = 0; objNumber < selObj.Length; objNumber++){
					selObj[objNumber].transform.parent = groupParent.transform;
					groupParent.transform.position += selObj[objNumber].transform.position;
				}
				groupParent.transform.position /= objNumber;
				
				for(dupCounter = 0; dupCounter < duplicates; dupCounter++){
					if(byLocal){
						rotation = groupParent.transform.rotation;
						dupTransform = groupParent.transform;
						
						GameObject go = Instantiate(groupParent, dupTransform.position, dupTransform.rotation) as GameObject;
						if(objName.Length != 0)
							go.name = objName;
						if(objNumering){
							go.name += dupCounter;
						}
						
						dupTransform.position += dupTransform.right * locOffset.x;
						dupTransform.position += dupTransform.up * locOffset.y;
						dupTransform.position += dupTransform.forward * locOffset.z;
						
						rotation.eulerAngles += rotOffset;
						dupTransform.rotation = rotation;
						dupTransform.localScale += scaleOffset;
						
						groupParent.transform.position = dupTransform.position;
						groupParent.transform.rotation = dupTransform.rotation;
						groupParent.transform.localScale = dupTransform.localScale;
					}
					else{
						rotation = groupParent.transform.rotation;
						dupTransform = groupParent.transform;
						
						GameObject go = Instantiate(groupParent, dupTransform.position, dupTransform.rotation) as GameObject;
						if(objName.Length != 0)
							go.name = objName;
						if(objNumering){
							go.name += dupCounter;
						}
						
						dupTransform.position += locOffset;
						rotation.eulerAngles += rotOffset;
						dupTransform.rotation = rotation;
						dupTransform.localScale += scaleOffset;
						
						groupParent.transform.position = dupTransform.position;
						groupParent.transform.rotation = dupTransform.rotation;
						groupParent.transform.localScale = dupTransform.localScale;
					}
				}
			}
			else{
				for(dupCounter = 0; dupCounter < duplicates; dupCounter++){
					if(byLocal){
						for(objNumber = 0; objNumber < selObj.Length; objNumber++){
							rotation = selObj[objNumber].transform.rotation;
							dupTransform = selObj[objNumber].transform;
							
							GameObject go = Instantiate(selObj[objNumber], dupTransform.position, dupTransform.rotation) as GameObject;
							if(objName.Length != 0)
								go.name = objName;
							else
								go.name = selObj[objNumber].name;
							if(objNumering){
								go.name += dupCounter;
							}
							
							dupTransform.position += dupTransform.right * locOffset.x;
							dupTransform.position += dupTransform.up * locOffset.y;
							dupTransform.position += dupTransform.forward * locOffset.z;
	
							rotation.eulerAngles += rotOffset;
							dupTransform.rotation = rotation;
							dupTransform.localScale += scaleOffset;
						}
					}
					else{
						for(objNumber = 0; objNumber < selObj.Length; objNumber++){
							rotation = selObj[objNumber].transform.rotation;
							dupTransform = selObj[objNumber].transform;
							
							GameObject go = Instantiate(selObj[objNumber], dupTransform.position, dupTransform.rotation) as GameObject;
							if(objName.Length != 0)
								go.name = objName;
							else
								go.name = selObj[objNumber].name;
							if(objNumering){
								go.name += dupCounter;
							}
							
							dupTransform.position += locOffset;
							rotation.eulerAngles += rotOffset;
							dupTransform.rotation = rotation;
							dupTransform.localScale += scaleOffset;
						}
					}
				}
			}
		}
	}
}