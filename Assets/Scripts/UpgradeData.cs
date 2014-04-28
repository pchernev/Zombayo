using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.IO;

public class UpgradeData : MonoBehaviour
{
  [HideInInspector]
  public UpgradeLevel[] upgradeLevels;
  public string dataFileName = "Assets/upgrade_data.txt";

  private const string TokenValue = "Value";
  private const string TokenPrice = "Price";
  private const string TokenForce = "Force";
  private const string TokenCapacity = "Capacity";


  void Awake()
  {
    ensureDataConsistency();
  }

  public UpgradeLevel.Specs getSpecs(int[] levels)
  {
    var specs = new UpgradeLevel.Specs();
    
    specs.bubbleGumCount   = upgradeLevels[levels[(int)UpgradeLevel.Type.BubbleGum]]   .specs.bubbleGumCount;
    specs.carrotSprayCount = upgradeLevels[levels[(int)UpgradeLevel.Type.CarrotSpray]] .specs.carrotSprayCount;
    specs.fartCapacity     = upgradeLevels[levels[(int)UpgradeLevel.Type.PowerUpFart]] .specs.fartCapacity;
    specs.fartForce        = upgradeLevels[levels[(int)UpgradeLevel.Type.PowerUpFart]] .specs.fartForce;
    specs.glideCapacity    = upgradeLevels[levels[(int)UpgradeLevel.Type.PowerUpGlide]].specs.glideCapacity;
    specs.magnetRadius     = upgradeLevels[levels[(int)UpgradeLevel.Type.Magnet]]      .specs.magnetRadius;

    return specs;
  }

  public int[] getPrices(int[] levels)
  {
    int[] prices = new int[(int)UpgradeLevel.Type.Count];

    for (int i = 0; i < (int)UpgradeLevel.Type.Count; ++i)
      prices[i] = upgradeLevels[levels[i]].prices[i];

    return prices;
  }

  public void ensureDataConsistency()
  {
    if (upgradeLevels == null) {
      upgradeLevels = new UpgradeLevel[UpgradeLevel.NumLevels];
      for (int i = 0; i < UpgradeLevel.NumLevels; ++i)
        upgradeLevels[i] = new UpgradeLevel();
    }
    else if (upgradeLevels.Length != UpgradeLevel.NumLevels) {
      int oldSize = upgradeLevels.Length;
      Array.Resize<UpgradeLevel>(ref upgradeLevels, UpgradeLevel.NumLevels);
      for (int i = oldSize; i < UpgradeLevel.NumLevels; ++i)
        upgradeLevels[i] = new UpgradeLevel();
    }
  }

  public void save()
  {
    string text = "";

    text += UpgradeLevel.Type.Magnet + " " + TokenValue;
    for (int level = 0; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].specs.magnetRadius;
    text += Environment.NewLine;

    text += UpgradeLevel.Type.Magnet + " " + TokenPrice;
    for (int level = 1; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].prices[(int)UpgradeLevel.Type.Magnet];
    text += Environment.NewLine;

    text += UpgradeLevel.Type.CarrotSpray + " " + TokenValue;
    for (int level = 0; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].specs.carrotSprayCount;
    text += Environment.NewLine;

    text += UpgradeLevel.Type.CarrotSpray + " " + TokenPrice;
    for (int level = 1; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].prices[(int)UpgradeLevel.Type.CarrotSpray];
    text += Environment.NewLine;

    text += UpgradeLevel.Type.BubbleGum + " " + TokenValue;
    for (int level = 0; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].specs.bubbleGumCount;
    text += Environment.NewLine;

    text += UpgradeLevel.Type.BubbleGum + " " + TokenPrice;
    for (int level = 1; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].prices[(int)UpgradeLevel.Type.BubbleGum];
    text += Environment.NewLine;

    text += UpgradeLevel.Type.PowerUpFart + " " + TokenCapacity + " " + TokenValue;
    for (int level = 0; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].specs.fartCapacity;
    text += Environment.NewLine;

    text += UpgradeLevel.Type.PowerUpFart + " " + TokenForce + " " + TokenValue;
    for (int level = 0; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].specs.fartForce;
    text += Environment.NewLine;

    text += UpgradeLevel.Type.PowerUpFart + " " + TokenPrice;
    for (int level = 1; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].prices[(int)UpgradeLevel.Type.PowerUpFart];
    text += Environment.NewLine;

    text += UpgradeLevel.Type.PowerUpGlide + " " + TokenCapacity + " " + TokenValue;
    for (int level = 0; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].specs.glideCapacity;
    text += Environment.NewLine;

    text += UpgradeLevel.Type.PowerUpGlide + " " + TokenForce + " " + TokenValue;
    for (int level = 0; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].specs.glideForce;
    text += Environment.NewLine;

    text += UpgradeLevel.Type.PowerUpGlide + " " + TokenPrice;
    for (int level = 1; level < upgradeLevels.Length; ++level)
      text += " " + upgradeLevels[level].prices[(int)UpgradeLevel.Type.PowerUpGlide];
    text += Environment.NewLine;

    Debug.Log(text);

    File.WriteAllText(dataFileName, text);
  }

