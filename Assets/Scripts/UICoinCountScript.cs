using UnityEngine;
using System.Collections;

public class UICoinCountScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		UILabel label = GetComponent<UILabel> ();
		label.text = "";



	
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject _player = GameObject.FindWithTag ("Player");
		UILabel label = GetComponent<UILabel> ();
		label.text = (string)_player.GetComponent<Player>().gameData.PlayerStats.Coins.ToString();
	}
}
