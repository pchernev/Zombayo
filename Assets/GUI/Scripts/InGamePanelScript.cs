using UnityEngine;
using System.Collections;

public class InGamePanelScript : MonoBehaviour {
    private GameObject player;

	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Pause() 
    {
        player.GetComponent<GameManager>().PauseGame();
    }

    public void UseFart() 
    {
        player.GetComponent<Player>().UseFart();
    }
}
