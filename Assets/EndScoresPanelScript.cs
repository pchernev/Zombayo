using UnityEngine;
using System.Collections;

public class EndScoresPanelScript : MonoBehaviour {
    private GameObject player;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
    }

    public void ShowShopPanel() 
    {
        player.GetComponent<GameManager>().OpenShop();
    }

    public void RestartGame() 
    {
        player.GetComponent<GameManager>().RestartGame();
    }
}
