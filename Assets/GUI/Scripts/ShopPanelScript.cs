using UnityEngine;
using System.Collections;

public class ShopPanelScript : MonoBehaviour {

    GameManager gm;
    // Use this for initialization
    void Start()
    {
        gm = GameObject.Find("Player").GetComponent<GameManager>();
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
