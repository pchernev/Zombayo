using UnityEngine;

public static class Layers
{
  public static readonly int Player  = LayerMask.NameToLayer("Player");
  public static readonly int Magnet = LayerMask.NameToLayer("Magnet");
  public static readonly int Terrain = LayerMask.NameToLayer("Terrain");
  public static readonly int Coins = LayerMask.NameToLayer("Coins");
  public static readonly int NPCs = LayerMask.NameToLayer("NPCs");
  public static readonly int AttractedCoins = LayerMask.NameToLayer("AttractedCoins");
  public static readonly int Spawners = LayerMask.NameToLayer("Spawners");
  public static readonly int Spawnables = LayerMask.NameToLayer("Spawnables");
  public static readonly int Recyclers = LayerMask.NameToLayer("Recyclers");
  public static readonly int Recyclables = LayerMask.NameToLayer("Recyclables");
}
