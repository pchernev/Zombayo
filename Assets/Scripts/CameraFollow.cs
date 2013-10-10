using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Vector3 cameraOffset = new Vector3(0,0,0) ;
	public Vector3 cameraRotaion = new Vector3(0,0,0) ;
	// Use this for initialization
	void Start () 
	{
		var p = transform.position;
		Camera.main.transform.position = new Vector3( p.x + cameraOffset.x, p.y + cameraOffset.y, p.z + cameraOffset.z );			
		Camera.main.transform.eulerAngles = cameraRotaion;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		var p = transform.position;
		Camera.main.transform.position = new Vector3( p.x + cameraOffset.x, p.y + cameraOffset.y, p.z + cameraOffset.z );			
		Camera.main.transform.eulerAngles = cameraRotaion;
	}
	
	// 0.24, 0.98, -3.07
	// 3.37, 7.89, 0

}
