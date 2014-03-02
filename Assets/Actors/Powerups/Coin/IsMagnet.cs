using UnityEngine;
using System.Collections;

public class IsMagnet : MonoBehaviour {
	public bool isMagnet;
	public int magnetPower;
	// Use this for initialization
	void Start () {
		if (isMagnet == true) {
						var capsuleCollider = GetComponent ("CapsuleCollider") as CapsuleCollider;
						
						capsuleCollider.radius *= magnetPower;
				}
	}
	

}
