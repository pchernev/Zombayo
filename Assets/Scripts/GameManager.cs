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
        gameData.PlayerStats.Distance = 0;
        SaveLoadGame.SaveData(this.gameData);
    }

    public void EndGame()
    {
        if (this.isGameOver == false)
        {
            gameData.PlayerStats.Distance = (int)gameObject.transform.position.x;
            stats["Coins: "] = gameData.PlayerStats.Coins;
            stats["Score: "] = gameData.PlayerStats.Points;
            stats["Distance: "] = gameData.PlayerStats.Distance;
            stats["MaxDistance: "] = gameData.PlayerStats.MaxDistance;
            Debug.Log("Game ended");
            this.isGameOver = true;
            DisablePanelsExcept("End Scores");

            EnablePanel("End Scores");
          
            if (gameData.PlayerStats.MaxDistance < gameData.PlayerStats.Distance)
            {
                gameData.PlayerStats.MaxDistance = gameData.PlayerStats.Distance;
            }
        }
    }

    public void OpenShop()
    {
        if (isGameOver)
        {
            DisablePanel("End Scores");
        }
        EnablePanel("Shop");
        UpdateShop();
    }

    public void CloseShop()
    {
        DisablePanel("Shop");
        stats["Coins: "] = gameData.PlayerStats.Coins;
        stats["Score: "] = gameData.PlayerStats.Points;
        stats["Distance: "] = gameData.PlayerStats.Distance;
        stats["MaxDistance: "] = gameData.PlayerStats.MaxDistance;
        EnablePanel("End Scores");
        if (isGameOver)
        {
          
        }
    }

    public bool UpgradeItem(string itemName) 
    {
        Debug.Log("In GameManager Upgrade Item: " + itemName);
        var item = gameData.ShopItems.FirstOrDefault(x => x.Name == itemName);

        if ((item != null) && (item.MaxUpgradesCount > item.UpgradesCount)
            && (gameData.PlayerStats.Coins >= item.Prices[item.UpgradesCount + 1]))
        {
            item.UpgradesCount++;
            this.gameData.PlayerStats.Coins -= item.Prices[item.UpgradesCount];
            UpdateShop();
            return true;
        }
        return false;
    }

    public void UpdateShop() 
    {
       var labelForCoins = GameObject.Find("Coins").GetComponent<UILabel>();
        labelForCoins.text = "Coins: " +  this.gameObject.GetComponent<Player>().gameData.PlayerStats.Coins;
        var gameData = this.gameObject.GetComponent<Player>().gameData;
        for (int i = 0; i < gameData.ShopItems.Length; i++)
        {
            var item = gameData.ShopItems[i];
            UILabel upgradeLabel = GameObject.Find(item.Name).transform
                .FindChild("Label").GetComponent<UILabel>();
            UIImageButton upgradeButton = GameObject.Find(item.Name).GetComponent<UIImageButton>();
            if (item.MaxUpgradesCount <= item.UpgradesCount)
            {
                upgradeLabel.text = "MAX";
                upgradeButton.isEnabled = false;
            }
            else if (item.Prices[item.UpgradesCount + 1] > gameData.PlayerStats.Coins)
            {
                upgradeLabel.text = "Upgrade: " + item.Prices[item.UpgradesCount + 1];
                upgradeButton.isEnabled = false;
            }
            else
            {
                upgradeLabel.text = "Upgrade: " + item.Prices[item.UpgradesCount + 1];
                upgradeButton.isEnabled = true;
            }
            // update stars
            var stars = GameObject.Find(item.Name).transform.FindChild("Stars");
            for (int j = 0; j < item.UpgradesCount; j++)
            {
                var starOn = stars.transform.FindChild("StarOn " + j);
                var starOff = stars.transform.FindChild("StarOff " + j);
                starOn.GetComponentInChildren<UISprite>().enabled = true;
                starOff.GetComponentInChildren<UISprite>().enabled = false;
            }

        }       
    }

	public void MuteDemuteSound()
	{
		if (AudioListener.pause == false) {
						AudioListener.pause = true;
				} 
		else {
			AudioListener.pause = false;
				}
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
