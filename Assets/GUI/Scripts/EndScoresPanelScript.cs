using UnityEngine;
using System.Collections;
using System.Linq;
public class EndScoresPanelScript : MonoBehaviour
{
    private GameObject player;
	private GameManager gameMgr;
    UILabel labelForCoins;

    void Start()
    {
        player = GameObject.Find("Player");
		gameMgr = player.GetComponent<GameManager>();
    }

    public void ShowShopPanel() 
    {        
        gameMgr.OpenShop();
        gameMgr.UpdateShop();
    }
    public void RestartGame() 
    {
		gameMgr.RestartGame();
    }
}
