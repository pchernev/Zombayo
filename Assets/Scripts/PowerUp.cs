using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
  public float fartTime;
  public float glideTime;
  public GameObject explosion;
  public AudioClip collectPowerUp;

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer == Layers.Player)
      CollisionLogic.OnPowerUpHit(this);
  }

  public void explode()
  {
    var explosionInstance = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
    explosionInstance.transform.parent = transform.parent;
    AudioSource.PlayClipAtPoint(collectPowerUp, explosionInstance.transform.position);

    gameObject.SetActive(false);
    this.Recycle();
    Destroy(explosionInstance, 1.0f);
  }
}
