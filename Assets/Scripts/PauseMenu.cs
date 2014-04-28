using UnityEngine;

public class PauseMenu : MonoBehaviour
{
  public UIImageButton restartButton;
  public UIImageButton mainMenuButton;
  public UIImageButton backButton;

  void Start()
  {
    if (restartButton.GetComponent<UIEventListener>() == null)
      restartButton.gameObject.AddComponent<UIEventListener>();
    if (mainMenuButton.GetComponent<UIEventListener>() == null)
      mainMenuButton.gameObject.AddComponent<UIEventListener>();
    if (backButton.GetComponent<UIEventListener>() == null)
      backButton.gameObject.AddComponent<UIEventListener>();
  }
}
