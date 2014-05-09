using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
  private enum AnimDesc {
    Idle,
    Fly,
    Hurt,
    Drag,
    PowerUp,
    Count
  }

  private static readonly string[] animDescriptions;
  private string[] getDescriptionTexts() { return animDescriptions; }

  public enum PowerUpState
  {
    None,
    BubbleGum,
    Fart,
    Glide
  }

  public CoinMagnet magnet;
  [HideInInspector]
  public AnimatorUtils animUtils;

  private Vector3 startPos;
  private Quaternion startRot;
  private bool isKicked;
  private bool isHurt;
  private bool isDragging;
  private Animator animator;
  private int idleIndex;
  private int hurtIndex;
  private Vector3 lastPos;
  private Vector3 speed;
  private float bubbleStartTime;
  private float originalDrag;
  private float originalBounce;

  private PowerUpState currentPowerUp = PowerUpState.None;
  public bool IsBubbling { get { return currentPowerUp == PowerUpState.BubbleGum; } }
  public bool IsFarting { get { return currentPowerUp == PowerUpState.Fart; } }
  public bool IsGliding { get { return currentPowerUp == PowerUpState.Glide; } }
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

      if (IsBubbling)
        if (bubbleStartTime + data.bubbleGumLife < Time.time)
          enterPowerUpState(PowerUpState.None);

      if (IsFarting)
        rigidbody.AddForce(data.specs.fartForce * data.fartDirection.normalized);

      if (IsGliding)
        rigidbody.AddForce(data.specs.glideForce * data.glideDirection.normalized);

      if (isDragging)
        rigidbody.AddForce(data.dragForce * Vector3.left);
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
    enterPowerUpState(PowerUpState.None);
    magnet.gameObject.SetActive(true);
    rigidbody.AddForce(kickForce);
  }

  public void enterHurtState()
  {
    hurtIndex = animUtils.crossFadeRandomEntry((int)AnimDesc.Hurt, 0.05f, hurtIndex);
    setRotationZConstraint(false);

    isHurt = true;
  }

  public void enterDragState()
  {
    animUtils.crossFadeRandomEntry((int)AnimDesc.Drag, 0.05f);
    iTween.RotateTo(gameObject, Vector3.zero, 0.25f);
    setRotationZConstraint(true);
    setPositionYConstraint(true);

    isDragging = true;
  }

  public void enterPowerUpState(PowerUpState state)
  {
    exitCurrentPowerUpState();

    switch (state) {
      case PowerUpState.None:
        animUtils.crossFadeRandomEntry((int)AnimDesc.Fly, 0.05f);
        break;

      case PowerUpState.BubbleGum:
        originalDrag = rigidbody.drag;
        originalBounce = collider.material.bounciness;
        rigidbody.drag = 0;
        collider.material.bounciness = 1;
        animUtils.crossFadeEntry((int)AnimDesc.PowerUp, (int)state-1, 0.05f);
        bubbleStartTime = Time.time;
        break;

      case PowerUpState.Glide:
		animUtils.crossFadeEntry((int)AnimDesc.PowerUp, (int)state-1, 0.05f);
        setPositionYConstraint(true);
        break;

      case PowerUpState.Fart:
			animUtils.crossFadeEntry((int)AnimDesc.PowerUp, (int)state-1, 0.05f);
			if (rigidbody.velocity.y < 0) 
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
		
        break;
    }

    currentPowerUp = state;
    iTween.RotateTo(gameObject, Vector3.zero, 0.25f);
    setRotationZConstraint(true);
    animator.SetInteger("PowerUp", (int)currentPowerUp);
  }

  private void exitCurrentPowerUpState()
  {
    isHurt = isDragging = false;
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
