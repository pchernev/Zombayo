using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LightmapMaterialWindow : EditorWindow
{
  [SerializeField]
  private Material dummyMat;
  private static Dictionary<Renderer, Material> matMap = new Dictionary<Renderer, Material>();
  

  void OnGUI()
  {
    dummyMat = EditorGUILayout.ObjectField("Dummy Lightmap Material", dummyMat, typeof(Material), true) as Material;

    if (matMap.Keys.Count == 0) {
      if (GUILayout.Button("Apply"))
        ApplyDummy();
    }
    else {
      if (GUILayout.Button("Restore"))
        RestoreOriginal();
    }
  }

  void ApplyDummy()
  {
    Renderer[] renderers = FindSceneObjectsOfType(typeof(Renderer)) as Renderer[];
    if (renderers == null)
      return;

    matMap.Clear();
    foreach (var r in renderers) {
      matMap.Add(r, r.sharedMaterial);
      r.sharedMaterial = dummyMat;
    }
  }

  void RestoreOriginal()
  {
    foreach (var kv in matMap)
      kv.Key.sharedMaterial = kv.Value;

    matMap.Clear();
  }

  [MenuItem("Custom Tools/Dummy Lightmap Material")]
  static void ShowWindow()
  {
    GetWindow<LightmapMaterialWindow>("LM Material");
  }
}