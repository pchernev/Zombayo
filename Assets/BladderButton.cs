using UnityEngine;
using System.Collections;
using System.Linq;


public class BladderButton : MonoBehaviour {
	GameObject _player;
	bool disableButton = false;
	public float disableTime;
	public PhysicMaterial bounceMaterial;
	private bool usedBubbleGum;
	private Quaternion rot;

	// Use this for initialization
	void Start () {
	
	}
	void ResetRotation()
	{
		usedBubbleGum = false;
		_player = GameObject.FindWithTag ("Player");
		_player.transform.rotation = rot;
	}
	// Update is called once per frame
	void Update () {
		if (usedBubbleGum == true) {
						_player = GameObject.FindWithTag ("Player");

						rot = _player.transform.rotation;
						_player.transform.rotation = Quaternion.Euler (0, 0, 0);
			Invoke("ResetRotation",disableTime);

				}
		
	
	}
	void OnClick()
	{
		if (!disableButton)
        {
		    _player = GameObject.FindWithTag("Player");
		    var item = _player.GetComponent<Player> ().gameData.ShopItems.FirstOrDefault (x => x.Name == "Bladder");
		    UILabel label = GetComponent<UILabel> ();
		    if (item.UpgradesCount != 0)
            {
				Animator _animator = _player.GetComponent<Animator>();
				if(_animator.GetBool("Fly")==true)
				{
		    	item.UpgradesCount -= 1;
		        disableButton = true;
		        Invoke("EnableButton",disableTime);

				_player.collider.material = bounceMaterial;

				

				_animator.SetBool("Fly", false);
				_animator.SetTrigger("BubbleGum");


				Debug.Log ("Bubble Gum");
				usedBubbleGum = true;
				}
		    }
		}
	}
	void EnableButton()
	{
		disableButton = false;
	}
}
