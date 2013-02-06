using UnityEngine;
using System.Collections;

public class StartForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log( "Start" );
	
		var force = new Vector3( 0.0f, 200.0f, -600.0f );
		this.rigidbody.AddForce( force );
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log( "Update" );
	}
	
	void OnCollisionEnter(Collision collision)
	{
		Debug.Log( collision.gameObject.name );	
	}
}
