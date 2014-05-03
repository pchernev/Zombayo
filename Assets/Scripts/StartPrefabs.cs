using UnityEngine;
using System;

public class StartPrefabs : MonoBehaviour
{
  [Serializable]
  public class Info
  {
    public Component prefab;
    public Vector3 position;
    public Vector3 rotation;
  }

  public Info[] prefabsInfo;

  public void spawn()
  {
    foreach (var info in prefabsInfo)
      ObjectPool.Spawn(info.prefab, info.position, Quaternion.Euler(info.rotation));
  }
}
