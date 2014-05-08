using UnityEngine;

public class EndGameMenu : MonoBehaviour
{
  public UIPanel  panel;
  public UIButton restartButton;
  public UIButton shopButton;

  void Awake()
  {
    if (restartButton.GetComponent<UIEventListener>() == null)
      restartButton.gameObject.AddComponent<UIEventListener>();
    if (shopButton.GetComponent<UIEventListener>() == null)
      shopButton.gameObject.AddComponent<UIEventListener>();
  }
}
