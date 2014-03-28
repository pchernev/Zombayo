using UnityEngine;
using System.Collections;

public class ProgressBarButtonScript : MonoBehaviour {
	public GameObject target;
	//private UISlider slider = target.GetComponent<UISlider> ();
	private bool use;
	public float speedOfuse ;

	// Use this for initialization
	void Awake() {


	}
	void Start()
	{
	 
	
	}
	
	// Update is called once per frame
	void Update() {
		if (use == true) {
						target.GetComponent<UISlider> ().value -= speedOfuse;
				}
	


	}

	void OnPress(bool isDown){
	if (isDown == true) {
			Debug.Log ("Presseed");
						
			use = true;
						

				} else {
			use = false;
		
			Debug.Log ("Released");
				}
	}

}
