using UnityEngine;
using System.Collections;

public class ShopPanelScript : MonoBehaviour {

    GameManager gm;
    UILabel labelForCoins;
    Statistics stats;
    void Start()
    {
        gm = GameObject.Find("Player").GetComponent<GameManager>();
        stats = GameObject.Find("Player").GetComponent<Player>().stat;
        labelForCoins = GameObject.Find("Coins").GetComponent<UILabel>();
        labelForCoins.text += stats.Coins;
    }

    public void HideShopPanel() 
    {
        gm.CloseShop();
    }

    public void UpgradeMagnet() 
    {
        bool isSuccess = gm.UpgradeItem("Magnet");
        if (isSuccess)
        {
            // todo: change stars here to +1 upg
            Debug.Log("Successs upgrading the magnet");
        }
        else
	    {
            // todo: alert message for the user
	    }
    }
}
