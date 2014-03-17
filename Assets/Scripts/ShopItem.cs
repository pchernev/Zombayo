using UnityEngine;
using System.Collections;

public class ShopItem 
{
    public string Name { get; set; }
    public int UpgradesCount { get; set; }
    public int MaxUpgradesCount 
    {
        get 
        {
            if (Prices.Length == Values.Length)
	        {
                return Prices.Length;
            }
            else
            {
                return -1;
            }
        }
    }
    public int[] Prices { get; set; }
    public float[] Values { get; set; }
}
