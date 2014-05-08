using UnityEngine;
using System.Collections;

public class InputLogic : MonoBehaviour
{
  public InGameGUI inGameGUI;
  public PauseMenu pauseMenu;
  public EndGameMenu endGameMenu;
  public ShopMenu shopMenu;
  public MainMenu mainMenu;

  private GameData gameData;
  private UpgradeData upgradeData;
  private Player player;
  
	void Start ()
	{
    gameData = GameLogic.Instance.gameData;
    upgradeData = GameLogic.Instance.upgradeData;
	  player = GameLogic.Instance.player;

    setInGameGUIDelegates();
    setPauseMenuDelegates();
    setEndGameMenuDelegates();
    setShopMenuDelegates();
    setMainMenuDelegates();
	}
	
	void Update () {
	
	}

  private void setInGameGUIDelegates()
  {
    if (inGameGUI != null) {
      inGameGUI.fartButton.GetComponent<UIEventListener>().onPress = OnFartButtonPressed;
      inGameGUI.glideButton.GetComponent<UIEventListener>().onPress = OnGlideButtonPressed;
      inGameGUI.bubbleButton.GetComponent<UIEventListener>().onClick = OnBubbleButtonClicked;
      inGameGUI.pauseButton.GetComponent<UIEventListener>().onClick = OnPauseButtonClicked;
    }
    else {
      Debug.LogError("InGameGUI variable not set!");
    }
  }

  private void setPauseMenuDelegates()
  {
    if (pauseMenu != null) {
      pauseMenu.restartButton.GetComponent<UIEventListener>().onClick = OnRestartButtonClicked;
      pauseMenu.mainMenuButton.GetComponent<UIEventListener>().onClick = OnMainMenuButtonClicked;
      pauseMenu.backButton.GetComponent<UIEventListener>().onClick = OnBackButtonClicked;
    }
    else {
      Debug.LogError("PauseMenu variable not set!");
    }
  }

  private void setEndGameMenuDelegates()
  {
    if (endGameMenu != null) {
      endGameMenu.restartButton.GetComponent<UIEventListener>().onClick = OnRestartButtonClicked;
    }
    else {
      Debug.LogError("EndGameMenu variable not set!");
    }
  }

  private void setShopMenuDelegates()
  {
    if (shopMenu != null) {
      shopMenu.magnetButton.GetComponent<UIEventListener>().onClick = OnBuyUpgradeButtonClicked;
      shopMenu.carrotSprayButton.GetComponent<UIEventListener>().onClick = OnBuyUpgradeButtonClicked;
      shopMenu.bubbleGumButton.GetComponent<UIEventListener>().onClick = OnBuyUpgradeButtonClicked;
      shopMenu.fartButton.GetComponent<UIEventListener>().onClick = OnBuyUpgradeButtonClicked;
      shopMenu.glideButton.GetComponent<UIEventListener>().onClick = OnBuyUpgradeButtonClicked;
    }
    else {
      Debug.LogError("ShopMenu variable not set!");
    }
  }

  private void setMainMenuDelegates()
  {
    if (mainMenu != null) {
      mainMenu.newGameButton.GetComponent<UIEventListener>().onClick = OnNewGameButtonClicked;
      mainMenu.continueButton.GetComponent<UIEventListener>().onClick = OnContinueButtonClicked;
    }
    else {
      Debug.LogError("EndGameMenu variable not set!");
    }
  }

  public void OnBubbleButtonClicked(GameObject go)
  {
    if (gameData.specs.bubbleGumCount > 0) {
      --gameData.specs.bubbleGumCount;
      player.enterPowerUpState(Player.PowerUpState.BubbleGum);
    }
  }

  public void OnFartButtonPressed(GameObject go, bool pressed)
  {
    if (gameData.fartTime > 0)
      player.enterPowerUpState(pressed ? Player.PowerUpState.Fart : Player.PowerUpState.None);
  }

  public void OnGlideButtonPressed(GameObject go, bool pressed)
  {
    if (gameData.glideTime > 0)
      player.enterPowerUpState(pressed ? Player.PowerUpState.Glide : Player.PowerUpState.None);
  }

  public void OnPauseButtonClicked(GameObject go)
  {
    GameLogic.Instance.pauseGame();
  }

  public void OnRestartButtonClicked(GameObject go)
  {
    GameLogic.Instance.startGame();
  }

  public void OnMainMenuButtonClicked(GameObject go)
  {
  }

  public void OnBackButtonClicked(GameObject go)
  {
    StartCoroutine(ResumeGameRoutine(Time.realtimeSinceStartup, 1));
  }

  public void OnBuyUpgradeButtonClicked(GameObject go)
  {
    ShopItemInfo info = go.GetComponent<ShopItemInfo>();

    int level = ++gameData.levels[(int)info.type];
    gameData.coinCount -= upgradeData.upgradeLevels[level].prices[(int)info.type] / ShopMenu.CoinValue;
    gameData.coinsOnStart = gameData.coinCount;
  }

  public void OnNewGameButtonClicked(GameObject go)
  {
    Debug.Log("New Game!");
    GameLogic.Instance.resetProgress();
    GameLogic.Instance.startGame();
  }

  public void OnContinueButtonClicked(GameObject go)
  {
    Debug.Log("Continue!");
    GameLogic.Instance.resetProgress();
    GameLogic.Instance.startGame();
  }

  private IEnumerator ResumeGameRoutine(float startTime, float delay)
  {
    while (Time.realtimeSinceStartup < startTime + delay)
      yield return null;

    GameLogic.Instance.resumeGame();
  }
}
