using UnityEngine;

public class Recycler : MonoBehaviour
{
  void Start()
  {
    gameObject.layer = Layers.Recyclers;
  }

  void OnTriggerEnter(Collider other)
  {
    other.Recycle();
  }
}
