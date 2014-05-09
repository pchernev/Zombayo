using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameData))]
public class GameDataInspector : Editor
{
  private GameData data;

  void OnEnable()
  {
    data = target as GameData;
    data.ensureDataConsistency();
  }

  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    CommonInspectorMethods.drawUpgradeSpecs(this, data.specs);

    if (GUILayout.Button("Set specs to level values"))
      data.setFromLevels();

    CommonInspectorMethods.drawLevels(this, data.levels);
  }
}
