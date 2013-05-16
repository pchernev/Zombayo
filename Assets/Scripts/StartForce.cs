using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof( FingerDownDetector ))]
[RequireComponent( typeof( FingerMotionDetector ))]
[RequireComponent( typeof( FingerUpDetector ))]
[RequireComponent( typeof( ScreenRaycaster ))]

public class StartForce : MonoBehaviour
{
	
	class PathRenderer
	{
		LineRenderer lineRenderer;
		
		List<Vector3> points = new List<Vector3>();
		List<GameObject> markers = new List<GameObject>();
		
		public PathRenderer( int index, LineRenderer lineRendererPrefab )
		{
			lineRenderer = Instantiate( lineRendererPrefab ) as LineRenderer;
			lineRenderer.name = lineRendererPrefab.name + index;
			lineRenderer.enabled = true;
			
			UpdateLines();
		}
		
		public void Reset()
		{
			points.Clear();
			UpdateLines();
			
			foreach( GameObject marker in markers )
				Destroy( marker );
			
			markers.Clear();
		}
		
		public void AddPoint( Vector2 screenPos )
		{
			AddPoint( screenPos, null );
		}
		
		public void AddPoint( Vector2 screenPos, GameObject markerPrefab )
		{
			Vector3 pos = GetWorldPos( screenPos );
			
			if( markerPrefab )
				AddMarker( pos, markerPrefab );
			
			points.Add( pos );
			UpdateLines();
		}
		
		GameObject AddMarker( Vector2 pos, GameObject prefab )
		{
			GameObject instance = Instantiate( prefab, pos, Quaternion.identity ) as GameObject;
			instance.name = prefab.name + "(" + markers.Count + ")";
			markers.Add( instance );
			return instance;
		}
		
		void UpdateLines()
		{
			lineRenderer.SetVertexCount( points.Count );
			for( int i = 0; i < points.Count; ++i )
				lineRenderer.SetPosition( i, points[i] );
		}
		
		
		public Vector3 GetWorldPos( Vector2 screenPos )
		{
			Ray ray = Camera.main.ScreenPointToRay( screenPos );
			
			float t = -ray.origin.z / ray.direction.z;
			
			return ray.GetPoint( t );
		}
	}
	
	// Use this for initialization
	void Start () {
		Debug.Log( "Start" );
	
		var force = new Vector3( 0.0f, 1000.0f, -2000.0f );
		this.rigidbody.AddForce( force );
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log( "Update" );
	}
	
	void OnCollisionEnter(Collision collision)
	{
//		Debug.Log( collision.gameObject.name );	
	}
}
