using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnCoins : BaseItem {
	public GameObject coin;
	public int ammount;

	// Use this for initialization
	public override List<BaseItem> Spawn( GameObject a )
	{
		var items = new List<BaseItem>();
		
		var positions = base.SpawnPositionsCoins (a);
		
		for(int i = 0;i<ammount;i++) {
			var coin = Instantiate (this, this.transform.position, this.gameObject.transform.rotation) as BaseItem;
			items.Add (coin);
			
		}
		
		return items;
	}
}
