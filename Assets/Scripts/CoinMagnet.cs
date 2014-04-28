using UnityEngine;
using System.Collections;

public class CoinMagnet : MonoBehaviour
{
  void Start()
  {
    SphereCollider sphcol = collider as SphereCollider;
    sphcol.radius = GameLogic.Instance.gameData.specs.magnetRadius;
  }

  void OnTriggerEnter(Collider other)
  {
    CollisionLogic.OnCoinAttracted(gameObject, other.gameObject);
  }
}
