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

    public int hasBeenKicked = 0;
    [HideInInspector]
    public float LastHeightY;
    [HideInInspector]
    public int TimesHitGround = 0;
    ProgressBarButtonWing wingsBtn;
    ProgressBarButtonFart fartBtn;
    BladderButton bladderBtn;

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
        prevPos = new List<Vector3>();
        prevPos.Add(transform.position);
        Speed = 0;
        startPos = transform.position;
        startRotation = transform.rotation;

        wingsBtn = GameObject.Find("Ingame Button Glide").GetComponent<ProgressBarButtonWing>();
        fartBtn = GameObject.Find("Ingame Button Fart").GetComponent<ProgressBarButtonFart>();
        bladderBtn = GameObject.Find("Ingame Button BubbleGum").GetComponent<BladderButton>();
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
        ExecuteAnimations();
        time -= Time.deltaTime;
    }

    void LateUpdate()
    {

        if ((rigidbody.isKinematic == true) || (this.Speed <= 0.15f && transform.position.y <= 0.40f && !gm.isGamePaused && hasBeenKicked >= 2))
            StartCoroutine(WaitAndEndGame(2.0f));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "_TerrainPlane")
        {
            TimesHitGround++;
            Debug.Log("GROUNDDDDDDDDD");
            if (hasBeenKicked >= 1)
            {
                _animator.SetBool("IsInHurt", true);
                switch (TimesHitGround)
                {
                    case 1: _animator.SetFloat("HurtState", 0.0f);
                        break;
                    case 2: _animator.SetFloat("HurtState", 1.0f);
                        break;
                    default:
                        break;
                }
            }
           
           
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
        hasBeenKicked++;
        gameObject.collider.material = initialMaterial;
        _animator.SetBool("IsInIdle", false);
    }

    private void ExecuteAnimations()
    {
        if (fartBtn.use || wingsBtn.use || bladderBtn.usedBubbleGum)
        {
            _animator.SetBool("IsInUpgrade", true);
            _animator.SetBool("IsInHurt", false);
            TimesHitGround = 0;
        }
        else
            _animator.SetBool("IsInUpgrade", false);

        if (bladderBtn.usedBubbleGum)
        {
            _animator.SetFloat("UpdateState", 0.0f);
        }
        else if (fartBtn.use)
            _animator.SetFloat("UpdateState", 0.5f);
        else if (wingsBtn.use)
            _animator.SetFloat("UpdateState", 1.0f);
       
        if (!_animator.GetBool("IsInUpgrade") && LastHeightY < transform.position.y)
        {
            // flyuppp
            _animator.SetFloat("FlyingState", 0.0f);
        }
        else if (!_animator.GetBool("IsInUpgrade") && LastHeightY > transform.position.y)
        {
            // flydownnn
            _animator.SetFloat("FlyingState", 1.0f);
        }
        LastHeightY = transform.localPosition.y;
    }

    public IEnumerator WaitAndEndGame(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);

        if ((int)this.Speed == 0 && hasBeenKicked > 0)
        {
            rigidbody.isKinematic = true;
            _animator.SetBool("IsInUpgrade", false);
            _animator.SetBool("IsGameOver", true);
            _animator.SetBool("IsInIdle", true);
            gameObject.SendMessage("EndGame");
        }
    }

    public void Reset()
    {
        transform.position = startPos;
        transform.rotation = startRotation;
        hasBeenKicked = 0;
        rigidbody.isKinematic = false;
        _animator.SetBool("IsInUpgrade", false);
        _animator.SetBool("IsGameOver", false);
        _animator.SetBool("IsInIdle", true);
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

//public enum RabbitAnimationState
//{
//    Idle = 0,
//    FlyUp = -1,
//    FlyLeveled = -2,
//    FlyDown = -3,

//    PowerUpFart = 4,
//    PowerUpWings = 5,
//    PowerUpBubbleGum = 6
//}
