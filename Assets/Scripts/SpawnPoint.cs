using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
  public Component prefab;

  void Start()
  {
    gameObject.layer = Layers.Spawnables;
  }

  void OnTriggerEnter(Collider other)
  {
    activate();
  }

  public void activate()
  {
    prefab.Spawn(transform.position);
    Destroy(gameObject);
  }
}
