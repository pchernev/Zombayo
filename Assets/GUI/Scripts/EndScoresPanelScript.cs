using UnityEngine;
using System.Collections;
using System.Linq;
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

        var magnetItem = player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Magnet");
        UILabel upgradeLabel = GameObject.Find(magnetItem.Name).transform
            .FindChild("Label").GetComponent<UILabel>();
        UIImageButton upgradeButton = GameObject.Find(magnetItem.Name).GetComponent<UIImageButton>();

        if (magnetItem.MaxUpgradesCount <= magnetItem.UpgradesCount)
        {
            upgradeLabel.text = "MAX";
            upgradeButton.isEnabled = false;
        }
        else if (magnetItem.Prices[magnetItem.UpgradesCount + 1] 
            > player.GetComponent<Player>().gameData.PlayerStats.Coins)
        {
            upgradeLabel.text = "Upgrade: " + magnetItem.Prices[magnetItem.UpgradesCount + 1];
            Debug.Log("Not Enought Money");
            upgradeButton.isEnabled = false;
        }
        else
        {
            upgradeLabel.text = "Upgrade: " + magnetItem.Prices[magnetItem.UpgradesCount + 1];
            upgradeButton.isEnabled = true;
        }
    }
    void Update() {
        
    }
    public void RestartGame() 
    {
		gameMgr.RestartGame();
    }
}
