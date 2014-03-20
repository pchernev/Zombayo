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
    DinamicStatistics stats = new DinamicStatistics();
    
    //private GameObject player;
    public GameData gameData;
    private Dictionary<string, GameObject> gamePanels = new Dictionary<string, GameObject>();

    void Start()
    {
        gameData = this.gameObject.GetComponent<Player>().gameData;

        Time.timeScale = 1f;
        this.isGameOver = false;
        this.isGamePaused = false;
        LoadAllPanels();
        DisablePanelsExcept("In Game");
        EnablePanel("In Game");
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

            stats["Coins: "] = gameData.PlayerStats.Coins;
            stats["Score: "] = gameData.PlayerStats.Points;
            stats["Distance: "] = gameData.PlayerStats.Distance;

            Debug.Log("Game ended");
            this.isGameOver = true;
            DisablePanelsExcept("End Scores");

            EnablePanel("End Scores");
           
            if (gameData.PlayerStats.Distance < (int)gameObject.transform.position.x)
            {
                gameData.PlayerStats.Distance = (int)gameObject.transform.position.x;
            }
            SaveLoadGame.SaveData(gameData);
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

    public bool UpgradeItem(string itemName) 
    {
        var item = gameData.ShopItems.FirstOrDefault(x => x.Name == itemName);

        Debug.Log("Trying to upg the magnet");
        Debug.Log("Item.MaxUpgCount: " + item.MaxUpgradesCount);
        Debug.Log("Item.Name: " + item.Name);
        Debug.Log("Item.UpgradesCount: " + item.UpgradesCount);
        Debug.Log("Item.Prices.Length: " + item.Prices.Length);
        Debug.Log("Item.Values.Length: " + item.Values.Length);

        if ((item != null) && (item.MaxUpgradesCount > item.UpgradesCount)
            && (gameData.PlayerStats.Coins >= item.Prices[item.UpgradesCount + 1]))
        {
            item.UpgradesCount++;
            this.gameData.PlayerStats.Coins -= item.Prices[item.UpgradesCount];
            Debug.Log("Upgraded item: " + item.Name);
            return true;
        }
        Debug.Log("Can't UPGRADE ITEM");
        return false;
    }

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
            //Debug.Log("Contains key: " + key);
            gamePanels[key].SetActive(true);
        }
    }
    #endregion
}
