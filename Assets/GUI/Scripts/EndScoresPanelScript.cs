using UnityEngine;
using System.Collections;

public class EndScoresPanelScript : MonoBehaviour
{
    private GameObject player;
	private GameManager gameMgr;
    UILabel labelForCoins;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
		gameMgr = player.GetComponent<GameManager>();
    }

    public void ShowShopPanel() 
    {        
        gameMgr.OpenShop();
        labelForCoins = GameObject.Find("Coins").GetComponent<UILabel>();
        labelForCoins.text = "Coins: " + player.GetComponent<Player>().gameData.PlayerStats.Coins;
    }
    void Update() {
        
    }
    public void RestartGame() 
    {
		gameMgr.RestartGame();
    }
}
