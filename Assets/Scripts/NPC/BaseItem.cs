using UnityEngine;
using System.Collections.Generic;

public class BaseItem : MonoBehaviour
{
	// Item specifications
	public float MinSpawnHeight;
	public float MaxSpawnHeight;
	public float SpawnDensity;


	
	protected List<Vector3> SpawnPositions( GameObject wp )
	{
		var pos = wp.transform.position;
		var positions = new List<Vector3>();

		positions.Add( new Vector3( pos.x + 20f ,pos.y, pos.z ));
		return positions;
	}
	protected List<Vector3> SpawnPositionsCoins(GameObject wp)
	{
		var pos = wp.transform.position;
		var positions = new List<Vector3> ();

						positions.Add (new Vector3 (pos.x + 1f , pos.y + 1f, pos.z));
						
				
		return positions;
		}

	public virtual List<BaseItem> Spawn( GameObject wp )
	{
		return new List<BaseItem>();
	}
}
