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
	
	// Use this for initialization
	void Start ()
	{
		_objs = new List<GameObject>();
		
		var p = new Vector3( 0F, 0F, 0F );
		foreach( var prefab in prefabs )
		{			
			var obj = Instantiate( prefab, p, prefab.gameObject.transform.rotation ) as GameObject;
			_objs.Add( obj );
			
			var mesh = prefab.GetComponent<MeshRenderer>();
			p.x += mesh.bounds.size.x;
			mesh.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( Input.GetKeyDown( KeyCode.RightArrow ))
			MoveRight();
		else if( Input.GetKeyDown( KeyCode.LeftArrow ))			
			MoveLeft();
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
}
