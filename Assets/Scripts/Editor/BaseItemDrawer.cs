using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer ( typeof( BaseItemAttribute ))]
public class BaseItemDrawer : PropertyDrawer
{
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label)
	{
		return 16f;
	}

	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label)
	{
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel += 1;
			
		EditorGUI.PropertyField( pos, prop, new GUIContent( label ));

		EditorGUI.indentLevel = indent;
	}
}
