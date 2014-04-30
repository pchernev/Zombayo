using UnityEngine;
using System.Collections;

public class PlayVideo : MonoBehaviour
{
  public MovieTexture movieTexture;

  void Start() {
    if (movieTexture != null)
      movieTexture.Play();
	}
}
