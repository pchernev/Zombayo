using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	Animator _animator;
	
	bool _isFlying;
	
		
	private Statistics stat;
	
	void Awake() 
	{
		stat = new Statistics();
	}
	
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_isFlying = false;
	}
	
	// Update is called once per frame
	float time = 3;
	void FixedUpdate () {		
		time -= Time.deltaTime;
		
		if( time <= 0 )
		{
			if( !_animator.IsInTransition( 0 ) )
			{
				if( _animator.GetInteger( "Idle" ) == 0 )
					_animator.SetInteger( "Idle", 2 );
				else 
					_animator.SetInteger( "Idle", 0 );
				
				time = Random.Range( 3, 10 );
			}
		}
	}
	
	void OnCollisionEnter( Collision collision )
	{
		if( collision.gameObject.tag.CompareTo( "Ground" ) == 0 )
		{
			_isFlying = false;
			_animator.SetBool( "Fly", false );		
		}
		else 
		{
			this.stat.Points += 1;
		}
	}
	
	void OnCollisionExit( Collision collision )
	{
		Debug.Log( "Exit collision" );
		_isFlying = true;
		_animator.SetBool( "Fly", true );
	}	
	
	
	void OnGUI()
	{
		GUI.Label( new Rect( 10.0f, 10.0f, 120.0f, 30.0f ), "Score: " + this.stat.Points.ToString() );
//		EditorGUILayout.TextField ("File Name:", "test file name");
//		
//		if(GUILayout.Button(recordButton)) {
//			if(recording) { //recording
//				status = "Idle...";
//				recordButton = "Record";				
//				recording = false;
//			} else { // idle
//				capturedFrame = 0;
//				recordButton = "Stop";
//				recording = true;
//			}
//		}
//		EditorGUILayout.LabelField ("Status: ", status);
	}	
}
