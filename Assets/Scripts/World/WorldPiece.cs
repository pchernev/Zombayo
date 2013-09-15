using UnityEngine;
using System.Collections;

public class WorldPiece : MonoBehaviour {
	
	[HideInInspector]
	public int index;
	
	private static Color[] _colors = new Color[] { Color.red, Color.green, Color.blue };
	
	[HideInInspector]
	public Bounds bounds = new Bounds();
	bool didOne = false;
	bool found = false;
	
	// Use this for initialization
	void Start () {
		
		didOne = GetBoundWithChildren( transform, ref bounds, ref found );								
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDrawGizmos() 
	{
		if( didOne )
		{
			var p1 = bounds.min;
			var p2 = bounds.max;
			
			Gizmos.color = _colors[index & 11];
			Gizmos.DrawWireCube( (p1 + p2) / 2.0f, bounds.size );
		}
	}
	
	public Bounds getBounds()
	{
		didOne = GetBoundWithChildren( transform, ref bounds, ref found );	
		
		return bounds;
	}
	
	bool GetBoundWithChildren( Transform parent, ref Bounds pBound, ref bool initBound )
	{
		Bounds bound = new Bounds();
		bool didOne = false;
		
		if( parent.gameObject.renderer != null )
		{
			bound = parent.gameObject.renderer.bounds;
			if( initBound )
			{
				pBound.Encapsulate( bound.min );
				pBound.Encapsulate( bound.max );
			}
			else
			{
				pBound.min = new Vector3( bound.min.x, bound.min.y, bound.min.z );
				pBound.max = new Vector3( bound.max.x, bound.max.y, bound.max.z );
				initBound = true;
			}
			didOne = true;
		}
		
//		foreach( Transform child in parent )
//		{
//			if( GetBoundWithChildren( child, ref pBound, ref initBound ))
//				didOne = true;
//		}
		
		return didOne;
	}
	
}
