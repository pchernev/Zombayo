using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snail : BaseItem
{
	public float speed = 0.01f;
	public Vector3 force;
	public Transform throwBone;

	private Animator _animator;
	
	private Transform _playerParent;
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

	private bool wasHit = false;
	void OnCollisionEnter( Collision collision )
	{
//		if( wasHit )
//			return;

		if( collision.gameObject.tag.CompareTo( "Player" ) == 0 )
		{
			wasHit = true;

			var collider = GetComponent<BoxCollider>();
			collider.enabled = false;	

			_animator.SetTrigger( "GrabThrow" );

			_rigidbody.isKinematic = true;
//			_rigidbody.AddForce( this.force );

			// catch player
			_playerParent = _player.transform.parent;
			_player.transform.parent = throwBone;
		}
	}

	public void ApplyForce()
	{
		Debug.Log( "ApplyForce: " + this.force.ToString() );

		_player.transform.parent = _playerParent;
		_rigidbody.isKinematic = false;
		_rigidbody.AddRelativeForce( this.force );
	}

	public override List<BaseItem> Spawn( GameObject wp )
	{
		var items = new List<BaseItem>();

		var positions = base.SpawnPositions( wp );
		foreach( var pos in positions )
		{
			var snail = Instantiate( this, pos, this.gameObject.transform.rotation ) as BaseItem;
			items.Add( snail );
		}

		return items;
	}
}
