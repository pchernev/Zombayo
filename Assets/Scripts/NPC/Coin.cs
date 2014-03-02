using UnityEngine;
using System.Collections;

public class Coin : BaseItem
{
	public GameObject coin;
	public int ammount;

	void Start()
	{
		for (int i = 0; i<ammount; i++) {
				Vector3 spawnLocation = new Vector3 (Random.Range (1.0f, 300.0f), Random.Range (1.0f, 10.0f), 0.0f);
				GameObject SpawnLocation = (GameObject)Instantiate (coin, spawnLocation, Quaternion.identity);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Player") {
			Destroy(other.gameObject);
		}
	}
}
	
	


