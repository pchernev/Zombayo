using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Carrot : BaseItem {



	private Animator _animator;

	private Transform _playerParent;
	private GameObject _player;
	private Rigidbody _rigidbody;

	void Start () {
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
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter( Collision collision )
	{

		if (collision.gameObject.tag.CompareTo ("Player") == 0) {
			Debug.Log ("The player hitting a carrot");
			gameObject.audio.Play();
			collision.gameObject.rigidbody.velocity = Vector3.zero;
		
			var collider = GetComponent<CapsuleCollider>();
			collider.enabled = false;	
			
			_animator.SetTrigger( "Catch" );
			
			_rigidbody.isKinematic = true;
			//			_rigidbody.AddForce( this.force );
			
			// catch player
			_playerParent = _player.transform.parent;


				}

	}
	public override List<BaseItem> Spawn( GameObject wp )
	{
		var items = new List<BaseItem>();

		
		var positions = base.SpawnPositions (wp);
		for(int i = 0;i<positions.Count;i++)
		{


			var carrot = Instantiate( this,positions[i], this.gameObject.transform.rotation ) as BaseItem;
			
			items.Add( carrot );

			
		}
		
		return items;
	}
}
