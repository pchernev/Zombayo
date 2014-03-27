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

        var stars = GameObject.Find("Magnet").transform.FindChild("Stars");
        var itemUpgraded = gameData.ShopItems.FirstOrDefault(x => x.Name == "Magnet" && x.UpgradesCount <= x.MaxUpgradesCount).UpgradesCount;
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

    public void HandleUpgrade(string itemName) 
    {
        Debug.Log("Trying to upgrade item: " + itemName);
     
        bool isSuccess = gm.UpgradeItem(itemName);
    }
}
