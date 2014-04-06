using UnityEngine;
using System.Collections;
using System.Linq;

public class ProgressBarFart : MonoBehaviour {
	private UISlider _slider;
	public Color color;
	GameObject _player;
	
	
	void Awake()
	{
		_slider = GetComponent<UISlider> ();
		if (_slider == null) {
			Debug.Log ("Could not found the UISlider component !");
			return;
		}
		
	}
	void Start () {
		
		
		
	}
	void Update()
	{
		_player = GameObject.FindWithTag("Player");
		var item = _player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Fart");
		if (item.UpgradesCount == 0) {
			
			var children = gameObject.GetComponentsInChildren<UISprite> ();
			
			
			foreach (UISprite child in children) {
				var c = child.GetComponent<UIWidget> ();
				c.color = this.color;
				
			}
		}
	}
	
}
