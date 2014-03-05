using UnityEngine;
using System.Collections;

public class EndScoresPanelScript : MonoBehaviour
{
    private GameObject player;
	private GameManager gameMgr;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
		gameMgr = player.GetComponent<GameManager>();
    }

    public void ShowShopPanel() 
    {
        gameMgr.OpenShop();
    }

    public void RestartGame() 
    {
		gameMgr.RestartGame();
    }
}
