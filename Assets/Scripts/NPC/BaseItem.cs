using UnityEngine;
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

	public  Vector2 minDistance; //between the same objects;
	public Vector2 xSpawnLimits;



	
	protected List<Vector3> SpawnPositions( GameObject wp )
	{
				var pos = wp.transform.position;
				var positions = new List<Vector3> ();
				



				for (int i = 0; i < SpawnDensity; i++) {
						var xOffset = Random.Range (this.xSpawnLimits.x,this.xSpawnLimits.y) ;
						var yOffset = Random.Range (this.MinSpawnHeight, this.MaxSpawnHeight);


						positions.Add (new Vector3 (xOffset, yOffset, pos.z));
				}
		for (int i = 0; i < positions.Count; i++) {
			for (int j = 0; j < positions.Count; j++) {
				if (Mathf.Abs (positions [i].x - positions [j].x) <= minDistance.x) 
				{
					positions.RemoveAt(i);
				}
			}
		}

		return positions;
	}


		

	public virtual List<BaseItem> Spawn( GameObject wp )
	{
		return new List<BaseItem>();

	}
}
