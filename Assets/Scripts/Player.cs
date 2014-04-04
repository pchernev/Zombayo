using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;

public class Player : MonoBehaviour
{
    public float timeToReach;
    ProgressBarButtonWing wingsBtn;
    public Vector3 bounceForce;
    // Upgrades
    [HideInInspector]
    public float ArmorCount = 0;

    [HideInInspector]
    public float Speed;
    public GameObject explosion;
    public AudioClip collectCoins;
	public PhysicMaterial initialMaterial;
    int velocityDirection = -999;
    private GUITexture _btnRestart;

    Animator _animator;

    private Vector3 startPos;
    private Quaternion startRotation;

    bool _isFlying;
    public int hasBeenKicked = 0;

    public GameData gameData;

    [HideInInspector]
    public float LastHeightY;
    [HideInInspector]
    public int TimesHitGround = 0;


    private List<Vector3> prevPos;
    private Rigidbody rigidbody;
    private GameManager gm;

    #region Base player logic

    void Awake()
    {
        gm = gameObject.GetComponent<GameManager>();
        rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        this.gameData = SaveLoadGame.LoadData();
        var armorItem = this.gameData.ShopItems.FirstOrDefault(x => x.Name == "Armor");
        int armorUpgCount = armorItem.UpgradesCount;
        this.ArmorCount = armorItem.Values[armorUpgCount];

        wingsBtn = GameObject.Find("Ingame Button Glide").GetComponent<ProgressBarButtonWing>();
    }

    // Use for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _isFlying = false;

        prevPos = new List<Vector3>();
        prevPos.Add(transform.position);
        Speed = 0;

        //_btnRestart = GameObject.FindWithTag( "RestartButton" ).guiTexture;
        //_btnRestart.enabled = false;	

        startPos = transform.position;
        startRotation = transform.rotation;
    }
    void LateUpdate() 
    {
        if (TimesHitGround > 0 && velocityDirection != -3)
        {
            Debug.Log("Go In IDLE STATE");
            velocityDirection = -3;
            _animator.SetInteger("VelocityDirection", velocityDirection);
        }
        else if (_isFlying && LastHeightY <= 18f && LastHeightY >= 15f && TimesHitGround == 0 && this.velocityDirection != 0)
        {
            Debug.Log("Leveledddddddddd");
            this.velocityDirection = 0;
            _animator.SetInteger("VelocityDirection", this.velocityDirection);
        }
        else if (_isFlying && LastHeightY < transform.position.y && TimesHitGround == 0 && velocityDirection != 1)
        {
            Debug.Log("Uppppppp");
            velocityDirection = 1;
            _animator.SetInteger("VelocityDirection", velocityDirection);
        }
        else if (_isFlying && LastHeightY > transform.position.y && TimesHitGround == 0 && velocityDirection != -1)
        {
            Debug.Log("Downnnnnnn");
            velocityDirection = -1;
            _animator.SetInteger("VelocityDirection", velocityDirection);
        }

        LastHeightY = transform.localPosition.y;
    }
    void Update()
    {
       

        Vector3 newPos = transform.position;
        prevPos.Insert(0, newPos);

        const int maxSize = 40;
        Speed = 0F;

        for (int i = 0; i < prevPos.Count - 1; i++)
        {
            var p1 = prevPos[i];
            var p2 = prevPos[i + 1];
            var s = Mathf.Abs(Vector3.Distance(p1, p2));
            if (s > Speed)
                Speed = s;
        }

        if (prevPos.Count > maxSize)
        {
            prevPos.RemoveAt(maxSize);

            //			if( Speed < 0.1F )
            //				rigidbody.isKinematic = true;
        }

        if (!gm.isGamePaused)
        {
           
            StartCoroutine(WaitAndEndGame(0.7f));
        }

        if (this.Speed < 0.1f && hasBeenKicked >= 2 && !rigidbody.isKinematic)
        {
            rigidbody.isKinematic = true;
        }
    }

    float time = 3;
    void FixedUpdate()
    {
        //Debug.Log("Gravity: " + Physics.gravity);
        //if (wingsBtn.available && wingsBtn.use && _isFlying)
        //{
        //    _animator.SetInteger("Idle", 3); // integer to play using wings animation
        //    Debug.Log("USING WINGS");
        //    Physics.gravity = new Vector3(0, -0.01f, 0);
        //}
        //else
        //{
        //    _animator.SetInteger("Idle", -1); // integer to play using wings animation
        //    Debug.Log("NOT USING WINGS");
        //    Physics.gravity = new Vector3(0, -9.8f, 0);
        //}
        time -= Time.deltaTime;
        //Debug.Log("Rabbit Last Local Heght: " + LastHeightY);

        //Debug.Log("Times Hited the Ground: " + TimesHitGround);

        if (time <= 0)
        {
            if (!_animator.IsInTransition(0))
            {
                if (_animator.GetInteger("Idle") == 0)
                    _animator.SetInteger("Idle", 2);
                else
                    _animator.SetInteger("Idle", 0);

                time = Random.Range(3, 10);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {

			
        if (collision.gameObject.tag.CompareTo("Ground") == 0 && _isFlying)
        {
            _isFlying = false;
            TimesHitGround++;
            _animator.SetBool("Fly", false);

            // here we set the animation to "hit the ground and in pain"


            //if (Speed > 0.1F)
            //{
            //    this.rigidbody.AddForce(bounceForce);
            //}
        }
        else
        {
            if (this.gameData != null)
            {
                this.gameData.PlayerStats.Points += 1;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        _isFlying = true;
        hasBeenKicked++;
        _animator.SetBool("Fly", true);
		gameObject.collider.material = initialMaterial;
    }

    IEnumerator WaitAndEndGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (this.Speed == 0 && hasBeenKicked > 0)
        {
            if (velocityDirection != -3)
            {
                _isFlying = false;
                _animator.SetBool("Fly", _isFlying);
                velocityDirection = -3;
                _animator.SetInteger("VelocityDirection", velocityDirection);           
            }
          
            gameObject.SendMessage("EndGame");
        }
    }

    public void Reset()
    {
        transform.position = startPos;
        transform.rotation = startRotation;
        hasBeenKicked = 0;
        rigidbody.isKinematic = false;
    }

    public void UseFart()
    {
        Debug.Log("use farttttttt");
    }

    #endregion

    #region NPC collisions

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Coin"))
        {
            var explosion1 = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
            explosion1.transform.parent = gameObject.transform;

            AudioSource.PlayClipAtPoint(collectCoins, transform.position);
            Destroy(collider.gameObject);
            Destroy(explosion1, 2.0f);

        }
    }

    #endregion

}
