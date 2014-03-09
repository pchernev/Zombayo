using UnityEngine;
using System.Collections;

public class Statistics
{
	public int Points;
    public int Coins;
    public int Distance;

    public Statistics(int coins, int points)
        :base()
	{		
        this.Coins = coins;
        this.Points = points;
	}

    public Statistics()
    {      
        this.Distance = 0;
    }
}
