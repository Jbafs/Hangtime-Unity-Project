using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : OnClick
{
	public bool toTitle;

	public bool toTutorial;

	public override void Click()
	{
		GameManager.gameOver = true;
		GameManager.gameNumber = 0;
		foreach (StatUpgrade statUpgrade in GameManager.Instance.playerStats.statUpgrades)
		{
			statUpgrade.currentLevel = 0;
		}
		GameManager.Instance.playerStats.techniques.Clear();
		TutorialManager.onTutorial = false;
		StartCoroutine(LoadGame());
	}

	private IEnumerator LoadGame()
	{
		GameManager.Instance.transition.Play();
		yield return new WaitForSecondsRealtime(0.5f);
		GameManager.Instance.ResetGame();
		PauseController.paused = false;
		if (toTitle)
		{
			TitleController.onTitle = true;
		}
		if (toTutorial)
		{
			TutorialManager.onTutorial = true;
			TutorialManager.tutorialPart = 0;
		}
		SceneManager.LoadScene("Game");
	}
}
