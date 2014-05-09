using UnityEngine;
using System;

public class GameData : MonoBehaviour
{
  private static string zeroLevelsString = "00000";

  [HideInInspector]
  public UpgradeLevel.Specs specs;

  public int coinsOnStart;
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
  public float distanceToFinish;
  public int coinsPerKm;
  public int currentHeight;
  public float travelledDistance;

  [HideInInspector]
  public int[] levels;
  [HideInInspector]
  public float kickTime;

  public float FartPercentage { get { return fartTime / specs.fartCapacity; } }
  public float GlidePercentage { get { return glideTime / specs.glideCapacity; } }
  public bool IsPlayerKicked { get { return kickTime >= 0; } }
  public bool ZeroProgress { get { return string.Join("", Array.ConvertAll(levels, x => x.ToString())) == zeroLevelsString && coinCount == 0; } }

  void Awake()
  {
    ensureDataConsistency();
    kickTime = -1;
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
      levels = new int[(int)UpgradeLevel.Type.Count];
    else if (levels.Length != (int)UpgradeLevel.Type.Count)
      Array.Resize(ref levels, (int)UpgradeLevel.Type.Count);
  }
}
