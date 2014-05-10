//Random Offsetter
//By Curtis Williams

using UnityEngine;
using UnityEditor;

public class RandomOffsetter : EditorWindow {
	
	GameObject[] selObj;
	int objCounter = 0;
	bool uniform = false;
	
	bool randLocation;
	Vector3 randLoc;
	
	float locXRand;
	float locXStart = 0;
	float locXEnd = 0;
	
	float locYRand;
	float locYStart = 0;
	float locYEnd = 0;
	
	float locZRand;
	float locZStart = 0;
	float locZEnd = 0;
	
	bool randRotation;
	Vector3 randRot;
	Quaternion rotation;
	
	float rotXRand;
	float rotXStart = 0;
	float rotXEnd = 360;
	
	float rotYRand;
	float rotYStart = 0;
	float rotYEnd = 360;
	
	float rotZRand;
	float rotZStart = 0;
	float rotZEnd = 360;
	
	bool randScaling;
	Vector3 randScale;
	
	float scaleXRand;
	float scaleXStart = 1;
	float scaleXEnd = 1;
	bool scaleXInvert;
	
	float scaleYRand;
	float scaleYStart = 1;
	float scaleYEnd = 1;
	bool scaleYInvert;
	
	float scaleZRand;
	float scaleZStart = 1;
	float scaleZEnd = 1;
	bool scaleZInvert;
	
	[MenuItem("Edit/Object Placement/Random Offsetter")]
	public static void Init (){
		var window = GetWindow (typeof(RandomOffsetter)) as RandomOffsetter;
		window.wantsMouseMove = true;
		
		Object.DontDestroyOnLoad( window );
	}
	
