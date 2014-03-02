using UnityEngine;
using System.Collections;

public class PausePanelScript : MonoBehaviour {
    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ExitToMainMenu() 
    {
        Application.LoadLevel("MainMenuScene");
    }

    public void Resume() 
    {
        player.GetComponent<GameManager>().ResumeGame();
    }

    public void Restart()
    {
        player.GetComponent<GameManager>().RestartGame();
    }
}
