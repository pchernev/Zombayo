using UnityEngine;
using System.Collections;

public class WorldPiece : MonoBehaviour {
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDrawGizmos() 
	{
		Bounds bound = new Bounds();
		bool didOne = false;
		bool found = false;
		
		didOne = GetBoundWithChildren( transform, ref bound, ref found );						
		if( didOne )
		{
			iTween.DrawLineGizmos( new Vector3[] { bound.min, bound.max }, Color.red );
		}
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
		
		foreach( Transform child in parent )
		{
			if( GetBoundWithChildren( child, ref pBound, ref initBound ))
				didOne = true;
		}
		
		return didOne;
	}
	
}
