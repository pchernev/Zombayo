using UnityEngine;
using System.Collections;

public class Statistics
{
	public int Points;
    public int Coins;
    public int Distance = 0;
    public int MaxDistance;
    public Statistics()
    {      
        this.Distance = 0;
        this.Points = 0;
        this.Coins = 0;
        this.MaxDistance = 0;
    }
}
