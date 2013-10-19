using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class DrController : MonoBehaviour {

	private CharacterController _controller;
	private CollisionFlags c_collisionFlags;
	private Animator _animator;
	
	// animator values
	private float speed = 0.0f;
	private bool inKickZone = false;
	
	void Awake() 
	{
		_controller = GetComponent<CharacterController>();
	}
	
	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();	
		
		speed = 0.2f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//var cameraTransform = Camera.main.transform;/
		//var moveDirection = cameraTransform.TransformDi/rection( Vector3.forward );
		var moveDirection = new Vector3( 0.01f, 0.0f, 0.0f );
//		moveDirection = moveDirection.normalized;
//		moveDirection *= Time.deltaTime;
		
////		Debug.Log( "moveDirection: " + moveDirection.ToString() );
//		
		
		//c_collis/ionFlags = controller.Move( new Vector3( 0.1f, 0F, 0F ));
		transform.Translate( moveDirection );
		
		// update animator
    	UpdateAnimtorValues();
	}
	
	void UpdateAnimtorValues()
	{
		_animator.SetFloat( "Speed", speed );
		_animator.SetBool( "inKickZone", inKickZone );
	}
	
	void OnTriggerEnter( Collider collider )
	{
		Debug.Log( "OnTriggerEnter" );
	}
	
	void OnTriggerExit( Collider collider )
	{
		Debug.Log( "OnTriggerExit" );
	}
	
}
