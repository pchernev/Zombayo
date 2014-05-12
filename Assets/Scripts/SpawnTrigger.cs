using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
  public Transform spawnPointsRoot;

  void Start()
  {
    gameObject.layer = Layers.Spawnables;
    if (spawnPointsRoot == null)
      spawnPointsRoot = transform;
  }

  void OnTriggerEnter(Collider other)
  {
    trigger();
  }

  public void trigger()
  {
    var spawnPoints = spawnPointsRoot.GetComponentsInChildren<SpawnPoint>();
    foreach (var point in spawnPoints)
      point.activate();
  }
}
