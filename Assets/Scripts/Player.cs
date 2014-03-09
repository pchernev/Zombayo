using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class Player : MonoBehaviour {	
	
	public Vector3 bounceForce;
	
	[HideInInspector]
	public float Speed;
	public GameObject explosion;
	public AudioClip collectCoins;
	
	private GUITexture _btnRestart;
	
	Animator _animator;
	
	private Vector3 startPos;
	private Quaternion startRotation;	
	
	bool _isFlying;
	public int hasBeenKicked = 0;

	
	public Statistics stat;
	//private DinamicStatistics Stat;
	private List<Vector3> prevPos;
	
	private Rigidbody rigidbody;
	
	#region Base player logic 

	void Awake() 
	{		
        SaveLoadGame sl = new SaveLoadGame();
        sl.LoadXML();
        stat = sl.DeserializeObject(sl._data);
        if (stat == null)
        {
            stat = new Statistics(0, 0);
        }
		rigidbody = GetComponent<Rigidbody>();
	}
	
	// Use for initialization
	void Start ()
	{
		
		
		
		_animator = GetComponent<Animator>();
		_isFlying = false;
		
		prevPos = new List<Vector3>();
		prevPos.Add( transform.position );
		Speed = 0;		
		
		_btnRestart = GameObject.FindWithTag( "RestartButton" ).guiTexture;
		_btnRestart.enabled = false;	
		
		startPos = transform.position;
		startRotation = transform.rotation;
	}
	
	void Update()
	{
		Vector3 newPos = transform.position;
		prevPos.Insert( 0, newPos );

		const int maxSize = 40;
		Speed = 0F;
		for( int i=0; i<prevPos.Count-1; i++ )
		{
			var p1 = prevPos[i];
			var p2 = prevPos[i+1];
			var s = Mathf.Abs( Vector3.Distance( p1, p2 ));
			if( s > Speed )
				Speed = s;
		}
		
		if( prevPos.Count > maxSize )
		{
			prevPos.RemoveAt( maxSize );
			
			//			if( Speed < 0.1F )
			//				rigidbody.isKinematic = true;
		}

        if (this.Speed == 0 && hasBeenKicked > 2 && Time.timeScale > 0.5f)
        {
            gameObject.SendMessage("EndGame");
        }
		
		if( this.Speed < 0.1f && hasBeenKicked >= 2 && !rigidbody.isKinematic )
		{
			rigidbody.isKinematic = true;
		}
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
	void OnCollisionEnter(Collision collision){

		if( collision.gameObject.tag.CompareTo( "Ground" ) == 0 )
		{
			_isFlying = false;
			_animator.SetBool( "Fly", false );	
			
			if( Speed > 0.1F )
				this.rigidbody.AddForce( bounceForce );
		}
		else 
		{
            if (this.stat == null)
            {
                this.stat = new Statistics(0,0);
            }
			this.stat.Points += 1;
		}
	}
	
	void OnCollisionExit( Collision collision )
	{
		_isFlying = true;
		hasBeenKicked++;
		_animator.SetBool( "Fly", true );
	}	
	
	
	void OnGUI()
	{
		
        //Stat["Distance: "] = transform.position.x;
        //Stat["Score: "] = this.stat.Points;
        //Stat["Coins: "] = Stat["Distance: "] * 1.5f;

		//UILabel scoresLabel = GameObject.Find("ScoreLabel").GetComponent<UILabel>();
		//scoresLabel.text = "Score: " + this.stat.Points.ToString();
		
		//UILabel distanceLabel = GameObject.Find("DistanceLabel").GetComponent<UILabel>();
		//distanceLabel.text = "Distance: " + transform.position.x.ToString("F");
		
		//GUI.Label( new Rect( 10.0f, 10.0f, 120.0f, 30.0f ), "Score: " + this.stat.Points.ToString() );
		
		//GUI.Label( new Rect(10F, 40F, 120F, 30F), "Speed: " + Speed.ToString() );
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
	
	public void Reset()
	{
		transform.position = startPos;
		transform.rotation = startRotation;
		hasBeenKicked = 0;		
		rigidbody.isKinematic = false;
	}

	#endregion
	
	#region NPC collisions

	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.CompareTag ("Coin"))
		   {
			Instantiate (explosion, transform.position, transform.rotation);
			AudioSource.PlayClipAtPoint(collectCoins, transform.position);
			Destroy(collider.gameObject);	
		
		}
	}

	
	private int test;
	
	#endregion
}
