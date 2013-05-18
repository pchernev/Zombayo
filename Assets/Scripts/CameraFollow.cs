using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		var p = transform.position;
		Camera.main.transform.position = new Vector3( p.x, p.y, Camera.main.transform.position.z );			
	}
	
	// Update is called once per frame
	void Update ()
	{
		var p = transform.position;
		Camera.main.transform.position = new Vector3( p.x, p.y, Camera.main.transform.position.z );			
	}
}
