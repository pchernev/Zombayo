using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ArmorCount : MonoBehaviour {
	GameObject _player;

	// Use this for initialization
	void Start () {
		_player = GameObject.FindWithTag("Player");
		var item = _player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Bladder");
		var ammountOfArmors = item.UpgradesCount;
		UILabel label = GetComponent<UILabel> ();
		label.text = "";
		label.text += ammountOfArmors.ToString ();


	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
