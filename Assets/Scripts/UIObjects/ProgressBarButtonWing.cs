using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProgressBarButtonWing : MonoBehaviour {
	public GameObject target;
	public UILabel label;
	//private UISlider slider = target.GetComponent<UISlider> ();
	public bool use;
	public float speedOfuse ;
	public bool available = false;
	GameObject _player;
	Animator _animator;
	public Color color;
	private bool lockPositionY;

	
	// Use this for initialization
	void Awake() {
		
		
	}
	void Start()
	{
		var _slider = target.GetComponent<UISlider> ();
		UIWidget widget = _slider.foreground.GetComponent<UIWidget>();
		_player = GameObject.FindWithTag("Player");
		
		var item = _player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Wings");
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
			_animator = _player.GetComponent<Animator>();
						var tmp = _player.transform.position;
						if (use == true) {
				var item =_player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Wings");
				target.GetComponent<UISlider> ().value -= Time.deltaTime/item.Values[item.UpgradesCount];
				_player.transform.rotation = Quaternion.Euler (0, 0, 0);
								
				if(_animator.GetBool("Fly")==true){

						
								_animator.SetBool("Fly", false);
					_animator.SetBool("FlyWings",true);

			
								_player.rigidbody.constraints = RigidbodyConstraints.FreezePositionY;

				}
						} else {

								_player.rigidbody.constraints = RigidbodyConstraints.None;
				_animator.SetBool("FlyWings",false);
			

			

						}
			if (target.GetComponent<UISlider> ().value <= 0) {
				use = false;
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
