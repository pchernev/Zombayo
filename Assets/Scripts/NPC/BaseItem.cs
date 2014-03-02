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
	
	protected List<Vector3> SpawnPositions( GameObject wp )
	{
		var pos = wp.transform.position;
		var positions = new List<Vector3>();

		positions.Add( new Vector3( pos.x + 20f, pos.y, pos.z ));
		return positions;
	}

	public virtual List<BaseItem> Spawn( GameObject wp )
	{
		return new List<BaseItem>();
	}
}
