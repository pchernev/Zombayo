using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private bool isGameOver;
    private bool isGamePaused = false;
    private GameObject doctorShark;
    private Dictionary<string, GameObject> gamePanels = new Dictionary<string, GameObject>();
    void Start()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        LoadAllPanels();
        DisablePanelsExcept("In Game");
        EnablePanel("In Game");
        doctorShark = GameObject.Find("Dr.Fishhead");
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

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
        if (isGameOver)
        {
            EnablePanel("End Scores");
        }
    }
    public void EndGame()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            DisablePanelsExcept("End Scores");
            EnablePanel("End Scores");
        }
    }
}
