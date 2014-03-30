using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class DrController : MonoBehaviour
{
	public float speed;
	public float slowSpeed;	
	public Vector3 startForce;	

	private CharacterController _controller;
	private CollisionFlags c_collisionFlags;
	private Animator _animator;
	
	private GameObject _player;
	
	private Vector3 startPos;
	private Quaternion startRotation;
	private float startSpeed;
	private Vector3 moveDirection;
		
	private bool inKickZone = false;
	
	private enum State { Idle, Run, Kick, AfterKick };
	private State state;
	
	void Awake() 
	{
		_controller = GetComponent<CharacterController>();
	}
	
	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();	
		_player = GameObject.FindWithTag( "Player" );
		
		startPos = transform.position;
		startRotation = transform.rotation;	
		startSpeed = speed;
		
		this.state = State.Idle;	
	}
	
	// Update is called once per frame
	void SetStartState()
	{
		this.state = State.Run;
	}

	private void UpdateIdleState()
	{
	
	}
	
	private void UpdateRunState()
	{
		transform.Translate( this.moveDirection );
	}
	
	private void UpdateKickState()
	{	
		transform.Translate( this.moveDirection );
			
		speed -= slowSpeed;
		if( speed < 0F )
			speed = 0F;
			
		if( speed < 2.5F )
		{			
			var rb = _player.GetComponent<Rigidbody>();
			rb.AddForce( startForce );	
			this.state = State.AfterKick;
		}
	}

	private void UpdateAfterKickState()
	{
		transform.Translate( this.moveDirection );

		speed -= slowSpeed; 
		if( speed < 0F )
			speed = 0F;
	}
		
	void Update ()
	{
		this.moveDirection = new Vector3( speed, 0.0f, 0.0f ) * Time.deltaTime;		
	
		switch( this.state )
		{
			case State.Idle: UpdateIdleState(); break;
			case State.Run: UpdateRunState(); break;
			case State.Kick: UpdateKickState(); break;
			case State.AfterKick: UpdateAfterKickState(); break;
		}
		
    	UpdateAnimtorValues();		
	}
	
	void OnTriggerEnter( Collider collider )
	{
		this.state = State.Kick;
		GetComponent<Rigidbody>().isKinematic = true;					
		gameObject.audio.Play();
	}
	
	void OnTriggerExit( Collider collider )
	{
		this.state = State.AfterKick;
	}
	
	void OnCollisionEnter( Collision collision )
	{
		Debug.Log( "drController->CollisionEnter: " );
	}
	
	public void Reset()
	{
		inKickZone = false;
		speed = startSpeed;
		slowSpeed = 0.0f;
		transform.position = startPos;
		transform.rotation = startRotation;
	}
	
	public void SetKickZone( bool flag )
	{
		inKickZone = flag;
	}
	
	#region Animator 
	
	void UpdateAnimtorValues()
	{
		_animator.SetFloat( "speed", speed );
		_animator.SetBool( "inKickZone", inKickZone && (speed > 0.0f));
	}
	
	#endregion
}
