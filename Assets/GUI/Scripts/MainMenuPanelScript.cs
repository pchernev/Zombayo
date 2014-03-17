using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Threading;

public class MainMenuPanelScript : MonoBehaviour {


	void Start () 
    {
 
	}

    public void StartGame() 
    {        
        var gameData = new GameData(true);
        SaveLoadGame.SaveData(gameData);
        Application.LoadLevel("IngameScene01");
    }

    public void Continue() 
    {
        Application.LoadLevel("IngameScene01");
    }
}
