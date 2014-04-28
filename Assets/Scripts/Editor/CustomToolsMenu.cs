using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class CustomToolsMenu : MonoBehaviour
{
  private static readonly HashSet<Renderer> isolatedRenderers = new HashSet<Renderer>();

  [MenuItem("Custom Tools/Isolate Selection &q")]
  static void IsolateSelection()
  {
    GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
    if (goArray == null)
      return;

    foreach (GameObject go in goArray) {
      Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
      foreach (Renderer r in renderers) {
        r.enabled = false;
        isolatedRenderers.Add(r);
      }
    }

    foreach (GameObject go in Selection.gameObjects) {
      Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
      foreach (Renderer r in renderers) {
        r.enabled = true;
        isolatedRenderers.Remove(r);
      }
    }
  }

  [MenuItem("Custom Tools/DeIsolate #q")]
  static void DeIsolate()
  {
    foreach (Renderer renderer in isolatedRenderers)
      renderer.enabled = true;

    isolatedRenderers.Clear();
  }

  [MenuItem("Custom Tools/Isolate Selection", true)]
  static bool IsolatePossible()
  {
    return isolatedRenderers.Count == 0;
  }

  [MenuItem("Custom Tools/DeIsolate", true)]
  static bool DeIsolatePossible()
  {
    return isolatedRenderers.Count > 0;
  }
}
