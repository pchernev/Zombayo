using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProgressBarButtonWing : MonoBehaviour {
	public GameObject target;
	public UILabel label;
	//private UISlider slider = target.GetComponent<UISlider> ();
	private bool use;
	public float speedOfuse ;
	private bool available = false;
	GameObject _player;
	public Color color;
	
	// Use this for initialization
	void Awake() {
		
		
	}
	void Start()
	{
		var _slider = target.GetComponent<UISlider> ();
		UIWidget widget = _slider.foreground.GetComponent<UIWidget>();
		_player = GameObject.FindWithTag("Player");
		
		var item = _player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Wing");
		if (item.UpgradesCount == 0) {
						label.text = "Disabled";
						widget.color = this.color;
				} 
		else {
			label.text ="";
			available = true;
				}
		
		
		
	}
	
	// Update is called once per frame
	void Update() {
		if (available == true) {
						if (use == true) {
								target.GetComponent<UISlider> ().value -= speedOfuse;
						}
				}
		
		
		
	}
	
	void OnPress(bool isDown){
		if (isDown == true) {
			Debug.Log ("Presseed");
			
			use = true;
			
			
		} else {
			use = false;
			
			Debug.Log ("Released");
		}
	}
	
}
