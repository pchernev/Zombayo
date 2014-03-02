using UnityEngine;
using System.Collections;

public class Carrot : MonoBehaviour {
	private string a = "op";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter( Collision collision )
	{
		if (collision.gameObject.tag.CompareTo ("Player") == 0) {
			gameObject.audio.Play();
			collision.gameObject.rigidbody.velocity = Vector3.zero;
				}
	}
}
