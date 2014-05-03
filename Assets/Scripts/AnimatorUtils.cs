using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AnimatorUtils : MonoBehaviour
{
  [Serializable]
  public class Entry
  {
    public int index;
    public string animationName;

    public Entry() { index = 0; animationName = ""; }
  }

  [Serializable]
  public class EntryArray
  {
    public Entry[] array;

    public EntryArray() { array = new Entry[1]; array[0] = new Entry(); }
  }

  public EntryArray[] descEntries;

  public Animator animator;

  private static string[] _animationNames;
  public string[] AnimationNames { get { return _animationNames; } }

  public delegate string[] DescriptionTextsDelegate();
  private DescriptionTextsDelegate textsDelegate;
  public void setTextsDelegate(DescriptionTextsDelegate del) { textsDelegate = del; }

  private string[] ownTexts;
  public string[] DescriptionTexts { get { return textsDelegate != null ? textsDelegate() : ownTexts; } }

  public bool namesFoldout;
  public bool descFoldout;
  public bool animFoldout;

  private bool descriptionEditingAllowed = true;
  public bool DescriptionEditingAllowed { get { return descriptionEditingAllowed; } set { if (value) textsDelegate = null; descriptionEditingAllowed = value; } }

  void Awake()
  {
    ensureDataConsistency();
  }

  public virtual void ensureDataConsistency()
  {
#if UNITY_DEITOR
    loadAnimationNames();
#endif

    if (descriptionEditingAllowed)
      textsDelegate = null;
    if (textsDelegate == null && ownTexts == null)
      ownTexts = new string[0];

    if (descEntries == null) {
      descEntries = new EntryArray[DescriptionTexts.Length];
      for (int i = 0; i < descEntries.Length; ++i)
        descEntries[i] = new EntryArray();
    }

    foreach (EntryArray t in descEntries)
      foreach (var entry in t.array)
        entry.index = findAnimIndex(entry.animationName);
  }

  public Entry getEntry(int descIdx, int pos)
  {
    return descEntries[descIdx].array[pos];
  }

  public void playEntry(int descIdx, int pos)
  {
    animator.Play(getEntry(descIdx, pos).animationName);
  }

  public void playRandomEntry(int descIdx)
  {
    int pos = Random.Range(0, descEntries[descIdx].array.Length);
    animator.Play(getEntry(descIdx, pos).animationName);
  }

  public void crossFadeEntry(int descIdx, int pos, float transitionDuration)
  {
    animator.CrossFade(getEntry(descIdx, pos).animationName, transitionDuration);
  }

  public void crossFadeRandomEntry(int descIdx, float transitionDuration)
  {
    int pos = Random.Range(0, descEntries[descIdx].array.Length);
    animator.CrossFade(getEntry(descIdx, pos).animationName, transitionDuration);
  }

  public void setDescCount(int index, int count)
  {
    int oldSize = descEntries[index].array.Length;
    if (oldSize != count)
      Array.Resize(ref descEntries[index].array, count);
    if (oldSize < count)
      for (int i = oldSize; i < count; ++i)
        descEntries[index].array[i] = new Entry();
  }

#if UNITY_EDITOR
  public static string[] GetAnimationNames(Animator animator)
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

  public void loadAnimationNames()
  {
    _animationNames = animator != null ? GetAnimationNames(animator) : new string[] { "None" };
  }
#endif

  public void addDescription()
  {
    int newSize = ownTexts.Length + 1;

    Array.Resize(ref ownTexts, newSize);
    Array.Resize(ref descEntries, newSize);

    descEntries[newSize - 1].array = new Entry[1];
    descEntries[newSize - 1].array[0] = new Entry();
  }

  public void removeDescription(int index)
  {
    int newSize = ownTexts.Length - 1;

    if (index < newSize) {
      Array.Copy(ownTexts, index + 1, ownTexts, index, newSize - index);
      Array.Copy(descEntries, index + 1, descEntries, index, newSize - index);
    }

    Array.Resize(ref ownTexts, newSize);
    Array.Resize(ref descEntries, newSize);
  }

  private int findAnimIndex(string animName)
  {
    return Array.FindIndex(_animationNames, 0, s => s == (animName.Length > 0 ? animName : "None"));
  }
}
