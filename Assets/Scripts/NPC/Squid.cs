using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Squid : BaseItem
{


	
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
		Debug.Log (_rigidbody.drag);

				
				
				
	}

	
	private bool wasHit = false;
	void OnTriggerEnter( Collider collision )
	{
	
		
		if( collision.gameObject.tag.CompareTo( "Player" ) == 0 )
		{
			_rigidbody.drag+=0.4f;
			gameObject.audio.Play();

			
			_animator.SetTrigger( "Hit" );

			wasHit = true;



		}
	}
	

	public override List<BaseItem> Spawn( GameObject wp)
	{
		var items = new List<BaseItem>();
		
		var positions = base.SpawnPositions( wp );
		foreach( var pos in positions )
		{
			var squid = Instantiate( this, pos, this.gameObject.transform.rotation ) as BaseItem;
			items.Add( squid );
		}
		
		return items;
	}
	
	
	
	
}