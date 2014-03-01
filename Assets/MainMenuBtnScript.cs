using UnityEngine;
using System.Collections;

public class MainMenuBtnScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick() 
    {
        Application.LoadLevel("MainMenuScene");
    }
}
