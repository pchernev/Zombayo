using UnityEngine;
using System.Collections;

public class DolphinSpawn : MonoBehaviour
{

		public GameObject dolphin;
		public int ammount;
	
		void Start ()
		{
			for (int i = 0; i<ammount; i++) {
					Vector3 spawnLocation = new Vector3 (Random.Range (1.0f, 300.0f), Random.Range (1.0f, 10.0f), 0.0f);

					GameObject SpawnLocation = (GameObject)Instantiate (dolphin, spawnLocation, Quaternion.Euler (0.0f, -90.0f, 0.0f));
			}
		}

		// Update is called once per frame
		void Update ()
		{
		}
}
