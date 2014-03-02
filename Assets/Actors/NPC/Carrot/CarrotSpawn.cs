using UnityEngine;
using System.Collections;

public class CarrotSpawn : MonoBehaviour {
	public GameObject carrot;
	public int ammount;
	// Use this for initialization
	void Start () {
				for (int i = 0; i<ammount; i++) {
						Vector3 spawnLocation = new Vector3 (Random.Range (1.0f, 245.0f), 0.0f, 0.0f);
			
						GameObject SpawnLocation = (GameObject)Instantiate (carrot, spawnLocation, Quaternion.Euler (0.0f, -90.0f, 0.0f));

				}
		}
	
	// Update is called once per frame
	void Update () {
	
	}
}
