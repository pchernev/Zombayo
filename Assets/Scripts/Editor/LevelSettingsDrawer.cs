using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomPropertyDrawer( typeof( SettingsListAttribute ))]
public class LevelSettingsDrawer : PropertyDrawer
{

	public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
	{
		return property.isExpanded ? 32f : 16f;
	}

	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
	{
		var size = 1;
		try {
			size = property.arraySize;
		}
		catch {
			try {
				size = property.FindPropertyRelative( "Array.size" ).intValue;
			}
			catch {}
		} 

		Debug.Log( "size: " + size.ToString() );

		position.height = 16f;
		position.x -= 14f;
		position.width += 14f;
		position.width /= 2f;
		label = EditorGUI.BeginProperty(position, label, property);
//		property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
		position.x += position.width;
		EditorGUI.LabelField( position, size.ToString() );
		EditorGUI.EndProperty();


		position.width *= 2f;

		position.y += 16f;
		
		if (!property.isExpanded) {
			return;
		}


		position.y += 16f;
		position.width /= (float)size;

		for( int i = 0; i < size; i++ )
		{
			var elem = property.GetArrayElementAtIndex( i );
			position.height = EditorGUI.GetPropertyHeight( elem );
			EditorGUI.TextField( position, label.ToString() );
			position.x += position.width;
		}
	}
}
