using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColorPoint))]
public class ColorPointDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		GUI.Label(position, label);
		position.x += 40f;
		position.width = 24f;
		GUI.Button( position, new GUIContent( "+", "duplication" ), EditorStyles.miniButtonMid );
	}
}
