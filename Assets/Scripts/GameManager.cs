using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Assets.Scripts;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;

public class GameManager : MonoBehaviour
{
    public bool isGameOver;
    public bool isGamePaused;

    
    //private GameObject player;
    private Statistics playerCurrentStats;
    private Dictionary<string, GameObject> gamePanels = new Dictionary<string, GameObject>();
    private ShopItem[] shopItems;

    #region constants
        const int MAX_NUMBER_SHOP_UPGRADE = 6;
    #endregion

    void Start()
    {

        playerCurrentStats = this.gameObject.GetComponent<Player>().stat;
        shopItems = LoadShopItems();
        Time.timeScale = 1f;
        this.isGameOver = false;
        this.isGamePaused = false;
        LoadAllPanels();
        DisablePanelsExcept("In Game");
        EnablePanel("In Game");
        //doctorShark = GameObject.Find("Dr.Fishhead");      
    }
   
    public void PauseGame()
    {
            Time.timeScale = 0f;
            this.isGamePaused = true;
            DisablePanelsExcept("Pause");
            EnablePanel("Pause");
    }

    public void ResumeGame()
    {
        if (isGamePaused && !isGameOver)
        {
            Time.timeScale = 1f;
            this.isGamePaused = false;
            DisablePanelsExcept("In Game");
            Debug.Log("In ResumeGame -> GameManager");
        }
        EnablePanel("In Game");
    }

    public void RestartGame()
    {

        Time.timeScale = 1f;
        Application.LoadLevel("IngameScene01");

        isGamePaused = false;
        isGameOver = false;
        EnablePanel("In Game");
    }

    public void EndGame()
    {
        if (this.isGameOver == false)
        {
            Debug.Log("Game ended");
            this.isGameOver = true;
            DisablePanelsExcept("End Scores");
            EnablePanel("End Scores");
            var stats = gameObject.GetComponent<Player>().stat;
            stats.Distance = (int)gameObject.transform.position.x;
            SaveLoadGame.SaveStats(stats);
        }
    }

    public void OpenShop()
    {
        if (isGameOver)
        {
            DisablePanel("End Scores");
        }
        EnablePanel("Shop");
    }

    public void CloseShop()
    {
        DisablePanel("Shop");
        EnablePanel("End Scores");
        if (isGameOver)
        {
          
        }
    }
  
    private ShopItem[] LoadShopItems() 
    {
        // todo load from saved file
        ShopItem[] result = new ShopItem[1];
        result[0] = new ShopItem { Name = "Magnet", Price = 200, UpgradesCount = 0, MaxUpgradesCount = 6, NextUpgradePricePercentage = 2 };
        return result;
    }

    public bool UpgradeItem(string itemName) 
    {
        var item = shopItems.FirstOrDefault(x => x.Name == itemName);
        if (item != null && item.MaxUpgradesCount < item.UpgradesCount
            &&  playerCurrentStats.Coins >= item.Price)
        {
            this.playerCurrentStats.Coins -= item.Price;
            switch (itemName)
            {
                case "Magnet": UpgradeMagnet();         // add here new upgrades 
                    break;
                default:
                    break;
            }
            item.UpgradesCount++;
            item.Price *= item.NextUpgradePricePercentage;
            Debug.Log("Upgraded item: " + item.Name);
            return true;
        }
        
        return false;
    }

    #region shopfunctions
    private void UpgradeMagnet()
    {
        gameObject.GetComponent<SphereCollider>().radius = 2000f; 
    }
    #endregion
    #region helpers
    private void LoadAllPanels()
    {
        var panelsHolder = GameObject.Find("Anchor").transform;
        foreach (Transform child in panelsHolder)
        {
            gamePanels.Add(child.transform.name, child.transform.gameObject);
        }
    }
    
    private void DisablePanel(string key)
    {
        if (gamePanels.ContainsKey(key))
        {
            gamePanels[key].active = false;
        }
    }

    private void DisablePanelsExcept(string panelNames)
    {
        var namesSplited = panelNames.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

        gamePanels.Where(x => !namesSplited.Contains(x.Key) && gamePanels.ContainsKey(x.Key) && x.Value.active == true)
             .ToList<KeyValuePair<string, GameObject>>().ForEach(x => { x.Value.active = false; });
    }

    private void EnablePanel(string key)
    {
        if (gamePanels.ContainsKey(key))
        {
            Debug.Log("Contains key: " + key);
            gamePanels[key].SetActive(true);
        }
    }
    #endregion
}
