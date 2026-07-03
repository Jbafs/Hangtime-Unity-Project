using System.Collections;
using UnityEngine;

public class TitleController : MonoBehaviour
{
	public static bool onTitle = true;

	public bool buttonClicked;

	public static TitleController Instance;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private Animation startGame;

	[SerializeField]
	private TitleCard titleCard;

	[SerializeField]
	private AudioSource titleMusic;

	[SerializeField]
	private AudioSource drum;

	[SerializeField]
	private ParticleSystem startParticles;

	private void Start()
	{
		StartCoroutine(delayedStart());
	}

	private IEnumerator delayedStart()
	{
		yield return null;
		if (onTitle)
		{
			GameManager.Instance.numberOfPlayers = 1;
			GameManager.SetMusic(titleMusic.clip, titleMusic.volume);
			yield return new WaitForSeconds(3f);
		}
		else
		{
			StartCoroutine(StartGame());
		}
	}

	public IEnumerator StartGame()
	{
		if (onTitle)
		{
			startGame.Play();
			AudioManager.Instance.PlaySFX(drum.clip, 0.47f);
			startParticles.Play();
		}
		else
		{
			title.SetActive(value: false);
			yield return new WaitForSeconds(1f);
			titleCard.MatchTitle();
		}
	}
}
