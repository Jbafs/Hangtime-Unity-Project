using System.Collections;
using TMPro;
using UnityEngine;

public class LoseController : MonoBehaviour
{
	[SerializeField]
	private GameObject loseSprite;

	[SerializeField]
	private Animation loseFade;

	[SerializeField]
	private TypeWriterEffect loseText;

	[SerializeField]
	private string[] loseDialogue;

	[SerializeField]
	private SoundLibrary soundLibrary;

	[SerializeField]
	private TextMeshPro stats;

	private void OnEnable()
	{
		GameManager.OnPlayerLose += HandleLoss;
	}

	private void OnDisable()
	{
		GameManager.OnPlayerLose -= HandleLoss;
	}

	private void HandleLoss()
	{
		if (loseSprite == null)
		{
			loseFade.Play();
			AudioManager.Instance.PlaySFX(base.gameObject.GetComponent<AudioSource>().clip, 0.5f);
			StartCoroutine(loseText.TypeText(loseDialogue[Random.Range(0, loseDialogue.Length)], setter: false, 3f));
			StartCoroutine(DelaySound());
			string text = GameManager.Instance.opponentTeam.name.Replace("(Clone)", "").Trim();
			stats.text = "Lost to: " + text + "\nFinal Score: " + StatTracker.playerScore + " - " + StatTracker.opponentScore + "\nBracket: " + TitleCard.instance.MatchTitles[GameManager.gameNumber];
		}
		else
		{
			Object.Instantiate(loseSprite, base.transform.position, Quaternion.identity);
			base.gameObject.SetActive(value: false);
		}
	}

	private IEnumerator DelaySound()
	{
		yield return new WaitForSeconds(5f);
		CameraController.Shake(0.3f);
		AudioManager.Instance.PlaySFX(soundLibrary.perfectBump, 0.3f);
	}
}
