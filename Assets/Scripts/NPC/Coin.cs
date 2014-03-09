
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coin : BaseItem {
	public bool gotMagnet;
	public int magnetPower;

	long score = 0;
	public float timeToReach;
	public float coinSpeed;




	
	void Update()
	{
		var pos = transform.position;
		pos.x -= coinSpeed;
		transform.position = pos;
		if (gotMagnet == true) {
						GameObject _player = GameObject.FindWithTag ("Player");
						var distance = Vector3.Distance (this.gameObject.transform.position, _player.transform.position);
						if (distance <= magnetPower) {
								iTween.MoveUpdate (this.gameObject, _player.transform.position, timeToReach);
						}

				}
		}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Player") {
			Destroy(this.gameObject);
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

		


		


	
	
	
	

	


	

