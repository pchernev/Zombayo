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
  public int startIdleIndex;
  [HideInInspector]
  public AnimatorUtils ai;

  private Vector3 startPos;
  private Quaternion startRot;
  private bool isKicked;
  private Animator animator;
  private int idleIndex;
  private Vector3 lastPos;
  private Vector3 speed;
  private float bubbleStartTime;
  private float originalDrag;
  private float originalBounce;

  private PowerUpState currentPowerUp = PowerUpState.None;
  public bool IsKicked  { set { isKicked = value; if (animator) animator.SetBool("Kicked", value); } get { return isKicked; } }
  public bool IsBubbling { get { return currentPowerUp == PowerUpState.BubbleGum; } }
  public bool IsFarting { get { return currentPowerUp == PowerUpState.Fart; } }
  public bool IsGliding { get { return currentPowerUp == PowerUpState.Glide; } }
  public bool IsTooSlow { get { return transform.position.y < 1 && rigidbody.velocity.sqrMagnitude < 5; } }
  public float CurrentKickEfficiency { get { return animator.GetFloat("KickEfficiency"); } }

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

    if (IsKicked) {
      animator.SetFloat("VerticalSpeed", 2*speed.y);

      if (IsBubbling)
        if (bubbleStartTime + data.bubbleGumLife < Time.time)
          enterPowerUpState(PowerUpState.None);

      if (IsFarting)
        rigidbody.AddForce(data.specs.fartForce * data.fartDirection.normalized);

      if (IsGliding)
        rigidbody.AddForce(data.specs.glideForce * data.glideDirection.normalized);
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    if (!IsKicked)
      return;

    if (collision.gameObject.layer == Layers.Terrain)
      CollisionLogic.OnGroundHit();
  }

  void OnMouseUpAsButton()
  {
    if (!IsKicked)
      GameLogic.Instance.initiateKick();
  }


  void OnIdleFinishing()
  {
    ai.crossFadeRandomEntry((int)AnimDesc.Idle, 0.1f);
  }

  public virtual void ensureDataConsistency()
  {
    ai = GetComponent<AnimatorUtils>() ?? gameObject.AddComponent<AnimatorUtils>();
    ai.setTextsDelegate(getDescriptionTexts);
    ai.DescriptionEditingAllowed = false;
  }

  public void prepareForStart()
  {
    transform.position = startPos;
    transform.rotation = startRot;
    IsKicked = false;

    rigidbody.isKinematic = false;
    rigidbody.velocity = Vector3.zero;
    rigidbody.angularVelocity = Vector3.zero;
    rigidbody.isKinematic = false;
    magnet.gameObject.SetActive(false);
    ai.playEntry((int)AnimDesc.Idle, startIdleIndex);
  }

  public void kickRabbit(Vector3 kickForce)
  {
    IsKicked = true;
    enterPowerUpState(PowerUpState.None);
    magnet.gameObject.SetActive(true);
    rigidbody.AddForce(kickForce);
  }

  public void enterHurtState()
  {
    ai.crossFadeRandomEntry((int)AnimDesc.Hurt, 0.1f);
    setRotationZConstraint(false);
  }

  public void enterDragState()
  {
    ai.crossFadeRandomEntry((int)AnimDesc.Drag, 0.1f);
    iTween.RotateTo(gameObject, Vector3.zero, 0.25f);
    setRotationZConstraint(true);
  }

  public void enterPowerUpState(PowerUpState state)
  {
    exitCurrentPowerUpState();

    switch (state) {
      case PowerUpState.None:
        ai.crossFadeRandomEntry((int)AnimDesc.Fly, 0.1f);
        break;

      case PowerUpState.BubbleGum:
        originalDrag = rigidbody.drag;
        originalBounce = collider.material.bounciness;
        rigidbody.drag = 0;
        collider.material.bounciness = 1;
        ai.crossFadeEntry((int)AnimDesc.PowerUp, (int)state-1, 0.1f);
        bubbleStartTime = Time.time;
        break;

      case PowerUpState.Glide:
        ai.crossFadeEntry((int)AnimDesc.PowerUp, (int)state-1, 0.1f);
        setPositionYConstraint(true);
        break;

      case PowerUpState.Fart:
        ai.crossFadeEntry((int)AnimDesc.PowerUp, (int)state-1, 0.1f);
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
    switch (currentPowerUp) {
      case PowerUpState.None:
        break;

      case PowerUpState.BubbleGum:
        rigidbody.drag = originalDrag;
        collider.material.bounciness = originalBounce;
        break;

      case PowerUpState.Glide:
        setPositionYConstraint(false);
        break;

      case PowerUpState.Fart:
        break;
    }
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
