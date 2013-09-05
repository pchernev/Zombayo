using UnityEngine;
using UnityEditor;
using System.Collections;

public class GetBBox : ScriptableObject
{
	private static GameObject go;
	
	[MenuItem ("GameObject/Get Bounding Box")]
	
	static void MenuGetBound()
	{
		Bounds bound = new Bounds();
		bool didOne = false;
		bool found = false;
		
		didOne = GetBoundWithChildren( Selection.activeTransform, ref bound, ref found );
		if( didOne )
		{
			var msg = string.Format( "{0}\nCenter: {1}\nSzie: {2}", FormatBounds( bound ), bound.center.ToString(), bound.size.ToString() );
			EditorUtility.DisplayDialog( "Object Bounds", msg, "OK", "" );
		}
		else
		{
			EditorUtility.DisplayDialog( "Nothing renderable found", "OK", "" );
			Debug.Log( "Object " + Selection.activeTransform.name + " Nothing renderable found" );
		}
	}
	
	static bool GetBoundWithChildren( Transform parent, ref Bounds pBound, ref bool initBound )
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
	
	static string FormatBounds( Bounds b )
	{
		string bs = string.Format( "Min: ({0}, {1}, {2})\nMax: ({3}, {4}, {5})", b.min.x, b.min.y, b.min.z, b.max.x, b.max.y, b.max.z );
		return bs;
	}
}
