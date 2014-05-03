using UnityEngine;
using System;
using System.Collections.Generic;

using Object = UnityEngine.Object;

public static class CollisionLogic
{
  private static GameData data { get { return GameLogic.Instance.gameData; } }
  private static Player player { get { return GameLogic.Instance.player; } }

  private delegate void CustomLogicDelegate(NPC npc);
  private static readonly Dictionary<Type, CustomLogicDelegate> typeToDelegate;

  static CollisionLogic()
  {
    typeToDelegate = new Dictionary<Type, CustomLogicDelegate>();
    typeToDelegate.Add(typeof(Carrot), OnCarrotHit);
  }


  // Base Logic

  public static void OnCoinAttracted(GameObject magnet, GameObject coin)
  {
    coin.AddComponent<Rigidbody>();
    coin.transform.parent = magnet.transform;
    coin.layer = Layers.AttractedCoins;
    iTween.MoveTo(coin, iTween.Hash("position", magnet.transform.localPosition, "islocal", true, "time", 1));
  }

  public static void OnCoinCollected(Coin coin)
  {
    ++data.coinCount;
    coin.explode();
  }

  public static void OnGroundHit()
  {
    if (!player.IsBubbling) {
      if (GameLogic.Instance.player.IsAboveDragThreshold)
        player.enterHurtState();
      else
        player.enterDragState();
    }
  }

  public static void OnNpcHit(NPC npc)
  {
    player.enterPowerUpState(Player.PowerUpState.None);
    player.rigidbody.AddForce(npc.impactInfo.force);

    if (npc.impactInfo.particles != null) {
      var particlesInstance = Object.Instantiate(npc.impactInfo.particles, npc.transform.position, npc.transform.rotation) as GameObject;
      particlesInstance.transform.parent = npc.transform.parent;
      Object.Destroy(particlesInstance, 1.0f);
    }

    if (npc.impactInfo.sound != null)
      AudioSource.PlayClipAtPoint(npc.impactInfo.sound, npc.transform.position);

    if (npc.impactInfo.animationName.Length > 0)
      npc.animUtils.animator.Play(npc.impactInfo.animationName);

    CustomLogicDelegate logic;
    if (typeToDelegate.TryGetValue(npc.GetType(), out logic))
      logic(npc);
  }


  // Custom Logic

  private static void OnCarrotHit(NPC npc)
  {
    Carrot carrot = (Carrot)npc;

    if (player.IsBubbling) {
      player.enterPowerUpState(Player.PowerUpState.None);
      carrot.miss();
    }
    else if (data.specs.carrotSprayCount > 0) {
      --data.specs.carrotSprayCount;
      player.rigidbody.velocity *= 0.85f;
      carrot.miss();
    }
    else {
      player.rigidbody.isKinematic = true;
      carrot.hit();

      GameLogic.Instance.delayedEndGame(0.25f);
    }
  }
}
