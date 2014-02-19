using UnityEngine;
using System.Collections;

public class ContinueBtnScript : MonoBehaviour {
    GameObject player;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        player.SendMessage("ResumeGame");
    }
}
