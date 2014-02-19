using UnityEngine;
using System.Collections;

public class DestroyOnCollision : MonoBehaviour {




	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			Destroy(gameObject);
		}
	}
}
