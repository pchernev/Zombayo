using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomPropertyDrawer (typeof (AnimEvent))]
public class AnimEventInspector : PropertyDrawer
{
	private bool viewData = true;
	
    public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) 
	{
		SerializedProperty animationTag = prop.FindPropertyRelative("animationTag");
		viewData = EditorGUILayout.Foldout(viewData, "Element: " + animationTag.stringValue);
		if(viewData){
			EditorGUILayout.PropertyField(animationTag, new GUIContent("Animation Tag", "The TAG of the animation state that the event is going to occur on."));
			
			if (animationTag.stringValue == ""){
				EditorGUILayout.LabelField("Please enter an animation Tag relating to the animation the event is going to occur on.");
			}
			else{
				SerializedProperty layer = prop.FindPropertyRelative("layer");
				EditorGUILayout.PropertyField(layer, new GUIContent("Animator Layer", "The index number of the animator layer your expecting this event " +
					"to be called from. 0 is the default base layer"));
				
				SerializedProperty fireTime = prop.FindPropertyRelative("fireTime");
				EditorGUILayout.PropertyField(fireTime, new GUIContent("Fire Time", "The normalized time at which the event should fire. " +
					"The integer part is the number of times a state has been looped. The fractional part is the % (0-1) of progress in the current loop."));
				
				// NOT Implemented
				//SerializedProperty repeat = prop.FindPropertyRelative("repeat");
				//EditorGUILayout.PropertyField(repeat, new GUIContent("Repeat", "Tic if you would like the even to repeat every cycle. If left unchecked it will only" +
				//	"occur once during the cycle at the set time."));
				
				SerializedProperty eventType = prop.FindPropertyRelative("eventType");
				EditorGUILayout.PropertyField(eventType, new GUIContent("Event Type"));
				
				if (eventType.enumValueIndex == 0)
				{
					SerializedProperty effect = prop.FindPropertyRelative("effect");
					EditorGUILayout.PropertyField(effect, new GUIContent("Instantiated Object", "The GameObject or Prefab that should be Instantiated."));
					
					SerializedProperty target = prop.FindPropertyRelative("target");
					EditorGUILayout.PropertyField(target, new GUIContent("Target", "The position where the object should be Instatiated."));
					
					SerializedProperty castToSurface = prop.FindPropertyRelative("castToSurface");
					EditorGUILayout.PropertyField(castToSurface, new GUIContent("Cast To Surface", "Fire a ray down and look for a surface to Instantiate the object at?"));
					
					SerializedProperty rotation = prop.FindPropertyRelative("rotation");
					EditorGUILayout.PropertyField(rotation, new GUIContent("Rotation", "Apply a random rotation to the Instatiated Object."));
				}
				
				else
				{
					SerializedProperty target = prop.FindPropertyRelative("target");
					EditorGUILayout.PropertyField(target, new GUIContent("Target", "The object the method should be called from."));
					
					SerializedProperty MethodCall = prop.FindPropertyRelative("MethodCall");
					EditorGUILayout.PropertyField(MethodCall, new GUIContent("Method Call", "The name of the method that should be called. " +
						"Do not include () or arguments, that feature is not yet implemented."));
				}
			}
		}
    }
}
