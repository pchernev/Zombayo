using UnityEngine;
using System.Collections;

public class DrFishhead : MonoBehaviour
{
  public string idleAnimationName;
  public string runAnimationName;
  public string kickAnimationName;

  private Vector3 startPos;
  private Quaternion startRot;
  private bool kickTransition;

  private Animator animator;

  void Awake()
  {
    animator = GetComponent<Animator>();

    startPos = transform.position;
    startRot = transform.rotation;
    kickTransition = false;
  }

  void OnTriggerEnter(Collider other)
  {
    if (!kickTransition) {
      animator.Play(kickAnimationName);
      kickTransition = true;
    }
    else
      GameLogic.Instance.launchRabbit();
  }

  public void playRunAndKickSequence()
  {
    animator.CrossFade(runAnimationName, 0.2f);
    rigidbody.AddForce(300 * Vector3.right);
  }

  public void prepareForStart()
  {
    transform.position = startPos;
    transform.rotation = startRot;
    kickTransition = false;

    animator.Play(idleAnimationName);

    rigidbody.isKinematic = false;
    rigidbody.velocity = Vector3.zero;
  }
}
