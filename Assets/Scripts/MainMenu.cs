using UnityEngine;

public class MainMenu : MonoBehaviour
{
  public UIImageButton newGameButton;
  public UIImageButton continueButton;
  public UIWidget adPlaceholder;
  public UICamera uiCamera;

  void Awake()
  {
    if (newGameButton.GetComponent<UIEventListener>() == null)
      newGameButton.gameObject.AddComponent<UIEventListener>();
    if (continueButton.GetComponent<UIEventListener>() == null)
      continueButton.gameObject.AddComponent<UIEventListener>();
  }

  void Start()
  {
    RevMobBanner banner = RevMobUtils.createBanner(adPlaceholder);
    banner.Show();
  }

  void Update()
  {
    continueButton.gameObject.SetActive(!GameLogic.Instance.gameData.ZeroProgress);
  }
}
