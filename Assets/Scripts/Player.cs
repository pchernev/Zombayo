using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public float Speed;
    [HideInInspector]
    public float ArmorCount = 0;
    public float timeToReach;


    public Vector3 bounceForce;
    public GameObject explosion;
    public AudioClip collectCoins;
	public PhysicMaterial initialMaterial;
    private Vector3 startPos;
    private Quaternion startRotation;
    private List<Vector3> prevPos;
    private Rigidbody rigidbody;

    private Animator _animator;
    private GameManager gm;
    public GameData gameData;


    #region AnimationVariables

    private int velocityDirection = (int)RabbitAnimationState.Idle;
    
    bool _isFlying;
    public int hasBeenKicked = 0;
    [HideInInspector]
    public float LastHeightY;
    [HideInInspector]
    public int TimesHitGround = 0;
    ProgressBarButtonWing wingsBtn;
    ProgressBarButtonFart fartBtn;
    #endregion

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

    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _isFlying = false;
        prevPos = new List<Vector3>();
        prevPos.Add(transform.position);
        Speed = 0;
        startPos = transform.position;
        startRotation = transform.rotation;

        wingsBtn = GameObject.Find("Ingame Button Glide").GetComponent<ProgressBarButtonWing>();
        fartBtn = GameObject.Find("Ingame Button Fart").GetComponent<ProgressBarButtonFart>();
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
            prevPos.RemoveAt(maxSize);
    }

    float time = 3;
    void FixedUpdate()
    {
        time -= Time.deltaTime;
    }

    void LateUpdate()
    {
        ExecuteAnimations();
        if ((rigidbody.isKinematic == true) || (this.Speed <= 0.15f && transform.position.y <= 0.40f && !gm.isGamePaused && hasBeenKicked >= 2))
            StartCoroutine(WaitAndEndGame(2.0f));        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.CompareTo("Ground") == 0 && _isFlying)
        {
            _isFlying = false;
            TimesHitGround++;
            // todo:  here we set the animation to "hit the ground and in pain"
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
		gameObject.collider.material = initialMaterial;
    }

    private void ExecuteAnimations() 
    {
        if (time <= 0)
        {
            if (!_animator.IsInTransition(0) && velocityDirection != (int)RabbitAnimationState.Idle)
            {
                if (_animator.GetInteger("Idle") == 0)
                    _animator.SetInteger("Idle", 2);
                else
                    _animator.SetInteger("Idle", 0);

                time = Random.Range(3, 10);
            }
        }

        _animator.SetBool("Fly", _isFlying);
        if (gm.isGameOver && velocityDirection != (int)RabbitAnimationState.Idle)
        {
            velocityDirection = (int)RabbitAnimationState.Idle;
        }

        if (TimesHitGround > 0 && velocityDirection != (int)RabbitAnimationState.Idle)
            velocityDirection = (int)RabbitAnimationState.Idle;
            // for leveled fly animation
        //else if (_isFlying && LastHeightY <= 18f && LastHeightY >= 15f && TimesHitGround == 0 && this.velocityDirection != 0)
        //{
        //    this.velocityDirection = 0;
        //    _animator.SetInteger("VelocityDirection", this.velocityDirection);
        //}
        else if (_isFlying && LastHeightY < transform.position.y && velocityDirection != (int)RabbitAnimationState.FlyUp)
            velocityDirection = (int)RabbitAnimationState.FlyUp;
        else if (_isFlying && LastHeightY > transform.position.y && velocityDirection != (int)RabbitAnimationState.FlyDown)
            velocityDirection = (int)RabbitAnimationState.FlyDown;

        if (_isFlying && fartBtn.use && velocityDirection != (int)RabbitAnimationState.PowerUpFart)
            velocityDirection = (int)RabbitAnimationState.PowerUpFart;

        if (_isFlying && wingsBtn.use && velocityDirection != (int)RabbitAnimationState.PowerUpWings)
            velocityDirection = (int)RabbitAnimationState.PowerUpWings;

        _animator.SetInteger("VelocityDirection", velocityDirection);

        LastHeightY = transform.localPosition.y;
    }

    public IEnumerator WaitAndEndGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        if ((int)this.Speed == 0 && hasBeenKicked > 0)
        {
            rigidbody.isKinematic = true;
           
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

public enum RabbitAnimationState 
{
    Idle = 0,
    FlyUp = -1,
    FlyLeveled = -2,
    FlyDown = -3,

    PowerUpFart = 4,
    PowerUpWings = 5,
    PowerUpBubbleGum = 6
}
