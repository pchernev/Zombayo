using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Coin : BaseItem {
	public bool gotMagnet;
	public int magnetPower; // todo: get from last saved value;
	
	long score = 0;
	public float timeToReach;
	public float coinSpeed;
    GameObject _player;
	private bool inRange;

	
	
	
	void Start()
	{
        _player = GameObject.FindWithTag("Player");
        gotMagnet = true;
        var item = _player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Magnet");
        if (item.UpgradesCount > 0)
        {
            magnetPower = (int)item.Values[item.UpgradesCount - 1];

        }
	}

    void FixedUpdate()
    {
        var pos = transform.position;
        //pos.x -= coinSpeed;
        //transform.position = pos;
        var playerpos = _player.transform.position;
      

        if (gotMagnet == true)
        {
            float distance = Vector3.Distance(pos, playerpos);
            if(distance <= magnetPower)
            {
				inRange = true;
				iTween.MoveUpdate(this.gameObject, playerpos ,timeToReach);
            }
        }
    }


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Player") {
			Destroy (this.gameObject);
			_player.GetComponent<Player>().gameData.PlayerStats.Coins++;
		}
	}
	
	public override List<BaseItem> Spawn( GameObject wp )
	{
		var items = new List<BaseItem>();
		
		var positions = base.SpawnPositions( wp );
		foreach( var pos in positions )
		{
			var coin = Instantiate( this,pos, this.gameObject.transform.rotation ) as BaseItem;
			items.Add( coin );
		}
		
		return items;
	}
}

