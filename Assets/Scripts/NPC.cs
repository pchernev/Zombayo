using UnityEngine;

public abstract class NPC : MonoBehaviour
{
  protected abstract string[] getDescriptionTexts();

  [HideInInspector]
  public ImpactInfo impactInfo;
  [HideInInspector]
  public AnimatorUtils animUtils;

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

    animUtils = GetComponent<AnimatorUtils>() ?? gameObject.AddComponent<AnimatorUtils>();
    animUtils.setTextsDelegate(getDescriptionTexts);
    animUtils.DescriptionEditingAllowed = false;
  }
}
