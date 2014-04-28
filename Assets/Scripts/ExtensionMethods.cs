using UnityEngine;
using System.Collections.Generic;

public static class ExtentionMethods
{
  public static string[] GetAnimationNames(this Animator animator)
  {
    UnityEditorInternal.AnimatorController ac = animator.runtimeAnimatorController as UnityEditorInternal.AnimatorController;
    List<string> names = new List<string>();

    names.Add("None");
    for (int layer = 0; layer < ac.layerCount; layer++) {
      UnityEditorInternal.StateMachine sm = ac.GetLayer(layer).stateMachine;
      for (int i = 0; i < sm.stateCount; i++) {
        UnityEditorInternal.State state = sm.GetState(i);
        names.Add(state.uniqueName);
      }
    }

    string[] result = new string[names.Count];
    names.CopyTo(result);

    return result;
  }
}
