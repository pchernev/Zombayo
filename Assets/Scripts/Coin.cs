using UnityEngine;

public class Coin : MonoBehaviour
{
  public GameObject coinExplosion;
  public AudioClip collectCoins;

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer == Layers.Player)
      CollisionLogic.OnCoinCollected(this);
  }

  public void explode()
  {
    var explosionInstance = Instantiate(coinExplosion, transform.position, transform.rotation) as GameObject;
    explosionInstance.transform.parent = transform.parent;
    AudioSource.PlayClipAtPoint(collectCoins, explosionInstance.transform.position);

    gameObject.SetActive(false);
    this.Recycle();
    Destroy(explosionInstance, 1.0f);
  }
}
