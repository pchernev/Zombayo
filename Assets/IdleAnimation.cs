using UnityEngine;
using System.Collections;

public class IdleAnimation : MonoBehaviour {
	
	Animator _animator;
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown( KeyCode.A ))
		{
			_animator.SetInteger( "Idle", 1 );
			Debug.Log( "Idle 1" );
		}
		else if( Input.GetKeyDown( KeyCode.S ))
		{
			_animator.SetInteger( "Idle", 2 );
			Debug.Log( "Idle 2" );
		}
		else if( Input.GetKeyDown( KeyCode.D ))
		{
			_animator.SetInteger( "Idle", 0 );
			Debug.Log( "Idle 0" );
		}
	}
}
