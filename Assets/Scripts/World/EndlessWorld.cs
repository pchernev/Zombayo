using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndlessWorld : MonoBehaviour
{
  	public float speed = 8f; 
    public float countdown = 3.0f;
	
	public GameObject[] prefabs;
	
	private List<GameObject> _objs;
	private float cameraSize = 400.0f;
	
	private GameObject _player;
			
	// Use this for initialization
	void Start ()
	{
		_player = GameObject.FindWithTag( "Player" );
		if( _player == null )
			Debug.Log( "Player object not found" );
		
		_objs = new List<GameObject>();
		
		var p = new Vector3( 0F, 0F, 40F );
		foreach( var prefab in prefabs )
		{			
			var obj = Instantiate( prefab, p, prefab.gameObject.transform.rotation ) as GameObject;
			_objs.Add( obj );
			
			var piece = prefab.GetComponent<WorldPiece>();
			p.x += piece.getBounds().size.x;//mesh.bounds.size.x;
			
			Debug.Log( string.Format( "pos: {0} size: {1}", p.ToString(), piece.bounds.size.ToString() ));
			
//			mesh.enabled = false;
		}
		
		InitWorldPieces();
	}
	
	// Update is called once per frame
	void Update ()
	{
		var fObj = _objs.FirstOrDefault();
		var lObj = _objs.LastOrDefault();
		
		var fMesh = fObj.GetComponent<MeshRenderer>();
		var lMesh = lObj.GetComponent<MeshRenderer>();
		
		var lCamX = transform.position.x - cameraSize;
		var rCamX = transform.position.x + cameraSize;
		
		if( fObj.transform.position.x + 100F < lCamX )
			MoveRight();
		else if( lObj.transform.position.x > rCamX )
			MoveLeft();
			
//		if( Input.GetKeyDown( KeyCode.RightArrow ))
//			MoveRight();
//		else if( Input.GetKeyDown( KeyCode.LeftArrow ))			
//			MoveLeft();
	}
	
	void MoveLeft()
	{
		var fObj = _objs.FirstOrDefault();
		var lObj = _objs.LastOrDefault();
		
		lObj.transform.position = new Vector3( fObj.transform.position.x - lObj.GetComponent<MeshRenderer>().bounds.size.x, 0F, 0F );
		
		_objs.Sort( (a, b) => a.transform.position.x < b.transform.position.x ? -1 : 1 );
//		foreach( var obj in _objs )
//			obj.transform.position -= new Vector3( speed * Time.deltaTime, 0F, 0F );
	}
	
	void MoveRight()
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
	
}
