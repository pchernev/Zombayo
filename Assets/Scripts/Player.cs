using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
  private enum AnimDesc {
    Idle,
    Fly,
    Hurt,
    Drag,
    PowerUpBubble,
    PowerUpFart,
    PowerUpGlide,
    Count
  }

  private static readonly string[] animDescriptions;
  private string[] getDescriptionTexts() { return animDescriptions; }

  public enum State
  {
    None,
    Hurt,
    Drag,
    PowerUpBubble,
    PowerUpFart,
    PowerUpGlide
  }

  public CoinMagnet magnet;
  [HideInInspector]
  public AnimatorUtils animUtils;

  private Vector3 startPos;
  private Quaternion startRot;
  private bool isKicked;
  private Animator animator;
  private int idleIndex;
  private int hurtIndex;
  private Vector3 lastPos;
  private Vector3 speed;
  private float bubbleStartTime;
  private float originalDrag;
  private float originalBounce;

  private State currentState = State.None;
  public bool IsBubbling { get { return currentState == State.PowerUpBubble; } }
  public bool IsFarting { get { return currentState == State.PowerUpFart; } }
  public bool IsGliding { get { return currentState == State.PowerUpGlide; } }
  public bool CanGlide { get { return currentState != State.Drag; } }
  public bool IsTooSlow { get { return transform.position.y < 1 && rigidbody.velocity.sqrMagnitude < 5; } }
  public bool IsAboveDragThreshold { get { return rigidbody.velocity.y > data.dragThreshold; } }
  public float CurrentKickEfficiency { get { return animator.GetFloat("KickEfficiency"); } }
  [HideInInspector]
  public bool UpdateKickEfficiency;

  private GameData data;

  static Player()
  {
    animDescriptions = new string[(int)AnimDesc.Count];
    for (int i = 0; i < (int)AnimDesc.Count; ++i)
      animDescriptions[i] = ((AnimDesc) i).ToString();
  }


  void Awake()
  {
    animator = GetComponent<Animator>();
    idleIndex = 0;

    lastPos = startPos = transform.position;
    startRot = transform.rotation;
  }

  void Start()
  {
    data = GameLogic.Instance.gameData;
  }

  void FixedUpdate()
  {
    speed = transform.position - lastPos;
    lastPos = transform.position;
    data.travelledDistance = lastPos.x - startPos.x;
    data.currentHeight = (int)lastPos.y;
    if (UpdateKickEfficiency)
      data.kickEfficiency = CurrentKickEfficiency;

    if (data.IsPlayerKicked) {
      animator.SetFloat("VerticalSpeed", 2*speed.y);

      switch (currentState) {
        case State.PowerUpBubble:
          if (bubbleStartTime + data.bubbleGumLife < Time.time)
            enterState(State.None);
          break;

        case State.PowerUpFart:
          rigidbody.AddForce(data.specs.fartForce * data.fartDirection.normalized);
          break;

        case State.PowerUpGlide:
          rigidbody.AddForce(data.specs.glideForce * data.glideDirection.normalized);
          break;

        case State.Drag:
          rigidbody.AddForce(data.dragForce * Vector3.left);
          break;
      }
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    if (!data.IsPlayerKicked)
      return;

    if (collision.gameObject.layer == Layers.Terrain)
      CollisionLogic.OnGroundHit();
  }

  void OnMouseUpAsButton()
  {
    if (!data.IsPlayerKicked)
      GameLogic.Instance.initiateKick();
  }


  void OnIdleFinishing()
  {
    idleIndex = animUtils.crossFadeRandomEntry((int)AnimDesc.Idle, 0.05f, idleIndex);
  }

  public virtual void ensureDataConsistency()
  {
    animUtils = GetComponent<AnimatorUtils>() ?? gameObject.AddComponent<AnimatorUtils>();
    animUtils.setTextsDelegate(getDescriptionTexts);
    animUtils.DescriptionEditingAllowed = false;
  }

  public void prepareForStart()
  {
    moveToStartSpot();
    rigidbody.isKinematic = false;
    rigidbody.velocity = Vector3.zero;
    rigidbody.angularVelocity = Vector3.zero;
    rigidbody.isKinematic = false;
    magnet.gameObject.SetActive(false);
    animUtils.playEntry((int)AnimDesc.Idle, idleIndex);
    UpdateKickEfficiency = true;
  }

  public void moveToStartSpot()
  {
    transform.position = startPos;
    transform.rotation = startRot;
  }

  public void kickRabbit(Vector3 kickForce)
  {
    enterState(State.None);
    magnet.gameObject.SetActive(true);
    rigidbody.AddForce(kickForce);
  }

  public void enterState(State state)
  {
    exitCurrentState();

    switch (state) {
      case State.None:
        animUtils.crossFadeRandomEntry((int)AnimDesc.Fly, 0.05f);
        break;

      case State.Hurt:
        hurtIndex = animUtils.crossFadeRandomEntry((int)AnimDesc.Hurt, 0.05f, hurtIndex);
        setRotationZConstraint(false);
        break;

      case State.Drag:
        animUtils.crossFadeRandomEntry((int)AnimDesc.Drag, 0.05f);
        iTween.RotateTo(gameObject, Vector3.zero, 0.25f);
        setRotationZConstraint(true);
        setPositionYConstraint(true);
        break;

      case State.PowerUpBubble:
        originalDrag = rigidbody.drag;
        originalBounce = collider.material.bounciness;
        rigidbody.drag = 0;
        collider.material.bounciness = 1;
        animUtils.crossFadeEntry((int)AnimDesc.PowerUpBubble, 0, 0.05f);
        bubbleStartTime = Time.time;
        break;

      case State.PowerUpFart:
        animUtils.crossFadeEntry((int)AnimDesc.PowerUpFart, 0, 0.05f);
        if (rigidbody.velocity.y < 0)
          rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        break;

      case State.PowerUpGlide:
		    animUtils.crossFadeEntry((int)AnimDesc.PowerUpGlide, 0, 0.05f);
        setPositionYConstraint(true);
        break;
    }

    currentState = state;
    iTween.RotateTo(gameObject, Vector3.zero, 0.25f);
    setRotationZConstraint(true);
    animator.SetInteger("PowerUp", (int)currentState);
  }

  private void exitCurrentState()
  {
    rigidbody.drag = originalDrag;
    collider.material.bounciness = originalBounce;
    setPositionYConstraint(false);
    setRotationZConstraint(false);
  }

  private void setRotationZConstraint(bool freeze)
  {
    if (freeze)
      rigidbody.constraints |= RigidbodyConstraints.FreezeRotationZ;
    else
      rigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
  }

  private void setPositionYConstraint(bool freeze)
  {
    if (freeze)
      rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
    else
      rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
  }
}
