using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimatorUtils), true)]
public class AnimatorUtilsInspector : Editor
{
  private AnimatorUtils ai;
  private int removeIdx;

  void OnEnable()
  {
    ai = target as AnimatorUtils;
    ai.ensureDataConsistency();
  }

  public override void OnInspectorGUI()
  {
    var tmp = EditorGUILayout.ObjectField(ai.animator, typeof(Animator), false) as Animator;
    if (tmp != ai.animator) {
      ai.animator = tmp;
      ai.loadAnimationNames();
    }

    ai.namesFoldout = EditorGUILayout.Foldout(ai.namesFoldout, "Animation Names");
    if (ai.namesFoldout)
      for (int i = 1; i < ai.AnimationNames.Length; ++i)
        EditorGUILayout.LabelField(ai.AnimationNames[i]);

    ai.descFoldout = EditorGUILayout.Foldout(ai.descFoldout, "Animation Counts");
    if (ai.descFoldout) {
      for (int i = 0; i < ai.DescriptionTexts.Length; ++i) {
        EditorGUILayout.BeginHorizontal();
        if (ai.DescriptionEditingAllowed)
          ai.DescriptionTexts[i] = EditorGUILayout.TextField(ai.DescriptionTexts[i]);
        else
          EditorGUILayout.LabelField(ai.DescriptionTexts[i]);
        ai.setDescCount(i, Mathf.Max(1, EditorGUILayout.IntField(ai.descEntries[i].array.Length)));
        EditorGUILayout.EndHorizontal();
      }

      if (ai.DescriptionEditingAllowed) {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
          ai.addDescription();
        if (GUILayout.Button("Remove"))
          ai.removeDescription(removeIdx);
        removeIdx = EditorGUILayout.Popup(removeIdx, ai.DescriptionTexts);
        EditorGUILayout.EndHorizontal();
      }

      EditorGUILayout.Space();
    }

    ai.animFoldout = EditorGUILayout.Foldout(ai.animFoldout, "Animations");
    if (ai.animFoldout)
      CommonInspectorMethods.drawAnimationDescriptions(this, ai);
  }
}
