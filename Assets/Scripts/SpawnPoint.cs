using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
  public Component prefab;
  public bool isScriptControlled;

  void Start()
  {
    if (!isScriptControlled)
      gameObject.layer = Layers.Spawnables;
  }

  void OnTriggerEnter(Collider other)
  {
    activate();
  }

  public void activate()
  {
    Component comp = prefab.Spawn(transform.position);
    comp.transform.parent = GameLogic.Instance.spawnRoot;
    if (!isScriptControlled)
      Destroy(gameObject);
  }
}
