using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;


public class Player : MonoBehaviour {	
	public float timeToReach;

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

	
	public Statistics stat = new Statistics();
    

	private List<Vector3> prevPos;	
	private Rigidbody rigidbody;
    private GameManager gm;

	#region Base player logic 

	void Awake() 
	{
        gm = gameObject.GetComponent<GameManager>();
		rigidbody = GetComponent<Rigidbody>();
       _animator = GetComponent<Animator>();
       this.stat = SaveLoadGame.LoadStats();
	}

	// Use for initialization
	void Start ()
	{		
		_animator = GetComponent<Animator>();
		_isFlying = false;
		
		prevPos = new List<Vector3>();
		prevPos.Add( transform.position );
		Speed = 0;
      
		//_btnRestart = GameObject.FindWithTag( "RestartButton" ).guiTexture;
        //_btnRestart.enabled = false;	
		
		startPos = transform.position;
		startRotation = transform.rotation;
	}
	
	void Update()
	{
		Vector3 newPos = transform.position;
		prevPos.Insert( 0, newPos );

		const int maxSize = 40;
		Speed = 0F;
		if (explosion.transform.parent = gameObject.transform) {
			explosion.transform.position = this.gameObject.transform.position;
				}
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

        if (!gm.isGamePaused)
        {
            StartCoroutine(WaitAndEndGame(0.7f));
        }

		if( this.Speed < 0.1f && hasBeenKicked >= 2 && !rigidbody.isKinematic )
		{
			rigidbody.isKinematic = true;
		}
	}
    IEnumerator WaitAndEndGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (this.Speed == 0 && hasBeenKicked > 0)
        {
            gameObject.SendMessage("EndGame");
        }
    }
	// Update is called once per frame
	float time = 3;
	void FixedUpdate () {
   
		time -= Time.deltaTime;
        //this.stat.Distance = (int)transform.position.x;

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
            if (this.stat != null)
            {
                this.stat.Points += 1;
            }			
		}
	}
	
	void OnCollisionExit( Collision collision )
	{
		_isFlying = true;
		hasBeenKicked++;
		_animator.SetBool( "Fly", true );
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



			
			var explosion1 = Instantiate (explosion, transform.position, transform.rotation) as GameObject ;
			explosion1.transform.parent = gameObject.transform;
			
			
			iTween.MoveTo (explosion1, gameObject.transform.position, 2.0f);
			AudioSource.PlayClipAtPoint(collectCoins, transform.position);
			Destroy (collider.gameObject);


		}
	}

	

	
	private int test;
	
	#endregion

}
