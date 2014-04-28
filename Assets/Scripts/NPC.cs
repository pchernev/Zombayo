using UnityEngine;
using System;

public abstract class NPC : MonoBehaviour
{
  protected abstract string[] getDescriptionTexts();

  [HideInInspector]
  public ImpactInfo impactInfo;
  [HideInInspector]
  public AnimatorUtils ai;

  [HideInInspector]
  public bool infoFoldout;
  
  void Awake()
  {
    ensureDataConsistency();
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer == Layers.Player)
      CollisionLogic.OnNpcHit(this);
  }

  public virtual void ensureDataConsistency()
  {
    if (impactInfo == null)
      impactInfo = new ImpactInfo();

    ai = GetComponent<AnimatorUtils>() ?? gameObject.AddComponent<AnimatorUtils>();
    ai.setTextsDelegate(getDescriptionTexts);
    ai.DescriptionEditingAllowed = false;
  }
}
