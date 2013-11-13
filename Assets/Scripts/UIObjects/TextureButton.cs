using UnityEngine;
using System.Collections;

[RequireComponent (typeof( AudioSource ))]
public class TextureButton : MonoBehaviour
{
	public Texture2D normalTexture;
	public Texture2D rollOverTexture;
	public AudioClip clickSound;
	
	public Player player;
	public DrController doctor;
	

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseEnter()
	{
		guiTexture.texture = rollOverTexture;
	}
	
	void OnMouseExit()
	{
		guiTexture.texture = normalTexture;
	}
	
	void OnMouseUp()
	{
		audio.PlayOneShot( clickSound );
		
		player.Reset();
		doctor.Reset();

		guiTexture.enabled = false;
	}
}
