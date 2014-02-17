using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer( typeof( SettingsListAttribute ))]
public class SettingsListDrawer : PropertyDrawer
{
	private new SettingsListAttribute attribute
	{
		get {
			return base.attribute as SettingsListAttribute;
		}
	}
	
	public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
	{
		return property.isExpanded ? 32f : 32f;
	}

	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
	{
		//		// draw foldout property
		//		ShowFoldout( position, property, label );
		//		if( !property.isExpanded )
		//			return;
		//
		//		position.y += 16f;
		//
		//		// check for wrong property values
		//		if( !property.isArray )
		//		{
		//			ShowError( position );
		//			return;	
		//		}
		//
		//		SerializedProperty size = property.FindPropertyRelative( "Array.size" );
		//		if( !size.hasMultipleDifferentValues )
		//		{
		//			ShowDifferentSizesText( position );
		//			return;
		//		}
		
		GUI.Button( position, new GUIContent( "+", "duplication" ), EditorStyles.miniButtonMid );
//		position.y += 10f;
//		ShowError( position );
	}

	private void ShowFoldout( Rect position, SerializedProperty property, GUIContent label )
	{
		position.x += 20f;
		EditorGUI.indentLevel -= 1;
		label = EditorGUI.BeginProperty( position, label, property );
		property.isExpanded = EditorGUI.Foldout( position, property.isExpanded, label, true );
		EditorGUI.EndProperty();
		EditorGUI.LabelField( position, "test label" );
	}
	
	private void ShowDifferentSizesText( Rect position )
	{
		position.x += 2;
		position.width -= 4F;
		position.height = 20F;
		EditorGUI.HelpBox( position, "Not showing lists with different sizes.", MessageType.Info );
	}

	private void ShowError( Rect position )
	{
		position.x += 2F;
		position.width -= 4F;
		position.height = 20F;
		EditorGUI.HelpBox( position, "Property is not an array nor a list.", MessageType.Error );
	}
}
