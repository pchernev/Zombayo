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
        if (gm.isGamePaused)
        {
            gm.PauseGame();
            UIPlayAnimation anim = new UIPlayAnimation() { target = GameObject.Find("Pause").GetComponent<Animation>(), clipName="igm_show" };
            anim.Play(true);
        }
        else if(gm.isGameOver)
        {
            gm.EndGame();
            UIPlayAnimation anim = new UIPlayAnimation() { target = GameObject.Find("End Scores").GetComponent<Animation>(), clipName = "igm_show" };
            anim.Play(true);
        }
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
