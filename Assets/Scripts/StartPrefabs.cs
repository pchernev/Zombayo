using UnityEngine;
using System;
using System.Collections.Generic;

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
  private List<Component> spawnedPrefabs;

  public void spawn()
  {
    foreach (var info in prefabsInfo)
      spawnedPrefabs.Add(ObjectPool.Spawn(info.prefab, info.position, Quaternion.Euler(info.rotation)));
  }

  public void recycleOld()
  {
    foreach (var comp in spawnedPrefabs)
      comp.Recycle();
  }
}
