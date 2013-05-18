using UnityEngine;
using System.Collections;
using System;

public class MouseInput : MonoBehaviour
{
	static bool _mouseDown = false;
	
	static float _startTime;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( Input.GetMouseButton(0) )
		{
			if( !_mouseDown )
				_startTime = Time.time;			
			_mouseDown = true;
		}
		else if( _mouseDown ) 
		{
			_mouseDown = false;
			var dt = Time.time - _startTime;
//			Debug.Log( "dtime: " + dt );
		
//			Component camera = GetComponent("Camera");

			if( camera != null )
			{
				var ray = camera.ScreenPointToRay( Input.mousePosition );
				RaycastHit hit;
				if( Physics.Raycast( ray, out hit, 1000 ))
				{
					if( hit.rigidbody != null )
					{
						var force = new Vector3( (550 + dt * 3000.0f), 100.0f + dt * 3000.0f, 0.0f );
//						hit.rigidbody.AddForce( force );
					}
				}
			}
		}	
	}
}