  public void load()
  {
    string text = File.ReadAllText(dataFileName);

    var lines = Regex.Split(text, "\r\n|\r|\n");
    foreach (var line in lines)
      parseLine(line);
  }

  public void parseLine(string line)
  {
    var tokens = line.Split(' ');

    if (tokens[0] == UpgradeLevel.Type.Magnet.ToString()) {
      if (tokens[1] == TokenValue)
        for (int level = 0; level < upgradeLevels.Length; ++level)
          upgradeLevels[level].specs.magnetRadius = float.Parse(tokens[2 + level]);
      else if (tokens[1] == TokenPrice)
        for (int level = 1; level < upgradeLevels.Length; ++level)
          upgradeLevels[level].prices[(int)UpgradeLevel.Type.Magnet] = int.Parse(tokens[1 + level]);
    }

    else if (tokens[0] == UpgradeLevel.Type.CarrotSpray.ToString()) {
      if (tokens[1] == TokenValue)
        for (int level = 0; level < upgradeLevels.Length; ++level)
          upgradeLevels[level].specs.carrotSprayCount = int.Parse(tokens[2 + level]);
      else if (tokens[1] == TokenPrice)
        for (int level = 1; level < upgradeLevels.Length; ++level)
          upgradeLevels[level].prices[(int)UpgradeLevel.Type.CarrotSpray] = int.Parse(tokens[1 + level]);
    }

    else if (tokens[0] == UpgradeLevel.Type.BubbleGum.ToString()) {
      if (tokens[1] == TokenValue)
        for (int level = 0; level < upgradeLevels.Length; ++level)
          upgradeLevels[level].specs.bubbleGumCount = int.Parse(tokens[2 + level]);
      else if (tokens[1] == TokenPrice)
        for (int level = 1; level < upgradeLevels.Length; ++level)
          upgradeLevels[level].prices[(int)UpgradeLevel.Type.BubbleGum] = int.Parse(tokens[1 + level]);
    }

    else if (tokens[0] == UpgradeLevel.Type.PowerUpFart.ToString()) {
      if (tokens[1] == TokenValue) {
        if (tokens[2] == TokenCapacity)
          for (int level = 0; level < upgradeLevels.Length; ++level)
            upgradeLevels[level].specs.fartCapacity = float.Parse(tokens[3 + level]);
        else if (tokens[2] == TokenForce)
          for (int level = 0; level < upgradeLevels.Length; ++level)
            upgradeLevels[level].specs.fartForce = float.Parse(tokens[3 + level]);
      }
      else if (tokens[1] == TokenPrice)
        for (int level = 1; level < upgradeLevels.Length; ++level)
          upgradeLevels[level].prices[(int)UpgradeLevel.Type.PowerUpFart] = int.Parse(tokens[1 + level]);
    }

    else if (tokens[0] == UpgradeLevel.Type.PowerUpGlide.ToString()) {
      if (tokens[1] == TokenValue) {
        if (tokens[2] == TokenCapacity)
          for (int level = 0; level < upgradeLevels.Length; ++level)
            upgradeLevels[level].specs.glideCapacity = float.Parse(tokens[3 + level]);
        else if (tokens[2] == TokenForce)
          for (int level = 0; level < upgradeLevels.Length; ++level)
            upgradeLevels[level].specs.glideForce = float.Parse(tokens[3 + level]);
      }
      else if (tokens[1] == TokenPrice)
        for (int level = 1; level < upgradeLevels.Length; ++level)
          upgradeLevels[level].prices[(int)UpgradeLevel.Type.PowerUpGlide] = int.Parse(tokens[1 + level]);
    }
  }
}
