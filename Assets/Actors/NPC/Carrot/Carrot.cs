using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	// Update is called once per frame
	void Update () {
		Debug.Log (_rigidbody.drag);
	}

	private bool wasHit = false;
	void OnCollisionEnter( Collision collision )
	{

		if (collision.gameObject.tag.CompareTo ("Player") == 0) {
			            
						wasHit = true;
						
			_rigidbody.drag = 0.1f;
						gameObject.audio.Play ();

						var collider = GetComponent<CapsuleCollider> ();
						collider.enabled = false;	
			
						_animator.SetTrigger ("Catch");
			
						_rigidbody.isKinematic = true;
			           
			
						// catch player
						_playerParent = _player.transform.parent;
						_player.transform.parent = throwBone;



						//_rigidbody.AddForce( this.force );
						//_rigidbody.velocity = Vector3.zero;


				} 


	}
	public void AddForceCarrot()
	{
		//Debug.Log( "ApplyForce: " + this.force.ToString() );
		
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
