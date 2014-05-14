using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor( typeof( CameraFollow ))]
public class CameraFollowEditor : Editor
{
	CameraFollow _instance;
	PropertyField[] _fields;

	public void OnEnable()
	{
		_instance = target as CameraFollow;
		_fields = ExposeProperties.GetProperties( _instance );
	}

	public override void OnInspectorGUI ()
	{
		if( _instance == null )
			return;

		this.DrawDefaultInspector();

		ExposeProperties.Expose( _fields );
	}
}
