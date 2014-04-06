using UnityEngine;
using System.Linq;

public class BladderCount : MonoBehaviour {
	GameObject _player;
	private bool available;
	public GameObject target;
	public Color color;
    public int itemUpgrades = 20;

    BladderButton bladderBtn;

    void Awake() 
    {
        bladderBtn = GameObject.Find("Ingame Button BubbleGum").GetComponent<BladderButton>();
        itemUpgrades = bladderBtn.timesToUse;
    }

	void Start ()
    {
		_player = GameObject.FindWithTag("Player");       
	}
	
	void Update () 
    {
        UILabel label = GetComponent<UILabel>();
        var _sprite = target.GetComponent<UISprite>();
        UIWidget widget = _sprite.GetComponent<UIWidget>();
        itemUpgrades = bladderBtn.timesToUse;
        label.text = itemUpgrades + "";		

        if (itemUpgrades == 0)
        {
            label.text = "Disabled";
            widget.color = this.color;
        }
	}
}

