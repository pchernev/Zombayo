using UnityEngine;
using System.Collections;

public class ProgressBars : MonoBehaviour {
	private UISlider _slider;

	void Awake()
	{
		_slider = GetComponent<UISlider> ();
		if (_slider == null) {
			Debug.Log ("Could not found the UISlider component !");
			return;
				}

		}
	void Start () {



	}
	void Update()
	{

	}





	


}
