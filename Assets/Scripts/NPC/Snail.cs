using UnityEngine;
using System.Collections;

public class Snail : BaseItem
{
	public float speed = 0.01f;
	public Vector3 force;

	private Animator _animator;

	private GameObject _player;
	private Rigidbody _rigidbody;
	
	void Start ()
	{
		_animator = GetComponent<Animator>();

		_player = GameObject.FindWithTag( "Player" );
		if( _player != null )
		{
			_rigidbody = _player.GetComponent<Rigidbody>();
		}
		else
		{
			Debug.Log( "Player object not found" );
		}
	}
	
	void Update ()
	{
		var pos = transform.position;
		pos.x -= speed;
		transform.position = pos;
	}

	void OnCollisionEnter( Collision collision )
	{
		if( collision.gameObject.tag.CompareTo( "Player" ) == 0 )
		{
			var collider = GetComponent<BoxCollider>();
			collider.enabled = false;	

			_animator.SetTrigger( "GrabThrow" );

			_rigidbody.isKinematic = true;
//			_rigidbody.AddForce( this.force );
		}
	}

	public void ApplyForce()
	{
		Debug.Log( "ApplyForce: " + this.force.ToString() );

		_rigidbody.isKinematic = false;
		_rigidbody.AddForce( this.force );
	}
}
