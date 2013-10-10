using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	Animator _animator;
	
	bool _isFlying;
	
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_isFlying = false;
	}
	
	// Update is called once per frame
	float time = 3;
	void FixedUpdate () {		
		time -= Time.deltaTime;
		
		Debug.Log( "deltaTime: " + Time.deltaTime );
		if( time <= 0 )
		{
			if( !_animator.IsInTransition( 0 ) )
			{
				if( _animator.GetInteger( "Idle" ) == 0 )
					_animator.SetInteger( "Idle", 2 );
				else 
					_animator.SetInteger( "Idle", 0 );
				
				time = Random.Range( 3, 10 );
			}
		}
	}
	
	void OnCollisionEnter( Collision collision )
	{
		Debug.Log( "Enter collision" );
		_isFlying = false;
		_animator.SetBool( "Fly", false );		
	}
	
	void OnCollisionExit( Collision collision )
	{
		Debug.Log( "Exit collision" );
		_isFlying = true;
		_animator.SetBool( "Fly", true );
	}	
}
