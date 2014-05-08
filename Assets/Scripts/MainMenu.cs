using UnityEngine;

public class MainMenu : MonoBehaviour
{
  public UIImageButton newGameButton;
  public UIImageButton continueButton;

  void Awake()
  {
    if (newGameButton.GetComponent<UIEventListener>() == null)
      newGameButton.gameObject.AddComponent<UIEventListener>();
    if (continueButton.GetComponent<UIEventListener>() == null)
      continueButton.gameObject.AddComponent<UIEventListener>();
  }
}
