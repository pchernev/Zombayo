using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public GameObject playerTarget;
  public Vector3 offsetBeforeKick;
  public Vector3 rotationBeforeKick;
  public Vector3 offsetAfterKick;
  public Vector3 rotationAfterKick;

  private Vector3 cameraOffset;
  private Vector3 cameraRotaion;
  private Vector3 nextOffset;
  private Vector3 nextRotaion;

  public bool doTransition;
  public float transitionTime;

  private bool transitionInProgress;
  private float transitionStartTime;
  private Vector3 offsetChangeRate;
  private Vector3 rotationChangeRate;


  void Update()
  {
    if (doTransition) {
      startTransition();
      doTransition = false;
    }

    if (transitionInProgress)
      updateTransition();

    follow();
  }

  public void reset()
  {
    cameraOffset = offsetBeforeKick;
    cameraRotaion = rotationBeforeKick;
    follow();
  }

  private void follow()
  {
    var p = playerTarget.transform.position;
    Camera.main.transform.position = new Vector3(p.x + cameraOffset.x, p.y + cameraOffset.y, p.z + cameraOffset.z);
    Camera.main.transform.eulerAngles = cameraRotaion;
  }

  private void startTransition()
  {
    nextOffset = offsetAfterKick;
    nextRotaion = rotationAfterKick;

    offsetChangeRate = (nextOffset - cameraOffset) / transitionTime;
    rotationChangeRate = (nextRotaion - cameraRotaion) / transitionTime;
    transitionInProgress = true;
  }

  private void updateTransition()
  {
    if (transitionTime <= 0) {
      cameraOffset = nextOffset;
      cameraRotaion = nextRotaion;

      transitionTime = 0;
      transitionInProgress = false;
    }
    else {
      cameraOffset += Time.deltaTime * offsetChangeRate;
      cameraRotaion += Time.deltaTime * rotationChangeRate;

      transitionTime -= Time.deltaTime;
    }
  }
	
	// 0.24, 0.98, -3.07
	// 3.37, 7.89, 0
}
