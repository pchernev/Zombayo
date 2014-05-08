using UnityEngine;

public class ShopMenu : MonoBehaviour
{
  public const int CoinValue = 10;

  public UILabel coinCount;

  public UIButton backButton;

  public UIImageButton magnetButton;
  public UIImageButton fartButton;
  public UIImageButton glideButton;
  public UIImageButton carrotSprayButton;
  public UIImageButton bubbleGumButton;

  private UIImageButton[] buttons;

  public UIPanel panel;

  private GameData gameData;
  private UpgradeData upgradeData;
  private ShopItemInfo[] stars;

  void Awake()
  {
    if (backButton.GetComponent<UIEventListener>() == null)
      backButton.gameObject.AddComponent<UIEventListener>();
    if (magnetButton.GetComponent<UIEventListener>() == null)
      magnetButton.gameObject.AddComponent<UIEventListener>();
    if (fartButton.GetComponent<UIEventListener>() == null)
      fartButton.gameObject.AddComponent<UIEventListener>();
    if (glideButton.GetComponent<UIEventListener>() == null)
      glideButton.gameObject.AddComponent<UIEventListener>();
    if (carrotSprayButton.GetComponent<UIEventListener>() == null)
      carrotSprayButton.gameObject.AddComponent<UIEventListener>();
    if (bubbleGumButton.GetComponent<UIEventListener>() == null)
      bubbleGumButton.gameObject.AddComponent<UIEventListener>();

    buttons = new UIImageButton[(int)UpgradeLevel.Type.Count];
    buttons[(int)UpgradeLevel.Type.Magnet] = magnetButton;
    buttons[(int)UpgradeLevel.Type.CarrotSpray] = carrotSprayButton;
    buttons[(int)UpgradeLevel.Type.BubbleGum] = bubbleGumButton;
    buttons[(int)UpgradeLevel.Type.PowerUpFart] = fartButton;
    buttons[(int)UpgradeLevel.Type.PowerUpGlide] = glideButton;
  }

  void Start()
  {
    gameData = GameLogic.Instance.gameData;
    upgradeData = GameLogic.Instance.upgradeData;

    connectShopItemInfos();
  }

  void Update()
  {
    setCounter(coinCount, gameData.coinCount * CoinValue);
    for (int upgIdx = 0; upgIdx < (int)UpgradeLevel.Type.Count; ++upgIdx) {
      int level = gameData.levels[upgIdx];
      bool isFull = level == UpgradeLevel.NumLevels - 1;

      stars[upgIdx].level = level;
      stars[upgIdx].priceNextLevel = isFull ? 0 : upgradeData.upgradeLevels[level+1].prices[upgIdx];
      buttons[upgIdx].isEnabled = (!isFull && stars[upgIdx].priceNextLevel <= gameData.coinCount * CoinValue);
    }
  }

  private void setCounter(UILabel counter, int value)
  {
    counter.text = value > 0 ? value.ToString() : "";
  }

  private void connectShopItemInfos()
  {
    bool switchOff = false;
    if (!panel.gameObject.activeSelf) {
      panel.gameObject.SetActive(true);
      switchOff = true;
    }

    ShopItemInfo[] foundStars = panel.GetComponentsInChildren<ShopItemInfo>();
    if (switchOff)
      panel.gameObject.SetActive(false);

    stars = new ShopItemInfo[(int)UpgradeLevel.Type.Count];
    foreach (var s in foundStars) {
      if (stars[(int)s.type] != null) {
        Debug.LogError("More than one ShopItemInfo component of type " + s.type.ToString() + " found. Please fix!");
        break;
      }

      stars[(int)s.type] = s;
    }
  }
}
