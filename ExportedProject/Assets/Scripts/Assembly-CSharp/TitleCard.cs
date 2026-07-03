using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCard : MonoBehaviour
{
	[SerializeField]
	private Text myText;

	[SerializeField]
	private Animation slideAnimation;

	[SerializeField]
	public List<string> MatchTitles;

	public static TitleCard instance;

	[SerializeField]
	private AudioSource drums;

	private void Start()
	{
		instance = this;
	}

	public void MatchTitle()
	{
		if (TutorialManager.onTutorial)
		{
			TextSlide("Pre Season");
		}
		else
		{
			TextSlide(MatchTitles[GameManager.gameNumber]);
		}
	}

	public void TextSlide(string text)
	{
		myText.text = text;
		drums.volume /= GameManager.Instance.saveData.sfxVolume;
		drums.Play();
		slideAnimation.Play();
	}
}
