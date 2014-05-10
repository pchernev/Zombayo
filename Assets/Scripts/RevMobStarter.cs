using UnityEngine;
using System.Collections.Generic;

public class RevMobStarter : MonoBehaviour
{
  private static readonly Dictionary<string, string> REVMOB_APP_IDS = new Dictionary<string, string>() {
    { "Android", "536b7021e55817df261a8446"}
    //{ "IOS", "copy your iOS RevMob App ID here" }
  };

  private RevMob revmob;

  void Awake()
  {
    revmob = RevMob.Start(REVMOB_APP_IDS, "RevMob");
    revmob.SetTestingMode(RevMob.Test.WITH_ADS);
  }

  void Start()
  {
#if UNITY_ANDROID
    RevMobBanner banner = revmob.CreateBanner(RevMob.Position.TOP, 0, 0, 350, 50);
    banner.Show();
#elif UNITY_IPHONE
    RevMobBanner banner = revmob.CreateBanner(0, 0, 350, 50, null, null);
    banner.Show();
#endif
  }
}
