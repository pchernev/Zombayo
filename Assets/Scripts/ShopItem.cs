using UnityEngine;
using System.Collections;

public class ShopItem 
{
    public string Name { get; set; }
    public int UpgradesCount { get; set; }
    public int MaxUpgradesCount { get; set; }
    public int Price { get; set; }
    public int NextUpgradePricePercentage { get; set; }
}
