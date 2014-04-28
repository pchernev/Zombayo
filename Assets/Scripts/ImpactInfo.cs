using UnityEngine;
using System;

[Serializable]
public class ImpactInfo
{
  public Vector3 force;
  public GameObject particles;
  public AudioClip sound;

  public int animIndex;
  public string animationName;

  public ImpactInfo() { force = Vector3.zero; animIndex = 0; animationName = ""; }
}
