using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Carrot : BaseItem {


	public Transform throwBone;
	public Vector3 force;

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

	private bool wasHit = false;
	void OnTriggerEnter( Collider collision )
	{
		if (collision.gameObject.tag.CompareTo ("Player") == 0 && _player.GetComponent<Player>().ArmorCount <= 0) 
        {		    
			wasHit = true;			
		    _rigidbody.drag = 0.1f;
			gameObject.audio.Play ();
			var collider = GetComponent<CapsuleCollider> ();
			collider.enabled = false;			
			_animator.SetInteger("Catch", 1);
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			_rigidbody.useGravity = true;
			_rigidbody.drag = 10f;
        }
        else if (collision.gameObject.tag.CompareTo ("Player") == 0 && _player.GetComponent<Player>().ArmorCount > 0)
        {

            _player.GetComponent<Player>().ArmorCount--;
            collider.enabled = false;
            wasHit = true;
            gameObject.audio.Play();
            _animator.SetInteger("Catch", -1);
			_rigidbody.AddForce(this.force);
            Destroy(transform.gameObject);
        }
	}
	public void AddForceCarrot()
	{
		_player.transform.parent = _playerParent;
		_rigidbody.isKinematic = false;
		_rigidbody.AddForce( force );
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
