using UnityEngine;
using System.Linq;

public class BladderButton : MonoBehaviour
{
    GameObject _player;
    bool disableButton = false;
    public float disableTime;
    public PhysicMaterial bounceMaterial;
    private bool usedBubbleGum;
    private Quaternion rot;
    public int timesToUse = 0;
    private ShopItem item;
	public Color color;


    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        item = _player.GetComponent<Player>().gameData.ShopItems.FirstOrDefault(x => x.Name == "Bladder");
        timesToUse = item.UpgradesCount;




    }

    void ResetRotation()
    {
        usedBubbleGum = false;
        _player.transform.rotation = rot;
    }
    // Update is called once per frame
    void Update()
    {
		if (item.UpgradesCount == 0) {
			
			var children = gameObject.GetComponentsInChildren<UISprite>();
			Debug.Log ("Children: "+children.Length);
			
			foreach (UISprite child in children) {
				var c = child.GetComponent<UIWidget>();
				c.color = this.color;
				Debug.Log("change color"+c.color);
			}
		}
        if (usedBubbleGum == true)
        {
            _player = GameObject.FindWithTag("Player");
            rot = _player.transform.rotation;
            _player.transform.rotation = Quaternion.Euler(0, 0, 0);
            Invoke("ResetRotation", disableTime);
        }
    }
    void OnClick()
    {
        if (!disableButton)
        {
            _player = GameObject.FindWithTag("Player");
            if (timesToUse != 0)
            {
                Animator _animator = _player.GetComponent<Animator>();
                if (_animator.GetBool("Fly") == true)
                {
                    timesToUse--;
                    disableButton = true;
                    Invoke("EnableButton", disableTime);
                    _player.collider.material = bounceMaterial;
                    _animator.SetBool("Fly", false);
                    _animator.SetTrigger("BubbleGum");
                    Debug.Log("Bubble Gum");
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
