using UnityEngine;
using System.Collections.Generic;

public class RevMobUtils : MonoBehaviour
{
  private static readonly Dictionary<string, string> REVMOB_APP_IDS = new Dictionary<string, string>() {
    { "Android", "536b7021e55817df261a8446"}
    //{ "IOS", "copy your iOS RevMob App ID here" }
  };

  public static RevMob revmob;

  void Awake()
  {
    revmob = RevMob.Start(REVMOB_APP_IDS, "RevMob");
    revmob.SetTestingMode(RevMob.Test.WITH_ADS);
  }

  public static RevMobBanner createBanner(UIWidget adPlaceholder)
  {
    Vector3 posBL = UICamera.mainCamera.WorldToScreenPoint(adPlaceholder.worldCorners[0]);
    Vector3 posTL = UICamera.mainCamera.WorldToScreenPoint(adPlaceholder.worldCorners[1]);
    //Vector3 posTR = UICamera.mainCamera.WorldToScreenPoint(adPlaceholder.worldCorners[2]);
    Vector3 posBR = UICamera.mainCamera.WorldToScreenPoint(adPlaceholder.worldCorners[3]);

#if UNITY_ANDROID
    return revmob.CreateBanner(RevMob.Position.TOP, (int)posTL.x, Screen.height - (int)posTL.y, (int)(posBR.x - posBL.x), (int)(posTL.y - posBL.y));
#elif UNITY_IPHONE
    return revmob.CreateBanner(0, 0, 350, 50, null, null);
#endif
  }
}
