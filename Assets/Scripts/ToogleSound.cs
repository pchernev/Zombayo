using UnityEngine;
using System.Collections;

public class ToogleSound : MonoBehaviour {

	void OnClick()
	{
	
			if (AudioListener.pause == false) {
				AudioListener.pause = true;
			} 
			else {
				AudioListener.pause = false;
			}

	}
}
