﻿using UnityEngine;
using System.Collections.Generic;

public class BaseItemAttribute : PropertyAttribute
{
}

public class BaseItem : MonoBehaviour
{
	// Item specifications
	[BaseItem]
	public float MinSpawnHeight;
	[BaseItem]
	public float MaxSpawnHeight;
	[BaseItem]
	public float SpawnDensity;

	public  float minDistance; //between the same objects;
	public Vector2 xSpawnLimits;



	
	protected List<Vector3> SpawnPositions( GameObject wp )
	{
				var pos = wp.transform.position;
				var positions = new List<Vector3> ();
				



				for (int i = 0; i < SpawnDensity; i++) {
						var xOffset = Random.Range (this.xSpawnLimits.x,this.xSpawnLimits.y) ;
						var yOffset = Random.Range (this.MinSpawnHeight, this.MaxSpawnHeight);
			Vector3 poss = new Vector3(xOffset, yOffset, 0f);

			var inRange = Physics.OverlapSphere(poss,minDistance);

			if(inRange.Length == 0)
			{
				positions.Add(poss);
			}



				}
	

		


		return positions;
	}


		

	public virtual List<BaseItem> Spawn( GameObject wp )
	{
		return new List<BaseItem>();

	}
}
