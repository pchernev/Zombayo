using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
  public static GameLogic Instance;

  public GameData gameData;
  public UpgradeData upgradeData;
  public UIPanel inGameGUIPanel;
  public Animation endGameAnim;
  public CameraFollow cameraFollow;

  public Player player;
  public bool IsPlayerActive { get { return player.IsKicked && !gameOver; } }
  public bool IsSwiping { get { return isSwiping; } }

  public DrFishhead doctor;
  public KickSwipe swipe;

  [HideInInspector]
  public int coinsOnStart;
  private bool isSwiping;
  private bool gameOver;
  private float kickTime;

  void Awake()
  {
    Instance = this;
  }
    
	void Start()
	{
    inGameGUIPanel.gameObject.SetActive(false);
    coinsOnStart = gameData.coinCount;

    startGame();
  }
	
	void Update()
  {
    if (!gameOver) {
      if (player.IsKicked) {
        updatePlayerState();
        checkForLowSpeedGameOver();
      }
      else if (!isSwiping)
        gameData.kickEfficiency = player.CurrentKickEfficiency;
    }
  }


  public void startGame()
  {
    doctor.prepareForStart();
    player.prepareForStart();
    cameraFollow.reset();
    gameData.setFromLevels();
    gameData.coinCount = coinsOnStart;
    isSwiping = gameOver = false;

    inGameGUIPanel.gameObject.SetActive(true);
    endGameAnim.gameObject.SetActive(false);
    endGameAnim["igm_show"].time = 0;

    swipe.gameObject.SetActive(true);

    Time.timeScale = 1;
  }

  public void pauseGame()
  {
    Time.timeScale = 0;
    isSwiping = false;
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

    inGameGUIPanel.gameObject.SetActive(false);
    endGameAnim.gameObject.SetActive(true);
  }

  public void delayedEndGame(float delay)
  {
    StartCoroutine(EndGameRoutine(delay));
  }


  public void onSwipeStart()
  {
    isSwiping = true;
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
