using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class DrController : MonoBehaviour {

	private CharacterController _controller;
	private CollisionFlags c_collisionFlags;
	private Animator _animator;
	
	// animator values
	public float speed;
	public bool inKickZone = false;
	
	private float slowSpeed = 0.0f;
	
	public void SetKickZone( bool flag )
	{
		inKickZone = flag;
	}
	
	void Awake() 
	{
		_controller = GetComponent<CharacterController>();
	}
	
	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//var cameraTransform = Camera.main.transform;/
		//var moveDirection = cameraTransform.TransformDi/rection( Vector3.forward );
		var moveDirection = new Vector3( speed, 0.0f, 0.0f );
		moveDirection *= Time.deltaTime;
		
//		Debug.Log( "moveDirection: " + moveDirection.ToString() );
		
		//c_collis/ionFlags = controller.Move( new Vector3( 0.1f, 0F, 0F ));
		transform.Translate( moveDirection );
		
		// update speed
		if( inKickZone )
			slowSpeed = 0.05f;		
		speed -= slowSpeed;
		if( speed < 0.0f )
			speed = 0.0f;
		
		// update animator
    	UpdateAnimtorValues();		
	}
	
	void UpdateAnimtorValues()
	{
		_animator.SetFloat( "speed", speed );
		_animator.SetBool( "inKickZone", inKickZone );
	}
	
	void OnTriggerEnter( Collider collider )
	{
		Debug.Log( "OnTriggerEnter" );
		inKickZone = true;
	}
	
	void OnTriggerExit( Collider collider )
	{
		Debug.Log( "OnTriggerExit" );
		inKickZone = false;
	}
	
	void OnCollisionEnter( Collision collision )
	{
		Debug.Log( "drController->CollisionEnter: " );
	}
	
}