    void OnGUI () {
		// get selected object
		selObj = Selection.gameObjects;
		
		//float input fields		
		randLocation = EditorGUILayout.Toggle ("Offset Location", randLocation);
		if(randLocation){
			locXStart = EditorGUILayout.FloatField("X Min", locXStart);
			locXEnd = EditorGUILayout.FloatField("X Max", locXEnd);
			locYStart = EditorGUILayout.FloatField("Y Min", locYStart);
			locYEnd = EditorGUILayout.FloatField("Y Max", locYEnd);
			locZStart = EditorGUILayout.FloatField("Z Min", locZStart);
			locZEnd = EditorGUILayout.FloatField("Z Max", locZEnd);
		}
		
		randRotation = EditorGUILayout.Toggle ("Offset Rotation", randRotation);
		if(randRotation){
			rotXStart = EditorGUILayout.FloatField("X Min", rotXStart);
			if(rotXStart < 0)
				rotXStart = 0;
			rotXEnd = EditorGUILayout.FloatField("X Max", rotXEnd);
			if(rotXEnd > 360)
				rotXEnd = 360;
			rotYStart = EditorGUILayout.FloatField("Y Min", rotYStart);
			if(rotYStart < 0)
				rotYStart = 0;
			rotYEnd = EditorGUILayout.FloatField("Y Max", rotYEnd);
			if(rotYEnd > 360)
				rotYEnd = 360;
			rotZStart = EditorGUILayout.FloatField("Z Min", rotZStart);
			if(rotZStart < 0)
				rotZStart = 0;
			rotZEnd = EditorGUILayout.FloatField("Z Max", rotZEnd);
			if(rotZEnd > 360)
				rotZEnd = 360;
		}
				
		randScaling = EditorGUILayout.Toggle ("Offset Scale", randScaling);
		if(randScaling){
			scaleXStart = EditorGUILayout.FloatField("X Min", scaleXStart);
			if(scaleXStart < 0.1f)
				scaleXStart = 0.1f;
			scaleXEnd = EditorGUILayout.FloatField("X Max", scaleXEnd);
			scaleXInvert = EditorGUILayout.Toggle("Allow Negative X", scaleXInvert);
			scaleYStart = EditorGUILayout.FloatField("Y Min", scaleYStart);
			if(scaleYStart < 0.1f)
				scaleYStart = 0.1f;
			scaleYEnd = EditorGUILayout.FloatField("Y Max", scaleYEnd);
			scaleYInvert = EditorGUILayout.Toggle("Allow Negative Y", scaleYInvert);
			scaleZStart = EditorGUILayout.FloatField("Z Min", scaleZStart);
			if(scaleZStart < 0.1f)
				scaleZStart = 0.1f;
			scaleZEnd = EditorGUILayout.FloatField("Z Max", scaleZEnd);
			scaleZInvert = EditorGUILayout.Toggle("Allow Negative Z", scaleZInvert);
		}	
		
		if(selObj.Length >1)
			uniform = EditorGUILayout.Toggle("Uniform Randomness", uniform);
		
		//offset button
		if(GUILayout.Button("Offset", EditorStyles.miniButton)){
			Undo.RegisterSceneUndo("Random Offsetting");
			if(!uniform){
				for(objCounter = 0; objCounter < selObj.Length; objCounter++){
				
				locXRand = Random.Range(locXStart, locXEnd);
				locYRand = Random.Range(locYStart, locYEnd);
				locZRand = Random.Range(locZStart, locZEnd);
				
				rotXRand = Random.Range(rotXStart, rotXEnd);
				rotYRand = Random.Range(rotYStart, rotYEnd);
				rotZRand = Random.Range(rotZStart, rotZEnd);
				
				scaleXRand = Random.Range(scaleXStart, scaleXEnd);
				if(scaleXInvert){
					int invertX = Random.Range(0, 2);
					if(invertX == 0)
						scaleXRand *= -1;
				}
				scaleYRand = Random.Range(scaleYStart, scaleYEnd);
				if(scaleYInvert){
					int invertY = Random.Range(0, 2);
					if(invertY == 0)
						scaleYRand *= -1;
				}
				scaleZRand = Random.Range(scaleZStart, scaleZEnd);
				if(scaleZInvert){
					int invertZ = Random.Range(0, 2);
					if(invertZ == 0)
						scaleZRand *= -1;
				}
				
				randLoc = new Vector3(locXRand, locYRand, locZRand);
				randRot = new Vector3(rotXRand, rotYRand, rotZRand);
				rotation.eulerAngles = randRot;
				randScale = new Vector3(scaleXRand, scaleYRand, scaleZRand);
				
				if(randLocation)
					selObj[objCounter].transform.position += randLoc;
				if(randRotation)
					selObj[objCounter].transform.rotation = rotation;
				if(randScaling)
					selObj[objCounter].transform.localScale = randScale;
				}
			}
			else{
				locXRand = Random.Range(locXStart, locXEnd);
				locYRand = Random.Range(locYStart, locYEnd);
				locZRand = Random.Range(locZStart, locZEnd);
				
				rotXRand = Random.Range(rotXStart, rotXEnd);
				rotYRand = Random.Range(rotYStart, rotYEnd);
				rotZRand = Random.Range(rotZStart, rotZEnd);
				
				scaleXRand = Random.Range(scaleXStart, scaleXEnd);
				if(scaleXInvert){
					int invertX = Random.Range(0, 2);
					if(invertX == 0)
						scaleXRand *= -1;
				}
				scaleYRand = Random.Range(scaleYStart, scaleYEnd);
				if(scaleYInvert){
					int invertY = Random.Range(0, 2);
					if(invertY == 0)
						scaleYRand *= -1;
				}
				scaleZRand = Random.Range(scaleZStart, scaleZEnd);
				if(scaleZInvert){
					int invertZ = Random.Range(0, 2);
					if(invertZ == 0)
						scaleZRand *= -1;
				}
				
				randLoc = new Vector3(locXRand, locYRand, locZRand);
				randRot = new Vector3(rotXRand, rotYRand, rotZRand);
				rotation.eulerAngles = randRot;
				randScale = new Vector3(scaleXRand, scaleYRand, scaleZRand);
				
				for(objCounter = 0; objCounter < selObj.Length; objCounter++){
					if(randLocation)
						selObj[objCounter].transform.position += randLoc;
					if(randRotation)
						selObj[objCounter].transform.rotation = rotation;
					if(randScaling)
						selObj[objCounter].transform.localScale = randScale;
				}
			}
		}
		// 0 out button
		if(GUILayout.Button("Reset Rotation", EditorStyles.miniButton)){
			Undo.RegisterSceneUndo("Random Offsetting");
			for(objCounter = 0; objCounter < selObj.Length; objCounter++){
				rotation.eulerAngles = Vector3.zero;
				selObj[objCounter].transform.rotation = rotation;
			}
		}
		if(GUILayout.Button("Reset Scale", EditorStyles.miniButton)){
			Undo.RegisterSceneUndo("Random Offsetting");
			for(objCounter = 0; objCounter < selObj.Length; objCounter++){
				selObj[objCounter].transform.localScale = Vector3.one;
			}
		}
	}
}
