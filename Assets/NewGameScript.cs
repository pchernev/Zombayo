using UnityEngine;
using System.Collections;

public class NewGameScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick() 
    {
        Application.LoadLevel("IngameScene01");
    }
}
