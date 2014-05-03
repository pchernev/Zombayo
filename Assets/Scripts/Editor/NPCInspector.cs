using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPC), true)]
public class NPCInspector : Editor
{
  private NPC npc;

  void OnEnable()
  {
    npc = target as NPC;
    npc.ensureDataConsistency();
  }

  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    npc.infoFoldout = EditorGUILayout.Foldout(npc.infoFoldout, "On Impact With Player:");
    if (npc.infoFoldout) {
      npc.impactInfo.force = EditorGUILayout.Vector3Field("Force", npc.impactInfo.force);
      npc.impactInfo.particles = EditorGUILayout.ObjectField("Particles", npc.impactInfo.particles, typeof(GameObject), false) as GameObject;
      npc.impactInfo.sound = EditorGUILayout.ObjectField("Particles", npc.impactInfo.sound, typeof(AudioClip), false) as AudioClip;
      CommonInspectorMethods.drawAnimPopup("Animation", ref npc.impactInfo.animIndex, npc.animUtils.AnimationNames, ref npc.impactInfo.animationName);
    }
  }
}
