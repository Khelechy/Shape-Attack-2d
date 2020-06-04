using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { set; get; }
	PlayerController player;
	public GameObject theManager;
	GameObject[] obstacleParent;
	public int gameInfectedCells = 3;
	int gameCollectedVaccines = 0;
	int finalCoin;
	public static bool isGameStarted = false;
	public GameObject Brain;

	//UI COMPONENTS
	public GameObject gameOverPanel;
	public GameObject pauseMenu;
	public GameObject startMenu;
	public GameObject freeGiftWrapper;
	public GameObject buyVaccineWrapper;
	public TextMeshProUGUI currentScoreText;
	public TextMeshProUGUI bestScoreText;
	public TextMeshProUGUI infectedScoreText;
	public TextMeshProUGUI coinText;
	public TextMeshProUGUI freeGiftText;
	public TextMeshProUGUI buyVaccineText;
	public TextMeshProUGUI gameStatusText;

	//Animator
	public Animator startMenuAnim;

	//Game Sounds
	public AudioClip warning1;
	public AudioClip warning2;
	public AudioClip warning3;
	public AudioClip coinSound;
	public AudioClip GameOver_Sound;

	public AudioSource bgSound;

	AudioSource sounds;

	public int vaccineAmount = 25000;
	int gameStatus;

	private void Awake()
	{

		Instance = this;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		sounds = GetComponent<AudioSource>();
		coinText.text = "Coin: " + PlayerPrefs.GetInt("Coins").ToString();
		gameStatus = PlayerPrefs.GetInt("Finished");
		bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
		if (SceneReference.isRestarted)
		{
			startMenu.SetActive(false);
			isGameStarted = true;
			SceneReference.isRestarted = false;
		}

		if(gameStatus >= 1)
		{
			gameStatusText.text = "You Have Finished this Game, Hurray!";
		}
		else
		{
			gameStatusText.text = "Gather coins to buy the sheild!";
		}

	}

	private void Update()
	{
		obstacleParent = GameObject.FindGameObjectsWithTag("ObstacleParent");
		finalCoin = PlayerPrefs.GetInt("Coins");
		gameStatus = PlayerPrefs.GetInt("Finished");
		coinText.text = "Coin: " + PlayerPrefs.GetInt("Coins").ToString();
		currentScoreText.text = gameCollectedVaccines.ToString();
		infectedScoreText.text = gameInfectedCells.ToString();

		if(Input.touchCount <= 0)
		{
			return;
		}
		foreach(var touch in Input.touches)
		{
			if(touch.tapCount == 2)
			{
				Debug.Log("Easter Egg");
			}
		}

		if (gameStatus >= 1)
		{
			gameStatusText.text = "You Have Finished this Game, Hurray!";
		}
		else
		{
			gameStatusText.text = "Gather coins to buy the shield!";
		}

	}

	public void IncrementInfectedCells()
	{
		gameInfectedCells -= 1;
	}
	public void IncrementCollectedVaccines()
	{
		gameCollectedVaccines += 1;
		
		if(gameCollectedVaccines > PlayerPrefs.GetInt("BestScore"))
		{
			PlayerPrefs.SetInt("BestScore", gameCollectedVaccines);
			bestScoreText.text = gameCollectedVaccines.ToString();
		};
	}

	public void StartGame()
	{
		isGameStarted = true;
		startMenuAnim.SetTrigger("Disactivate");
	}

	public void Death()
	{
		player.isDead = true;
		gameInfectedCells = 0;
		SetObstacleToDead();
		StartCoroutine(GameOver());
	}

	public void GameContinueEvent()
	{
		sounds.Stop();
		StartCoroutine(Continue());
	}

	IEnumerator GameOver()
	{
		bgSound.Pause();
		sounds.PlayOneShot(GameOver_Sound);
		finalCoin += gameCollectedVaccines;
		PlayerPrefs.SetInt("Coins", finalCoin);
		yield return new WaitForSeconds(0.5f);
		gameOverPanel.SetActive(true);
		yield break;
	}

	public void PauseGame()
	{
		Time.timeScale = 0;
		pauseMenu.SetActive(true);
	}

	public void PlayGame()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = 1;
	}

	public void PlayWarningOne()
	{
		sounds.PlayOneShot(warning1);
	}

	public void PlayWarningTwo()
	{
		sounds.PlayOneShot(warning2);
	}
	public void PlayWarningThree()
	{
		sounds.PlayOneShot(warning3);
	}

	public void PlayCoin()
	{
		sounds.PlayOneShot(coinSound);
	}

	IEnumerator Continue()
	{
		
		bgSound.Play();
		Time.timeScale = 1;
		gameInfectedCells = 3;
		finalCoin -= gameCollectedVaccines;
		PlayerPrefs.SetInt("Coins", finalCoin);
		yield return new WaitForSeconds(0.5f);
		gameOverPanel.SetActive(false);
		player.isDead = false;
		SetObstacleToAlive();
		player.EnableCollider();

	}

	void SetObstacleToDead()
	{
		foreach(var obstacle in obstacleParent)
		{
			obstacle.GetComponent<ObstacleParent>().isDead = true;
		}
	}

	void SetObstacleToAlive()
	{
		foreach (var obstacle in obstacleParent)
		{
			obstacle.GetComponent<ObstacleParent>().isDead = false;
		}
	}

	public void RestartGame()
	{

		Time.timeScale = 1;
		SceneReference.isRestarted = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	
	public void ToStartMenu()
	{
		Time.timeScale = 1;
		isGameStarted = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	

	public void FreeGiftEvent()
	{
		StartCoroutine(FreeGift());
	}

	public void BuyVaccineNoticeEvent()
	{
		if (finalCoin >= vaccineAmount)
		{
			sounds.PlayOneShot(coinSound);
			finalCoin -= vaccineAmount;
			PlayerPrefs.SetInt("Coins", finalCoin);
			FinishedGame();
		}
		else
		{
			StartCoroutine(BuyVaccineNotice());
		}
		
	}

	void FinishedGame()
	{
		gameStatusText.text = "You Have Finished this Game, Hurray!";
		PlayerPrefs.SetInt("Finished", 1);
	}

	IEnumerator FreeGift()
	{
		int giftPoint = (int)Random.Range(10, 25);
		finalCoin += giftPoint;
		PlayerPrefs.SetInt("Coins", finalCoin);
		freeGiftWrapper.SetActive(true);
		sounds.PlayOneShot(coinSound);
		freeGiftText.text = "You Have Been Gifted " + giftPoint + " Coins!";
		yield return new WaitForSeconds(3.0f);
		freeGiftWrapper.SetActive(false);
	}

	IEnumerator BuyVaccineNotice()
	{
		buyVaccineWrapper.SetActive(true);
		buyVaccineText.text = "You Need 25,000 coins!";
		yield return new WaitForSeconds(3.0f);
		buyVaccineWrapper.SetActive(false);

	}



}
