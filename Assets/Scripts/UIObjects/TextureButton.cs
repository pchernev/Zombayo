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
	
	private Vector3 startPos;
	private Quaternion startRotation;	
	
	// Use this for initialization
	void Start()
	{
		startPos = transform.position;
		startRotation = transform.rotation;
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
		
//		yield return new WaitForSeconds( 1.0f );
		
		player.transform.position = startPos;
		player.transform.rotation = startRotation;
		player.hasBeenKicked = 0;
		
		doctor.Reset();
		
		guiTexture.enabled = false;
	}
}
