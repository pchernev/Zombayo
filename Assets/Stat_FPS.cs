using UnityEngine;
using System.Collections;

public class Stat_FPS : MonoBehaviour
{
	private int count = 0;
	UILabel lbl;

	// Use this for initialization
	void Start () {
		lbl = GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
		count++;
		lbl.text = count.ToString();
	}
}
