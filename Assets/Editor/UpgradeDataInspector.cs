using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeData))]
public class UpgradeDataInspector : Editor
{
  private UpgradeData data;
  public bool[] foldouts = new bool[UpgradeLevel.NumLevels];

  void OnEnable()
  {
    data = target as UpgradeData;
    data.ensureDataConsistency();
  }

  public override void OnInspectorGUI()
  {
    for (int i = 0; i < foldouts.Length; ++i) {
      foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "Level " + i.ToString());
      if (foldouts[i])
        CommonInspectorMethods.drawUpgradeSpecs(this, data.upgradeLevels[i].specs, data.upgradeLevels[i].prices);
    }

    EditorGUILayout.BeginHorizontal();
    if (GUILayout.Button("Load"))
      data.load();
    if (GUILayout.Button("Save"))
      data.save();
    EditorGUILayout.EndHorizontal();

    DrawDefaultInspector();
  }
}
