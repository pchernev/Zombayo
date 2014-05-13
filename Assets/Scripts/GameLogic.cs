using UnityEngine;
using System;
using System.Collections;

public class GameLogic : MonoBehaviour
{
  public static GameLogic Instance;

  public GameData gameData;
  public UpgradeData upgradeData;
  public UIPanel inGameGUIPanel;
  public Animation mainMenuAnim;
  public Animation endGameAnim;
  public CameraFollow cameraFollow;

  public SpawnTrigger spawnOnStart;
  public Transform spawnRoot;

  public GameObject mainChars;
  [HideInInspector]
  public DrFishhead doctor;
  [HideInInspector]
  public Player player;
  public bool IsPlayerActive { get { return gameData.IsPlayerKicked && !gameOver; } }

  public KickSwipe swipe;

  private bool gameOver;

  void Awake()
  {
    Instance = this;

    player = mainChars.GetComponentInChildren<Player>();
    doctor = mainChars.GetComponentInChildren<DrFishhead>();

    if (gameData.ZeroProgress || !loadGame())
      resetProgress();
  }
    
	void Start()
	{
    showStartScene();

    inGameGUIPanel.enabled = false;
    mainMenuAnim.gameObject.SetActive(true);
  }
	
	void Update()
  {
    if (!gameOver && gameData.IsPlayerKicked) {
      checkForLevelComplete();
      updatePlayerState();
      checkForLowSpeedGameOver();
    }
  }


  public void showStartScene()
  {
    player.moveToStartSpot();
    mainChars.SetActive(false);
    recycleSpawned();
    spawnStartObjects();
    cameraFollow.reset();
  }

  public void startGame()
  {
    showStartScene();
    loadGame();
    mainChars.SetActive(true);
    doctor.prepareForStart();
    player.prepareForStart();
    gameData.setFromLevels();
    gameOver = false;

    inGameGUIPanel.enabled = true;
    mainMenuAnim.gameObject.SetActive(false);
    mainMenuAnim["igm_show"].time = 0;
    endGameAnim.gameObject.SetActive(false);
    endGameAnim["igm_show"].time = 0;

    swipe.gameObject.SetActive(true);

    gameData.kickTime = -1;
    Time.timeScale = 1;
  }

  public void pauseGame()
  {
    Time.timeScale = 0;
    swipe.gameObject.SetActive(false);
  }

  public void resumeGame()
  {
    Time.timeScale = 1;
    swipe.gameObject.SetActive(!gameData.IsPlayerKicked);
  }

  public void endGame()
  {
    player.rigidbody.isKinematic = true;
    gameOver = true;

    inGameGUIPanel.enabled = false;
    endGameAnim.gameObject.SetActive(true);

    gameData.coinCount += (int) (0.001f * gameData.travelledDistance * (float)gameData.coinsPerKm);
    saveGame();
  }

  public void delayedEndGame(float delay)
  {
    StartCoroutine(EndGameRoutine(delay));
  }

  public void goToMainMenu()
  {
    inGameGUIPanel.enabled = false;
    mainMenuAnim.gameObject.SetActive(true);
    showStartScene();
  }

  public void finishLevel()
  {
    endGame();
  }

  public void saveGame()
  {
    string levels = "";
    for (int i = 0; i < (int)UpgradeLevel.Type.Count; ++i)
      levels += (char)gameData.levels[i];

    PlayerPrefs.SetString("levels", levels);
    PlayerPrefs.SetInt("coins", gameData.coinCount);
  }

  public bool loadGame()
  {
    string levels = PlayerPrefs.GetString("levels");
    if (levels.Length == (int)UpgradeLevel.Type.Count)
      for (int i = 0; i < (int)UpgradeLevel.Type.Count; ++i)
        gameData.levels[i] = (int)levels[i];
    else
      return false;

    gameData.coinCount = PlayerPrefs.GetInt("coins");

    return true;
  }


  public void initiateKick()
  {
    player.UpdateKickEfficiency = false;
    doctor.playRunAndKickSequence();
  }

  public void initiateKickInCustomDirection(Vector3 direction)
  {
    gameData.kickDirection = direction;
    initiateKick();
  }

  public void launchRabbit()
  {
    doctor.rigidbody.isKinematic = true;
    player.kickRabbit(gameData.KickForce);
    gameData.kickTime = Time.time;

    cameraFollow.doTransition = true;
  }

  public void resetProgress()
  {
    gameData.coinCount = 0;
    for (int i = 0; i < gameData.levels.Length; ++i)
      gameData.levels[i] = 0;
    
    saveGame();
  }


  private void updatePlayerState()
  {
    if (player.IsFarting) {
      gameData.fartTime -= Time.deltaTime;
      checkIfFartUsedUp();
    }
    else if (player.IsGliding) {
      gameData.glideTime -= Time.deltaTime;
      checkIfGlideUsedUp();
    }
  }

  private void checkForLowSpeedGameOver()
  {
    if (gameData.kickTime + 0.1f < Time.time && player.IsTooSlow)
      endGame();
  }

  private void checkForLevelComplete()
  {
    if (gameData.travelledDistance >= gameData.distanceToFinish)
      finishLevel();
  }

  private void checkIfFartUsedUp()
  {
    if (player.IsFarting && gameData.fartTime <= 0) {
      gameData.fartTime = 0;
      player.enterPowerUpState(Player.PowerUpState.None);
    }
  }

  private void checkIfGlideUsedUp()
  {
    if (player.IsGliding && gameData.glideTime <= 0) {
      gameData.glideTime = 0;
      player.enterPowerUpState(Player.PowerUpState.None);
    }
  }

  private void spawnStartObjects()
  {
    spawnOnStart.trigger();
  }

  private void recycleSpawned()
  {
    var spawnedObjects = spawnRoot.GetComponentsInChildren<Transform>();
    foreach (var obj in spawnedObjects)
      if (obj.parent == spawnRoot)
        obj.Recycle();
  }


  private IEnumerator EndGameRoutine(float delay)
  {
    yield return new WaitForSeconds(delay);

    endGame();
  }
}
