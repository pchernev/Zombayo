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
    private bool isGameOver;
    private bool isGamePaused;
    //private GameObject doctorShark;
    private GameObject player;
    private Dictionary<string, GameObject> gamePanels = new Dictionary<string, GameObject>();

    void Start()
    {
        player = GameObject.Find("Player");
        Time.timeScale = 1f;
        this.isGameOver = false;
        this.isGamePaused = false;
        LoadAllPanels();
        DisablePanelsExcept("In Game");
        EnablePanel("In Game");
        //doctorShark = GameObject.Find("Dr.Fishhead");      
    }

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

    public void PauseGame()
    {
  
            Time.timeScale = 0f;
            isGamePaused = true;
            DisablePanelsExcept("Pause");
            EnablePanel("Pause");
        

    }

    public void ResumeGame()
    {
        if (isGamePaused && !isGameOver)
        {
            Time.timeScale = 1f;
            isGamePaused = false;
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
    public void EndGame()
    {
        if (this.isGameOver == false && this.isGamePaused == false)
        {
            this.isGameOver = true;
            DisablePanelsExcept("End Scores");
            EnablePanel("End Scores");
            var stats = player.GetComponent<Player>().stat;
            stats.Distance = (int)player.transform.position.x;
            stats.Coins += (int)(stats.Distance * 1.5f);
            SaveLoadGame.SaveStats(stats);
        }
    }      
}
