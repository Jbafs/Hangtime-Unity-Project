using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public SaveData saveData;

	[SerializeField]
	private bool skipTutorial;

	public bool recordingMode;

	public static GameManager Instance;

	[Header("Demo")]
	public bool demo;

	public GameObject team1;

	public GameObject team2;

	[Header("Input")]
	public GeneralControllerInputReader ControllerInput;

	public InputManager inputManager;

	[Header("Multiplayer")]
	public int numberOfPlayers;

	[Header("In game")]
	public int matchLength;

	private int originalMatchLength;

	public int playerPoints;

	public int opponentPoints;

	[SerializeField]
	private int startLevel;

	public GameObject ball;

	[SerializeField]
	private AudioSource heartBeat;

	public FanController fanController;

	public static bool gameOver;

	[Header("Stats")]
	public PlayerStats playerStats;

	public static bool statOverload;

	public static int gameNumber;

	public bool done;

	[Header("Opponent Team")]
	private int lastTeam;

	public GameObject practiceTeam;

	public GameObject midBoss1;

	public GameObject midBoss2;

	public GameObject finalBoss;

	public List<GameObject> teams;

	public List<GameObject> comboTeams;

	public OpponentTeam opponentTeam;

	public GameObject currentTeam;

	public TournamentLineUps tournamentLineUps;

	[Header("Effects")]
	public Animation transition;

	public ParticleManager particles;

	public Animation fadeToWhite;

	public AudioSource music;

	[SerializeField]
	private ParticleSystem matchPointEffects;

	private int comebackCounter;

	public static event Action OnPlayerLose;

	private void Awake()
	{
		if (skipTutorial)
		{
			TutorialManager.onTutorial = false;
		}
		UnityEngine.Random.InitState(DateTime.Now.Millisecond + UnityEngine.Random.Range(0, 9999));
		if (Instance == null)
		{
			Instance = this;
			gameNumber = startLevel;
			originalMatchLength = matchLength;
			music = base.gameObject.GetComponent<AudioSource>();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			saveData = SaveSystem.Load();
			if (saveData.tutorialCompleted)
			{
				TutorialManager.onTutorial = false;
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public IEnumerator LoadGame()
	{
		yield return new WaitForSeconds(1.5f);
		Instance.transition.Play();
		yield return new WaitForSeconds(0.5f);
		TitleController.onTitle = false;
		SceneManager.LoadScene("Game");
	}

	public static void SetMusic(AudioClip clip, float volume)
	{
		Instance.music.clip = clip;
		Instance.music.volume = volume;
		Instance.music.Play();
	}

	public void OnStart(GameObject ballObject)
	{
		comebackCounter = 0;
		gameOver = false;
		ball = ballObject;
		currentTeam = null;
		int num = 0;
		if (TitleController.onTitle)
		{
			Instance.inputManager.UpdateInputMaps(menuInputActive: true, "Title");
			Debug.Log("on title!!");
			return;
		}
		Instance.inputManager.UpdateInputMaps(menuInputActive: false, "game");
		if (gameNumber == 0)
		{
			saveData.tutorialCompleted = true;
			tournamentLineUps.NewLineUp();
			LimitBreakEffects.ResetLimits();
			statOverload = false;
			matchLength = 4;
			currentTeam = UnityEngine.Object.Instantiate(practiceTeam, new Vector3(0f, 0f, 0f), Quaternion.identity);
			StatTracker.runTimer = 0f;
		}
		else if (gameNumber == 3)
		{
			currentTeam = UnityEngine.Object.Instantiate(midBoss1, new Vector3(0f, 0f, 0f), Quaternion.identity);
			num = 2;
		}
		else if (gameNumber == 8)
		{
			currentTeam = UnityEngine.Object.Instantiate(finalBoss, new Vector3(0f, 0f, 0f), Quaternion.identity);
			num = -5;
		}
		else if (demo)
		{
			if (gameNumber == 1)
			{
				currentTeam = UnityEngine.Object.Instantiate(team1, new Vector3(0f, 0f, 0f), Quaternion.identity);
			}
			else if (gameNumber == 2)
			{
				currentTeam = UnityEngine.Object.Instantiate(team2, new Vector3(0f, 0f, 0f), Quaternion.identity);
			}
		}
		else if (gameNumber == 6)
		{
			currentTeam = UnityEngine.Object.Instantiate(midBoss2, new Vector3(0f, 0f, 0f), Quaternion.identity);
		}
		else if (gameNumber == 7)
		{
			currentTeam = UnityEngine.Object.Instantiate(comboTeams[UnityEngine.Random.Range(0, comboTeams.Capacity)], new Vector3(0f, 0f, 0f), Quaternion.identity);
		}
		else
		{
			currentTeam = UnityEngine.Object.Instantiate(tournamentLineUps.GetTeam(gameNumber), new Vector3(0f, 0f, 0f), Quaternion.identity);
		}
		StatTracker.ClearAllStats();
		opponentTeam = currentTeam.GetComponent<OpponentTeam>();
		opponentTeam.Init(gameNumber * 3 + 1 + num);
	}

	private void Update()
	{
		if (!done)
		{
			if (playerPoints >= matchLength)
			{
				if (gameNumber == 8 || (demo && gameNumber == 3))
				{
					StartCoroutine(EndGame("Win"));
				}
				else
				{
					StartCoroutine(EndGame("Upgrade"));
				}
			}
			if (opponentPoints >= matchLength)
			{
				saveData.trophies = 0;
				StartCoroutine(EndGame("Lose"));
			}
		}
		if (recordingMode && music != null)
		{
			statOverload = true;
		}
		if (music != null)
		{
			Instance.music.volume = saveData.musicVolume;
		}
		Debug.Log(StatTracker.runTimer);
	}

	private IEnumerator EndGame(string nextScene)
	{
		if (done)
		{
			yield break;
		}
		matchPointEffects.Stop();
		done = true;
		if (nextScene == "Lose")
		{
			if (gameNumber == 0 && !TutorialManager.onTutorial)
			{
				AchievementManager.I.Unlock(AchievementManager.Id.LoseToPractice);
			}
			if (opponentTeam.teamName == "Kozuki")
			{
				AchievementManager.I.Unlock(AchievementManager.Id.LoseToKozuki);
			}
			yield return new WaitForSeconds(1f);
			GameManager.OnPlayerLose();
			gameOver = true;
			music.Stop();
			yield break;
		}
		if (comebackCounter >= 3)
		{
			AchievementManager.I.Unlock(AchievementManager.Id.Comeback);
		}
		if (gameNumber == 3)
		{
			AchievementManager.I.Unlock(AchievementManager.Id.BeatTenzio);
		}
		if (gameNumber == 8)
		{
			AchievementManager.I.Unlock(AchievementManager.Id.WinGame);
		}
		if (nextScene == "Win")
		{
			saveData.trophies++;
			if (saveData.trophies >= 3)
			{
				AchievementManager.I.Unlock(AchievementManager.Id.ThreeWinStreak);
			}
			if (numberOfPlayers == 2)
			{
				AchievementManager.I.Unlock(AchievementManager.Id.TWoPlayer);
			}
			yield return new WaitForSeconds(2f);
			fadeToWhite.Play();
			yield return new WaitForSeconds(4f);
		}
		else
		{
			if (StatTracker.spikes == 0)
			{
				AchievementManager.I.Unlock(AchievementManager.Id.WinNoSpikes);
			}
			yield return new WaitForSeconds(3f);
			transition.Play();
			yield return new WaitForSeconds(0.5f);
		}
		ResetGame();
		SceneManager.LoadScene(nextScene);
	}

	private bool TutorialPoint()
	{
		if (TutorialManager.tutorialPart == 4)
		{
			TutorialManager.pointCounter++;
			return true;
		}
		TutorialManager.pointCounter++;
		return false;
	}

	public bool PlayerGotPoint()
	{
		if (TutorialManager.onTutorial)
		{
			return TutorialPoint();
		}
		if (opponentPoints == 7)
		{
			comebackCounter++;
		}
		playerPoints++;
		StatTracker.playerScore = playerPoints;
		TitleSlide(playerPoints);
		return playerPoints == matchLength;
	}

	public bool OpponentGotPoint()
	{
		if (TutorialManager.onTutorial)
		{
			return TutorialPoint();
		}
		opponentPoints++;
		StatTracker.opponentScore = opponentPoints;
		TitleSlide(opponentPoints);
		return opponentPoints == matchLength;
	}

	private void TitleSlide(int points)
	{
		if (points == matchLength - 1)
		{
			TitleCard.instance.TextSlide("Match Point!");
			heartBeat.volume /= saveData.sfxVolume;
			matchPointEffects.Play();
			if (StatTracker.opponentScore == matchLength - 1 && !heartBeat.isPlaying)
			{
				heartBeat.Play();
			}
		}
		else
		{
			_ = matchLength;
		}
	}

	public void ResetGame()
	{
		done = false;
		heartBeat.Stop();
		Time.timeScale = 1f;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
		playerPoints = 0;
		opponentPoints = 0;
		matchLength = originalMatchLength;
	}
}
