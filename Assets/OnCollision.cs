
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
		


	}
	void Update()
	{
		if (gotMagnet == true) {
						GameObject _player = GameObject.FindWithTag ("Player");
						var distance = Vector3.Distance (this.gameObject.transform.position, _player.transform.position);
						if (distance <= magnetPower) {
								iTween.MoveUpdate (this.gameObject, _player.transform.position, coinsSpeed);
						}

				}
		}
	void OnTriggerEnter (Collider collider) {
		
				if (collider.gameObject.CompareTag ("Player")) {
						Destroy (this.gameObject);
						Instantiate (explosion, transform.position, transform.rotation);
				}
		}
		
		


		


	
	
	
	

	


	}

