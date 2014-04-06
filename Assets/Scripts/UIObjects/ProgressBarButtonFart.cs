using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProgressBarButtonFart : MonoBehaviour {
	public GameObject target;
	public UILabel label;
	//private UISlider slider = target.GetComponent<UISlider> ();
	public bool use;
	public float speedOfuse ;
	private bool available = false;
	GameObject _player;
	Animator _animator;

	private bool usedFart;
	private Quaternion rot;
	public Vector3 force;




	// Use this for initialization
	void Awake() {


	}
	void Start()
	{
	
		var _slider = target.GetComponent<UISlider> ();
		UIWidget widget = _slider.foreground.GetComponent<UIWidget>();

	
		_player = GameObject.FindWithTag("Player");
		_animator = _player.GetComponent<Animator>();
	
		var item = _player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Fart");
		if (item.UpgradesCount == 0) {
						label.text = "Disabled";


				} 
		else {
			label.text ="";
			available = true;
				}
	

	
	}

	void Update()
	{
		if (_animator.GetBool ("Fly") == false || target.GetComponent<UISlider> ().value == 0) {
						gameObject.collider.enabled = false;
			
		} 
		else {gameObject.collider.enabled= true;

				}
		var item = _player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Fart");
		if (item.UpgradesCount == 0) {
			gameObject.GetComponent<UIButton>().isEnabled = false;
			var colorr = gameObject.GetComponent<UIButton>().disabledColor;
			var children = gameObject.GetComponentsInChildren<UISprite>();
			
			
			foreach (UISprite child in children) {
				var c = child.GetComponent<UIWidget>();
				c.color = colorr;
				
			}
		}
	}
	void ResetRotation()
	{
		usedFart= false;
		_player = GameObject.FindWithTag ("Player");
		_player.transform.rotation = rot;
	}

	void FixedUpdate() {

		if(usedFart == true){
			_player = GameObject.FindWithTag ("Player");
			
			rot = _player.transform.rotation;
			
			_player.transform.rotation = Quaternion.Euler (0, 0, 0);
			_player.rigidbody.AddRelativeForce(force);
		}
		if (available == true) {
						if (use == true) {
								if (_animator.GetBool ("Fly") == true) {

										
					_animator.SetBool("Fly", false);
					_animator.SetBool("Fart",true);






								}
				var item = _player.GetComponent<Player> ().gameData.ShopItems.FirstOrDefault (x => x.Name == "Fart");
				target.GetComponent<UISlider> ().value -= Time.deltaTime/item.Values[item.UpgradesCount];
						}
			else{
				_animator.SetBool("Fart",false);
			}
			if (target.GetComponent<UISlider> ().value <= 0) {
				use = false;
				Invoke("ResetRotation",0.2f);
			}
				}

	}

	void OnPress(bool isDown){
	if (isDown == true) {
			Debug.Log ("Presseed");
						
			use = true;
			usedFart = true;



				} else {
			use = false;
			Invoke("ResetRotation",0.2f);
			Debug.Log ("Released");
				}
	}

}
