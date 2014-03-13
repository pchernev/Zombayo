using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coin : BaseItem {
	public bool gotMagnet;
	public int magnetPower;
	
	long score = 0;
	public float timeToReach;
	public float coinSpeed;
	
	
	
	
	void Start()
	{
		
		
		
		
		
		
	}
	void Update()
	{
		var pos = transform.position;
		pos.x -= coinSpeed;
		transform.position = pos;
		GameObject _player = GameObject.FindWithTag ("Player");
		var playerpos = _player.transform.position;
		if (gotMagnet == true) {


			var distance = Vector3.Distance (gameObject.transform.position, playerpos);
			if (distance <= magnetPower) {
				
				iTween.MoveTo(gameObject,iTween.Hash("position",playerpos,"easetype",iTween.EaseType.easeInOutSine,"time",timeToReach));
			}
		}
		
		
		
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Player") {
			Destroy (this.gameObject);
			
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

