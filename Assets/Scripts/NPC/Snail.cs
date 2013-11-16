using UnityEngine;
using System.Collections;

public class Snail : BaseItem
{
	public float speed = 0.01f;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		var pos = transform.position;
		pos.x -= speed;
		transform.position = pos;
	}
}
