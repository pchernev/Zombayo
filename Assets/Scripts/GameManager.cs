using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class GameManager : MonoBehaviour {

    Dictionary<string, GameObject> gamePanels = new Dictionary<string, GameObject>();
	void Start ()
    {
        loadAllPanels();
        disablePanelsExcept("In Game");
	}
	   
	void Update ()
    {
	
	}

    void FixedUpdate() 
    {
        
    }

    private void loadAllPanels()
    {
        var panelsHolder = GameObject.Find("Anchor").transform;
        foreach (Transform child in panelsHolder)
        {
            gamePanels.Add(child.transform.name, child.transform.gameObject);
        }
    }

    private void disablePanelsExcept(string panelNames)
    {
        var namesSplited = panelNames.Split(new string[] { ", "}, StringSplitOptions.RemoveEmptyEntries);

       gamePanels.Where(x => !namesSplited.Contains(x.Key) && gamePanels.ContainsKey(x.Key) && x.Value.active == true)
            .ToList<KeyValuePair<string, GameObject>>().ForEach(x => { x.Value.active = false; });
    }
}
