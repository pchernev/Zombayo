using UnityEngine;
using System.Collections;

public class OnCollision : MonoBehaviour {
	public bool gotMagnet;
	public int magnetPower;

	// Use this for initialization
	void Start()
	{

			if (gotMagnet = true) {
			(gameObject.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider).radius *= magnetPower;
			}
		}
	
	// Update is called once per frame

	

	void OnCollisionEnter (Collision col)
	{
		long score = 0;
		if(col.gameObject.tag == "Player")
		{
			Destroy(gameObject);
			score+=1;
		}


	}

}
