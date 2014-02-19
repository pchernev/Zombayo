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
        isGameOver = false;
        LoadAllPanels();
        DisablePanelsExcept("In Game");
        EnablePanel("In Game");
        doctorShark = GameObject.Find("Dr.Fishhead");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }
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
        if (!isGamePaused && !isGameOver)
        {
            Time.timeScale = 0f;
            isGamePaused = true;
            DisablePanelsExcept("Pause");
            EnablePanel("Pause");
        }

    }

    public void ResumeGame()
    {
        if (isGamePaused && !isGameOver)
        {
            Time.timeScale = 1f;
            isGamePaused = false;
            DisablePanelsExcept("In Game");
            EnablePanel("In Game");
        }
    }

    public void RestartGame()
    {
        if (isGameOver || isGamePaused)
        {
            isGameOver = false;
            DisablePanelsExcept("In Game");
            EnablePanel("In Game");
            this.gameObject.SendMessage("Reset");
            doctorShark.SendMessage("Reset");

            doctorShark.SendMessage("Start");
            if (isGamePaused)
            {
                ResumeGame();
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
