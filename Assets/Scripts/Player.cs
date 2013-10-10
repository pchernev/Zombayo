using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	Animator _animator;
	
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
				
	}
	
	
	void OnCollisionEnter( Collision collision )
	{
		Debug.Log( "Enter collision" );
		_animator.SetBool( "Fly", false );		
	}
	
	void OnCollisionExit( Collision collision )
	{
		Debug.Log( "Exit collision" );
		_animator.SetBool( "Fly", true );
	}	
}
