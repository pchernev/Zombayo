using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Threading;

public class MainMenuPanelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Animation>().Play("igm_show");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame() 
    {
        var stats = new Statistics();
        SaveLoadGame.SaveStats(stats);
        Application.LoadLevel("IngameScene01");
    }

    public void Continue() 
    {
        Application.LoadLevel("IngameScene01");
    }
}
