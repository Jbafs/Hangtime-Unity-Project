using System.Collections;
using TMPro;
using UnityEngine;

public class WinManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro winStats;

	[SerializeField]
	private TextMeshPro timerStat;

	[SerializeField]
	private TextMeshPro abilityStat;

	[SerializeField]
	private TypeWriterEffect dialogue;

	[SerializeField]
	private AudioSource music;

	[SerializeField]
	private AudioSource applause;

	private float runTime;

	private void Start()
	{
		runTime = StatTracker.runTimer;
		GameManager.Instance.inputManager.UpdateInputMaps(menuInputActive: true, "win");
		winStats.text = "Final score: " + StatTracker.playerScore + " - " + StatTracker.opponentScore;
		timerStat.text = "Time: " + GetFormattedTime(runTime);
		string text = "Abilities: ";
		foreach (Technique allTechnique in GameManager.Instance.playerStats.GetAllTechniques())
		{
			text = text + allTechnique.name + ", ";
		}
		if (text.Length > 2)
		{
			text = text.Substring(0, text.Length - 2);
		}
		abilityStat.text = text;
		if (GameManager.Instance.demo)
		{
			StartCoroutine(dialogue.TypeText("The game's just getting started!\nDon't forget to WISHLIST!", setter: false, 6f));
		}
		else
		{
			StartCoroutine(dialogue.TypeText("We... Actually did it?!\n", setter: false, 6f));
		}
		StartCoroutine(DelayMusic());
	}

	private IEnumerator DelayMusic()
	{
		GameManager.SetMusic(applause.clip, 0f);
		yield return new WaitForSeconds(14.3f);
		GameManager.SetMusic(music.clip, music.volume);
	}

	public string GetFormattedTime(float time)
	{
		if (time >= 3600f)
		{
			return "Over an hour...";
		}
		int num = Mathf.FloorToInt(time / 60f);
		int num2 = Mathf.FloorToInt(time % 60f);
		int num3 = Mathf.FloorToInt(time * 100f % 100f);
		return $"{num:00}:{num2:00}.{num3:00}";
	}
}
