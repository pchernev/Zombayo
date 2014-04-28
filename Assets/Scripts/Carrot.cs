using System;
using UnityEngine;

public class Carrot : NPC
{
  private enum AnimDesc {
    Idle,
    Miss,
    Hit,
    Count
  }

  private static readonly string[] animDescriptions;
  protected override string[] getDescriptionTexts() { return animDescriptions; }

  static Carrot()
  {
    animDescriptions = new string[(int)AnimDesc.Count];
    for (int i = 0; i < (int)AnimDesc.Count; ++i)
      animDescriptions[i] = ((AnimDesc) i).ToString();
  }

  public void miss()
  {
    ai.playRandomEntry((int)AnimDesc.Miss);
  }

  public void hit()
  {
    ai.playRandomEntry((int)AnimDesc.Hit);
    //AudioSource.PlayClipAtPoint(contactInfo.sound, transform.position);
  }
}
