using UnityEngine;

public class RecycleTrigger : MonoBehaviour
{
  public Transform recyclable;

  void Start()
  {
    gameObject.layer = Layers.Recyclables;
    if (recyclable == null)
      recyclable = transform;
  }

  void OnTriggerEnter(Collider other)
  {
    recyclable.Recycle();
  }
}
