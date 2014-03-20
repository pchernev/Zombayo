using UnityEngine;
using System.Collections;

public class PausePanelScript : MonoBehaviour {
    private GameObject player;
	void Start () {
        player = GameObject.Find("Player");
	}
	
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
