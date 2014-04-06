using UnityEngine;
using System.Collections;

public class UICoinCountScript : MonoBehaviour {

	void Start () 
    {
		UILabel label = GetComponent<UILabel> ();
		label.text = "";	
	}
		
	void Update ()
    {
		GameObject _player = GameObject.FindWithTag("Player");
		UILabel label = GetComponent<UILabel> ();
		label.text = (string)_player.GetComponent<Player>().gameData.PlayerStats.Coins.ToString();
	}
}
