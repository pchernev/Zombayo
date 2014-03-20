using UnityEngine;
using System.Collections;
using System.Linq;

public class ShopPanelScript : MonoBehaviour {

    GameManager gm;
    GameData gameData;
    UILabel labelForCoins;

    void Start()
    {
        gm = GameObject.Find("Player").GetComponent<GameManager>();
        gameData = GameObject.Find("Player").GetComponent<Player>().gameData;
        //labelForCoins = GameObject.Find("Coins").GetComponent<UILabel>();
        //labelForCoins.text += gameData.PlayerStats.Coins;

        var stars = GameObject.Find("Magnet").transform.FindChild("Stars");
        var itemUpgraded = gameData.ShopItems.FirstOrDefault(x => x.Name == "Magnet").UpgradesCount;
        for (int i = 0; i < itemUpgraded; i++)
        {
             stars.FindChild("StarOff " + i).GetComponent<UISprite>().enabled = false;
             stars.FindChild("StarOn " + i).GetComponent<UISprite>().enabled = true;
        }               
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
        Debug.Log("Clicked");
        bool isSuccess = gm.UpgradeItem("Magnet");
        if (isSuccess)
        {
           var stars = GameObject.Find("Magnet").transform.FindChild("Stars");
           var itemUpgraded = gameData.ShopItems.FirstOrDefault(x => x.Name == "Magnet");
           stars.FindChild("StarOff " + (itemUpgraded.UpgradesCount - 1)).GetComponent<UISprite>().enabled = false;
           stars.FindChild("StarOn " + (itemUpgraded.UpgradesCount - 1)).GetComponent<UISprite>().enabled = true;
           Debug.Log("Successs upgrading the magnet");

        }
        else
	    {
            // todo: alert message for the user that no success in upg.
	    }
    }
}
