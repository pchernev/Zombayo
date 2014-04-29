using UnityEngine;
using System;

public class GameData : MonoBehaviour
{
  [HideInInspector]
  public UpgradeLevel.Specs specs;

  public int coinCount;
  public float bubbleGumLife;
  public float fartTime;
  public Vector3 fartDirection;
  public float glideTime;
  public Vector3 glideDirection;

  public float minKickPower;
  public float maxKickPower;
  public Vector3 kickDirection;
  public float kickEfficiency;
  public Vector3 KickForce { get { return Mathf.Max(minKickPower, kickEfficiency * maxKickPower) * kickDirection; } }
  public float dragForce;
  public float dragThreshold;

    [HideInInspector]
  public int[] levels;

  public float FartPercentage { get { return fartTime / specs.fartCapacity; } }
  public float GlidePercentage { get { return glideTime / specs.glideCapacity; } }

  void Awake()
  {
    ensureDataConsistency();
  }
  
  void Start ()
  {
    setFromLevels();
  }

  public void setFromUpgradeSpecs(UpgradeLevel.Specs specs, bool setTimeToCapacity = true)
  {
    this.specs = specs;
    if (setTimeToCapacity) {
      fartTime = specs.fartCapacity;
      glideTime = specs.glideCapacity;
    }
  }

  public void setFromLevels()
  {
    var newSpecs = GameLogic.Instance != null ? GameLogic.Instance.upgradeData.getSpecs(levels) : GetComponent<UpgradeData>().getSpecs(levels);
    setFromUpgradeSpecs(newSpecs);
  }

  public void ensureDataConsistency()
  {
    if (levels == null)
      levels = new int[UpgradeLevel.NumLevels];
    else if (levels.Length != UpgradeLevel.NumLevels)
      Array.Resize(ref levels, UpgradeLevel.NumLevels);
  }
}
