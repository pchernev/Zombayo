using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ArmorCount : MonoBehaviour {
	GameObject _player;
	private bool available;
	public GameObject target;
	public Color color;

	// Use this for initialization
	void Start () {
		_player = GameObject.FindWithTag("Player");
		var item = _player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Armor");
		UILabel label = GetComponent<UILabel> ();

		var _sprite = target.GetComponent<UISprite> ();
		UIWidget widget = _sprite.GetComponent<UIWidget> ();


		if (item.UpgradesCount == 0) {
			widget.color = this.color;
						label.text = "Disabled";
				}
		else {
						label.text += item.UpgradesCount;
				}


	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
