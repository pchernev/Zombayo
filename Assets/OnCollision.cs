
using UnityEngine;
using System.Collections;

public class OnCollision : MonoBehaviour {
	public bool gotMagnet;
	public int magnetPower;
	public GameObject explosion;
	long score = 0;
	public float coinsSpeed;
	
	
	// Use this for initialization
	void Start()
	{
		
		
		if (gotMagnet == true) {
			(gameObject.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider).radius *= magnetPower;
		}
	}
	
	
	
	void OnTriggerStay (Collider collider) {

		if (collider.gameObject.CompareTag ("Player")) {			
			this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,collider.gameObject.transform.position,Time.deltaTime*coinsSpeed);
			if(collider.gameObject.transform.position == this.gameObject.transform.position)
			{
				Instantiate(explosion,transform.position,transform.rotation);
				Destroy(this.gameObject);
				score+=1;

			}
		}


	}


	}

