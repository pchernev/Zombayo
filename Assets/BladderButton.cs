using UnityEngine;
using System.Collections;
using System.Linq;


public class BladderButton : MonoBehaviour {
	GameObject _player;
	bool disableButton = false;
	public float disableTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
		    	item.UpgradesCount -= 1;
		        disableButton = true;
		        Invoke("EnableButton",disableTime);		    
		    }
		}
	}
	void EnableButton()
	{
		disableButton = false;
	}
}
