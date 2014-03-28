﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dolphin : BaseItem
{
	public float speed = 0.02f;
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
	void OnTriggerEnter( Collider collision )
	{

		if( collision.gameObject.tag.CompareTo( "Player" ) == 0 )
		{
			Debug.Log ("The player hitting dolphin");
			_rigidbody.drag = 0.1f;
			wasHit = true;
			gameObject.audio.Play();

			
			_animator.SetTrigger( "Throw" );
			_rigidbody.AddForce( force );
			Debug.Log( "ApplyForce: " + this.force.ToString() );
			
		

		}
	}
	


	public override List<BaseItem> Spawn( GameObject wp)
	{
		var items = new List<BaseItem>();
		
		var positions = base.SpawnPositions( wp );
		foreach( var pos in positions )
		{
			var dolphin = Instantiate( this, pos, this.gameObject.transform.rotation ) as BaseItem;
			items.Add( dolphin );
		}
		
		return items;
	}

}
