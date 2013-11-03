using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof( FingerDownDetector ))]
[RequireComponent( typeof( FingerMotionDetector ))]
[RequireComponent( typeof( FingerUpDetector ))]
[RequireComponent( typeof( ScreenRaycaster ))]

public class StartForce : MonoBehaviour
{
	public LineRenderer lineRendererPrefab;
	public GameObject fingerDownMarkerPrefab;
	public GameObject fingerMoveBeginMarkerPrefab;
	public GameObject fingerMoveEndMarkerPrefab;
	public GameObject fingerUpMarkerPrefab;
	public GameObject trailPrefab;
	
	public Rigidbody accelarateObject;
	
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
//			markers.Add( instance );
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
		
		public float Size()
		{
			return (float)points.Count;
		}
	}
	
	PathRenderer[] paths;
	
	// Use this for initialization
	void Start ()
	{
		paths = new PathRenderer[FingerGestures.Instance.MaxFingers];
		for( int i = 0; i < paths.Length; i++ )
			paths[i] = new PathRenderer( i, lineRendererPrefab );
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log( "Update" );
	}
	
	void OnCollisionEnter(Collision collision)
	{
//		Debug.Log( collision.gameObject.name );	
	}
	
	void OnFingerDown( FingerDownEvent e )
	{
		PathRenderer path = paths[e.Finger.Index];
		path.Reset();
		path.AddPoint( e.Finger.Position, fingerDownMarkerPrefab );
		
		if( trailPrefab != null )
		{
			var vec = new Vector3( e.Finger.Position.x, e.Finger.Position.y, trailPrefab.transform.position.z );
			Debug.Log( "posDown: " + vec );
			trailPrefab.transform.position = vec;
		}
	}
	
	void OnFingerMove( FingerMotionEvent e )
	{
		PathRenderer path = paths[e.Finger.Index];
		
		if( e.Phase == FingerMotionPhase.Started )
		{
			path.AddPoint( e.Position, fingerMoveBeginMarkerPrefab );
		}
		else if( e.Phase == FingerMotionPhase.Started )
		{
			path.AddPoint( e.Position );
		}
		else
		{
			path.AddPoint( e.Position, fingerMoveEndMarkerPrefab );
		}
		
		if( trailPrefab != null )
		{
			var vec = new Vector3( e.Position.x, e.Finger.Position.y, trailPrefab.transform.position.z );
			Debug.Log( "pos: " + vec.ToString() );
			trailPrefab.transform.position = vec;
		}
	}
	
	void OnFingerUp( FingerUpEvent e )
	{
		AccelerateObject( e.Finger.Index );
		PathRenderer path = paths[e.Finger.Index];
		path.AddPoint( e.Finger.Position, fingerUpMarkerPrefab );
		
		if( trailPrefab != null )
		{
			var vec = new Vector3( e.Finger.Position.x, e.Finger.Position.y, trailPrefab.transform.position.z );
			Debug.Log( "posUp: " + vec.ToString() );
			trailPrefab.transform.position = vec;
		}
	}
	
	void AccelerateObject( int index )
	{
		var f = paths[index].Size();
		var force = new Vector3( f * 170.0f, f * 530.0f, 0.0f );
		//this.accelarateObject.AddForce( force );	
		
		//Debug.Log( "AccelerateObject" );
	}
}
