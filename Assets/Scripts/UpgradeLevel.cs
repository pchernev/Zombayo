using System;

[Serializable]
public class UpgradeLevel
{
  public enum Type
  {
    Magnet,
    CarrotSpray,
    BubbleGum,
    PowerUpFart,
    PowerUpGlide,
    Count
  }

  [Serializable]
  public class Specs
  {
    public float magnetRadius;
    public int bubbleGumCount;
    public int carrotSprayCount;
    public float fartForce;
    public float fartCapacity;
    public float glideForce;
    public float glideCapacity;
  }

  public const int NumLevels = 6;

  public Specs specs;
  public int[] prices;

  public UpgradeLevel()
  {
    specs = new Specs();
    prices = new int[(int)UpgradeLevel.Type.Count];
  }
}
