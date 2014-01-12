using UnityEngine;
using System.Collections;

public class TriggerArea : MonoBehaviour {
	
	public DrController doctor;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter( Collider collider )
	{
		var obj = collider.gameObject;
		//Debug.Log( "obj.tag: " + obj.tag );
		if( obj.tag == "Doctor" )
			doctor.SetKickZone( true );
	}
	
	void OnTriggerExit( Collider collider )
	{
		var obj = collider.gameObject;
		if( obj.tag == "Doctor" )
			doctor.SetKickZone( false );
	}
	
	void OnCollisionEnter( Collision collision )
	{
		Debug.Log( "Trigger->OnCollisionEnter: " );
	}
}
