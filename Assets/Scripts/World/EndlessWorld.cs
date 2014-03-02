using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndlessWorld : MonoBehaviour
{
  	public float speed = 8f; 
    public float countdown = 3.0f;
	
	public GameObject[] tiles;
	public GameObject[] npcs;
	public BaseItem[] items;
	
	private List<GameObject> _objs;
	private List<GameObject> _npcs;
	private List<BaseItem> _items;
	private float cameraSize = 150.0f;
	
	private GameObject _player;

	// Use this for initialization
	void Start ()
	{
		_player = GameObject.FindWithTag( "Player" );
		if( _player == null )
			Debug.Log( "Player object not found" );
		
		_objs = new List<GameObject>();
		_npcs = new List<GameObject>();
		_items = new List<BaseItem>();
		
		var p = new Vector3( 0F, 0F, 0F );
		foreach( var prefab in tiles )
		{			
			var obj = Instantiate( prefab, p, prefab.gameObject.transform.rotation ) as GameObject;
			_objs.Add( obj );
			
			var piece = prefab.GetComponent<WorldPiece>();
			p.x += piece.getBounds().size.x;//mesh.bounds.size.x;
			
//			Debug.Log( string.Format( "pos: {0} size: {1}", p.ToString(), piece.bounds.size.ToString() ));
			
//			mesh.enabled = false;
			break;
		}
		
		InitWorldPieces();
	}
	
	// Update is called once per frame
	void Update ()
	{
		var fObj = _objs.FirstOrDefault() as GameObject;
		var lObj = _objs.LastOrDefault() as GameObject;
		
		var fX = fObj.transform.position.x;
		var lX = lObj.transform.position.x;
		
		var fWorldPiece = fObj.GetComponent<WorldPiece>();
		var lWorldPiece = lObj.GetComponent<WorldPiece>();
		
		var fBounds = fWorldPiece.getBounds();
		var lBounds = lWorldPiece.getBounds();
		
		// camera position 
		var lCamX = transform.position.x - cameraSize;
		var rCamX = transform.position.x + cameraSize;
		
//		Debug.Log( string.Format( "lCamX: {0} rCamX: {1}, fBounds.min.x: {2}, lBounds.max.x: {3}", lCamX, rCamX, fBounds.min.x, lBounds.max.x ));
//		Debug.Log( "_objs.Lenght: " + _objs.Count );
		
		// backward update
		if( lCamX < fX )
		{
			var index = Random.Range( 0, tiles.Length - 1);
			var prefab = tiles[index];
			var piece = prefab.GetComponent<WorldPiece>();
			var x = fObj.transform.position.x - piece.getBounds().size.x;
			var pos = new Vector3( x, 0F, 0F );
			
			var obj = AddTile( prefab, pos );
			if( obj != null )
				_objs.Insert( 0, obj );
			
//			Debug.Log( "backward x: " + x );
		}
		
		// forward update
		if( rCamX > lX )
		{
			var index = Random.Range( 0, tiles.Length - 1);
			var prefab = tiles[index];
			var piece = prefab.GetComponent<WorldPiece>();
			var x = lX + fBounds.max.x + piece.getBounds().size.x * 0.5f;
			var pos = new Vector3( x, 0F, 0F );
			
			var obj = AddTile( prefab, pos );
			if( obj != null )
				_objs.Add( obj );
			
//			Debug.Log( "forward x: " + x );
		}		
	}
	
	void MoveBackward()
	{
		var fObj = _objs.FirstOrDefault();
		var lObj = _objs.LastOrDefault();
		
		lObj.transform.position = new Vector3( fObj.transform.position.x - lObj.GetComponent<MeshRenderer>().bounds.size.x, 0F, 0F );
		
		_objs.Sort( (a, b) => a.transform.position.x < b.transform.position.x ? -1 : 1 );
//		foreach( var obj in _objs )
//			obj.transform.position -= new Vector3( speed * Time.deltaTime, 0F, 0F );
	}
	
	void MoveForward()
	{
		var fObj = _objs.FirstOrDefault();
		var lObj = _objs.LastOrDefault();
		
		fObj.transform.position = new Vector3( lObj.transform.position.x + lObj.GetComponent<MeshRenderer>().bounds.size.x, 0F, 0F );
		
		_objs.Sort( (a, b) => a.transform.position.x < b.transform.position.x ? -1 : 1 );
//		foreach( var obj in _objs )
//			obj.transform.position += new Vector3( speed * Time.deltaTime, 0F, 0F );		
	}
	
	void InitWorldPieces()
	{		
	}
	
	GameObject AddTile( GameObject prefab, Vector3 position )
	{
		var piece = Instantiate( prefab, position, prefab.gameObject.transform.rotation ) as GameObject;
				
		// add npc to tile		
//		var maxIndex = npcs.Length - 1;
//		if( maxIndex >= 0 )
//		{
//			var prefNPC = npcs[Random.Range( 0, maxIndex )];
//			if( prefNPC != null )
//			{
//				position.x += 20F;
//				position.y = 0.5f;
//				var npc = Instantiate( prefNPC, position, prefNPC.gameObject.transform.rotation ) as GameObject;
//				_npcs.Add( npc );
//			}
//		}

		for( var i = 0; i < items.Length; i++ )
		{
			var item = items[i];

			_items.AddRange( item.Spawn( piece ));
		}

				
		return piece as GameObject;				
	}
}
