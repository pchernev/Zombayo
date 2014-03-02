using UnityEngine;
using System.Collections;

public class ShopPanelScript : MonoBehaviour {

    private GameObject player;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
    }

	
	// Update is called once per frame
	void Update () {
	
	}

    public void HideShopPanel() 
    {
        player.GetComponent<GameManager>().CloseShop();
    }
}
