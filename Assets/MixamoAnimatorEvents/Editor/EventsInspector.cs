using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AnimatorEvents))]
public class EventsInspector : Editor {

	public override void OnInspectorGUI() 
	{
		serializedObject.Update();
	    var myIterator = serializedObject.FindProperty("Events");
		
	   	while (true) {
	        Rect myRect = GUILayoutUtility.GetRect(0f, 16f);
	        var showChildren = EditorGUI.PropertyField(myRect, myIterator);
			
	        if (!myIterator.NextVisible(showChildren)){
				break;
			}
		}
	 	
	    serializedObject.ApplyModifiedProperties();
	}
}
