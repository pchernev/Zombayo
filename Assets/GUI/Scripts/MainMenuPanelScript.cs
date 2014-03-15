using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Threading;

public class MainMenuPanelScript : MonoBehaviour {

	// todo: load or destroy saved shop panel upgrades
	void Start () {
        var wtf = this.GetComponent<Animation>();
        wtf.Play("igm_show");
        Debug.Log("WTFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF " + wtf.GetType());
	}

    public void StartGame() 
    {        
        var stats = new Statistics();
        SaveLoadGame.SaveStats(stats);
        Application.LoadLevel("IngameScene01");
    }

    public void Continue() 
    {
        Application.LoadLevel("IngameScene01");
    }
}
