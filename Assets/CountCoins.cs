using UnityEngine;
using System.Collections;

public class CountCoins : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Zombayo") {
			return;
		}
		
		Destroy (gameObject);
	}
}
