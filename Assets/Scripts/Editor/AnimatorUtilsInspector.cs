using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimatorUtils), true)]
public class AnimatorUtilsInspector : Editor
{
  private AnimatorUtils animUtils;
  private int removeIdx;

  void OnEnable()
  {
    animUtils = target as AnimatorUtils;
    animUtils.ensureDataConsistency();
  }

  public override void OnInspectorGUI()
  {
    var tmp = EditorGUILayout.ObjectField(animUtils.animator, typeof(Animator), false) as Animator;
    if (tmp != animUtils.animator) {
      animUtils.animator = tmp;
      animUtils.loadAnimationNames();
    }

    animUtils.namesFoldout = EditorGUILayout.Foldout(animUtils.namesFoldout, "Animation Names");
    if (animUtils.namesFoldout)
      for (int i = 1; i < animUtils.AnimationNames.Length; ++i)
        EditorGUILayout.LabelField(animUtils.AnimationNames[i]);

    animUtils.descFoldout = EditorGUILayout.Foldout(animUtils.descFoldout, "Animation Counts");
    if (animUtils.descFoldout) {
      for (int i = 0; i < animUtils.DescriptionTexts.Length; ++i) {
        EditorGUILayout.BeginHorizontal();
        if (animUtils.DescriptionEditingAllowed)
          animUtils.DescriptionTexts[i] = EditorGUILayout.TextField(animUtils.DescriptionTexts[i]);
        else
          EditorGUILayout.LabelField(animUtils.DescriptionTexts[i]);
        animUtils.setDescCount(i, Mathf.Max(1, EditorGUILayout.IntField(animUtils.descEntries[i].array.Length)));
        EditorGUILayout.EndHorizontal();
      }

      if (animUtils.DescriptionEditingAllowed) {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
          animUtils.addDescription();
        if (GUILayout.Button("Remove"))
          animUtils.removeDescription(removeIdx);
        removeIdx = EditorGUILayout.Popup(removeIdx, animUtils.DescriptionTexts);
        EditorGUILayout.EndHorizontal();
      }

      EditorGUILayout.Space();
    }

    animUtils.animFoldout = EditorGUILayout.Foldout(animUtils.animFoldout, "Animations");
    if (animUtils.animFoldout)
      CommonInspectorMethods.drawAnimationDescriptions(this, animUtils);
  }
}
