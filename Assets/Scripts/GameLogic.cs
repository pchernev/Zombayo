using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
  public static GameLogic Instance;

  public GameData gameData;
  public UpgradeData upgradeData;
  public UIPanel inGameGUIPanel;
  public UIPanel mainMenuPanel;
  public Animation endGameAnim;
  public CameraFollow cameraFollow;

  public StartPrefabs startPrefabs;

  public GameObject mainChars;
  [HideInInspector]
  public DrFishhead doctor;
  [HideInInspector]
  public Player player;
  public bool IsPlayerActive { get { return player.IsKicked && !gameOver; } }

  public KickSwipe swipe;

  private bool gameOver;
  private float kickTime;

  void Awake()
  {
    Instance = this;

    player = mainChars.GetComponentInChildren<Player>();
    doctor = mainChars.GetComponentInChildren<DrFishhead>();
  }
    
	void Start()
	{
    gameData.isSwiping = false;
    mainChars.SetActive(false);

    inGameGUIPanel.enabled = false;
    mainMenuPanel.enabled = true;
  }
	
	void Update()
  {
    if (!gameOver && player.IsKicked) {
      checkForLevelComplete();
      updatePlayerState();
      checkForLowSpeedGameOver();
    }
  }


  public void showMainMenu()
  {
    mainChars.SetActive(false);
  }

  public void startGame()
  {
    startPrefabs.recycleOld();
    startPrefabs.spawn();
    mainChars.SetActive(true);
    doctor.prepareForStart();
    player.prepareForStart();
    cameraFollow.reset();
    gameData.setFromLevels();
    gameData.coinCount = gameData.coinsOnStart;
    gameOver = false;

    inGameGUIPanel.enabled = true;
    endGameAnim.gameObject.SetActive(false);
    endGameAnim["igm_show"].time = 0;

    swipe.gameObject.SetActive(true);

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
    swipe.gameObject.SetActive(!player.IsKicked);
  }

  public void endGame()
  {
    player.rigidbody.isKinematic = true;
    gameOver = true;

    inGameGUIPanel.enabled = false;
    endGameAnim.gameObject.SetActive(true);

    gameData.coinCount += (int) (0.001f * gameData.travelledDistance * (float)gameData.coinsPerKm);
  }

  public void delayedEndGame(float delay)
  {
    StartCoroutine(EndGameRoutine(delay));
  }

  public void finishLevel()
  {
    endGame();
  }


  public void onSwipeStart()
  {
    gameData.isSwiping = true;
  }

  public void initiateKick()
  {
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
    kickTime = Time.time;

    cameraFollow.doTransition = true;
  }

  public void resetProgress()
  {
    gameData.coinCount = 0;
    for (int i = 0; i < gameData.levels.Length; ++i)
      gameData.levels[i] = 0;
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
    if (kickTime + 0.1f < Time.time && player.IsTooSlow)
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


  private IEnumerator EndGameRoutine(float delay)
  {
    yield return new WaitForSeconds(delay);

    endGame();
  }
}
