using UnityEditor;
using UnityEngine;

public static class CommonInspectorMethods
{
  public static void drawUpgradeSpecs(Editor editor, UpgradeLevel.Specs specs, int[] prices = null)
  {
    EditorGUILayout.Space();
    EditorGUILayout.LabelField("Specs:");
    EditorGUILayout.Space();

    drawUpgradeInfoFloat(editor, "Magnet Radius", ref specs.magnetRadius);
    drawPriceInfo(editor, prices, UpgradeLevel.Type.Magnet);
    EditorGUILayout.Space();

    drawUpgradeInfoInt(editor, "Carrot Sprays", ref specs.carrotSprayCount);
    drawPriceInfo(editor, prices, UpgradeLevel.Type.CarrotSpray);
    EditorGUILayout.Space();

    drawUpgradeInfoInt(editor, "Bubble Gums", ref specs.bubbleGumCount);
    drawPriceInfo(editor, prices, UpgradeLevel.Type.BubbleGum);
    EditorGUILayout.Space();

    drawUpgradeInfoFloat(editor, "Fart Capacity", ref specs.fartCapacity);
    drawUpgradeInfoFloat(editor, "Fart Force", ref specs.fartForce);
    drawPriceInfo(editor, prices, UpgradeLevel.Type.PowerUpFart);
    EditorGUILayout.Space();

    drawUpgradeInfoFloat(editor, "Glide Capacity", ref specs.glideCapacity);
    drawUpgradeInfoFloat(editor, "Glide Force", ref specs.glideForce);
    drawPriceInfo(editor, prices, UpgradeLevel.Type.PowerUpGlide);

    EditorGUILayout.Space();
  }

  public static void drawLevels(Editor editor, int[] levels)
  {
    EditorGUILayout.Space();
    EditorGUILayout.LabelField("Levels:");
    EditorGUILayout.Space();

    for (int i = 0; i < (int)UpgradeLevel.Type.Count; ++i) {
      var type = (UpgradeLevel.Type) i;
      levels[i] = EditorGUILayout.IntField(type.ToString(), levels[i]);
      EditorGUILayout.Space();
    }

    EditorGUILayout.Space();
  }

  public static void drawAnimationDescriptions(Editor editor, AnimatorUtils ai)
  {
    for (int descIdx = 0; descIdx < ai.DescriptionTexts.Length; ++descIdx)
      if (ai.descEntries[descIdx].array.Length == 1)
        drawAnimPopup(ai.DescriptionTexts[descIdx], ref ai.descEntries[descIdx].array[0].index, ai.AnimationNames, ref ai.descEntries[descIdx].array[0].animationName);
      else
        for (int i = 0; i < ai.descEntries[descIdx].array.Length; ++i)
          drawAnimPopup(ai.DescriptionTexts[descIdx] + " " + (i + 1), ref ai.descEntries[descIdx].array[i].index, ai.AnimationNames, ref ai.descEntries[descIdx].array[i].animationName);
  }

  public static void drawUpgradeInfoInt(Editor editor, string caption, ref int value)
  {
    value = EditorGUILayout.IntField(caption, value);
  }

  public static void drawUpgradeInfoFloat(Editor editor, string caption, ref float value)
  {
    value = EditorGUILayout.FloatField(caption, value);
  }

  public static void drawPriceInfo(Editor editor, int[] prices, UpgradeLevel.Type type)
  {
    if (prices != null)
      prices[(int)type] = EditorGUILayout.IntField("Price", prices[(int)type]);
  }

  public static void drawAnimPopup(string label, ref int index, string[] names, ref string animationName)
  {
    int tmpIndex = EditorGUILayout.Popup(label, index, names);
    if (index != tmpIndex) {
      index = tmpIndex;
      animationName = index > 0 ? names[index] : "";
    }
  }
}
