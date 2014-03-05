using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Carrot : BaseItem {
	private string a = "op";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter( Collision collision )
	{
		if (collision.gameObject.tag.CompareTo ("Player") == 0) {
			gameObject.audio.Play();
			collision.gameObject.rigidbody.velocity = Vector3.zero;
				}
	}
	public override List<BaseItem> Spawn( GameObject wp )
	{
		var items = new List<BaseItem>();
		
		var positions = base.SpawnPositions (wp);
		foreach( var pos in positions )
		{
			var carrot = Instantiate( this,pos, this.gameObject.transform.rotation ) as BaseItem;
			items.Add( carrot );
		}
		
		return items;
	}
}
