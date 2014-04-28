﻿using UnityEngine;

public class InGameGUI : MonoBehaviour
{
  public UISlider fartSlider;
  public UISlider glideSlider;
  public UISlider kickForceSlider;

  public UILabel bubbleGumCounter;
  public UILabel carrotSprayCounter;
  public UILabel coinCounter;

  public UILabel fartDisabledLabel;
  public UILabel glideDisabledLabel;

  public UISprite carrotSpraySprite;
  public UISprite bubbleGumSprite;

  public UIButton fartButton;
  public UIButton glideButton;
  public UIButton bubbleButton;
  public UIButton pauseButton;

  private GameData data;

  void Start()
  {
    data = GameLogic.Instance.gameData;

    if (fartButton.GetComponent<UIEventListener>() == null)
      fartButton.gameObject.AddComponent<UIEventListener>();
    if (glideButton.GetComponent<UIEventListener>() == null)
      glideButton.gameObject.AddComponent<UIEventListener>();
    if (bubbleButton.GetComponent<UIEventListener>() == null)
      bubbleButton.gameObject.AddComponent<UIEventListener>();
    if (pauseButton.GetComponent<UIEventListener>() == null)
      pauseButton.gameObject.AddComponent<UIEventListener>();

    fartDisabledLabel.gameObject.SetActive(data.specs.fartCapacity <= 0);
    glideDisabledLabel.gameObject.SetActive(data.specs.glideCapacity <= 0);
  }

  void Update()
  {
    fartSlider.value = data.FartPercentage;
    glideSlider.value = data.GlidePercentage;
    kickForceSlider.value = data.kickEfficiency;
    kickForceSlider.gameObject.SetActive(!GameLogic.Instance.IsSwiping);

    setCounter(coinCounter, data.coinCount);
    setCounter(carrotSprayCounter, data.specs.carrotSprayCount);
    setCounter(bubbleGumCounter, data.specs.bubbleGumCount);

    setSpriteColor(carrotSpraySprite, data.specs.carrotSprayCount > 0);
    setSpriteColor(bubbleGumSprite, data.specs.bubbleGumCount > 0);

    bool buttonsActive = GameLogic.Instance.IsPlayerActive;
    fartButton.isEnabled = buttonsActive && data.fartTime > 0;
    glideButton.isEnabled = buttonsActive && data.glideTime > 0;
    bubbleButton.isEnabled = buttonsActive && data.specs.bubbleGumCount > 0;
  }

  private void setCounter(UILabel counter, int value)
  {
    counter.text = value > 0 ? value.ToString() : "";
  }

  private void setSpriteColor(UISprite sprite, bool isActive)
  {
    sprite.color = isActive ? Color.white : Color.gray;
  }
}
