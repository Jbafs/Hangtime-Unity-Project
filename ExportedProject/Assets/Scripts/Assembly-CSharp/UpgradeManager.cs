using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
	public enum State
	{
		Intro = 0,
		Preperation = 1,
		Outro = 2
	}

	private float dialogueOriginalScale;

	private Animation dialogueAnimation;

	private float originalStatPosition;

	private float originalButtonPosition;

	private static int indexNum;

	private bool stillOnUpgrade;

	[Header("Dialogue")]
	[SerializeField]
	private DialogueManager dialogueManager;

	[SerializeField]
	private PoseController[] poses;

	[Header("UI")]
	[SerializeField]
	private TextMeshPro dialogue;

	private TypeWriterEffect dialogueEffect;

	[SerializeField]
	private TextMeshPro stats;

	[SerializeField]
	private GameObject nextButton;

	private ButtonController nextButtonController;

	[SerializeField]
	private GameObject skipButton;

	private ButtonController skipButtonController;

	[SerializeField]
	private Transform StatChartUI;

	[SerializeField]
	private StatChart statChart;

	[Header("Upgrades")]
	[SerializeField]
	private List<Logo_technique> techniques;

	[SerializeField]
	private List<UpgradePair> upgrades;

	[SerializeField]
	private ButtonGroup cards;

	[SerializeField]
	private int limitBreakLevel;

	[SerializeField]
	private Logo_technique limitBreakTechnique;

	[Header("Buttons")]
	[SerializeField]
	private GameObject upgradeButton;

	[SerializeField]
	private GameObject techniqueButton;

	private List<ButtonController> buttons = new List<ButtonController>();

	public static bool gotUpgrade;

	public State state;

	[Header("Sounds")]
	[SerializeField]
	private SoundLibrary soundLibrary;

	[SerializeField]
	private AudioSource music;

	private void Start()
	{
		GameManager.Instance.inputManager.UpdateInputMaps(menuInputActive: true, "Upgrade!");
		GameManager.SetMusic(music.clip, music.volume);
		originalStatPosition = stats.transform.position.x;
		originalButtonPosition = nextButton.transform.position.y;
		dialogueAnimation = dialogue.GetComponent<Animation>();
		dialogueOriginalScale = dialogue.rectTransform.localScale.x;
		gotUpgrade = false;
		state = State.Intro;
		MakeState();
		dialogueEffect = dialogue.GetComponent<TypeWriterEffect>();
		dialogue.transform.localScale = Vector3.zero;
		nextButton.transform.position = new Vector3(nextButton.transform.position.x, -17.33f);
		nextButtonController = nextButton.GetComponent<ButtonController>();
		nextButtonController.interactable = false;
		skipButton.transform.position += new Vector3(20f, 0f);
		skipButtonController = skipButton.GetComponent<ButtonController>();
		skipButtonController.interactable = false;
		if (GameManager.Instance.ControllerInput.controllerConnected)
		{
			_ = GameManager.Instance.inputManager.menus;
			GameManager.Instance.ControllerInput.pauseInput += SkipUpgrade;
		}
	}

	private void OnDestroy()
	{
		_ = GameManager.Instance.inputManager.Gameplay;
		GameManager.Instance.ControllerInput.pauseInput -= SkipUpgrade;
	}

	private void SkipUpgrade()
	{
		ButtonController component = skipButton.GetComponent<ButtonController>();
		component.isHovering = true;
		component.Press();
		Debug.Log("Skip!");
	}

	private void UpdatePoses()
	{
		PoseController[] array = poses;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].NextPose();
		}
	}

	public void MakeState()
	{
		UpdatePoses();
		switch (state)
		{
		case State.Intro:
			StartCoroutine(CreateDialogue(dialogueManager.GetWinIntro()));
			StartCoroutine(CreateRecap());
			break;
		case State.Preperation:
			stillOnUpgrade = true;
			stats.transform.DOMoveX(originalStatPosition, 0.8f).SetEase(Ease.InCubic);
			stats.transform.DORotate(new Vector3(0f, 0f, 0f), 0.3f).SetEase(Ease.InCubic);
			StartCoroutine(CreateDialogue(dialogueManager.GetPrep()));
			StartCoroutine(CreateUpgrade());
			break;
		case State.Outro:
			stillOnUpgrade = false;
			StartCoroutine(CreateDialogue(dialogueManager.GetOutro()));
			StartCoroutine(LoadGame());
			break;
		}
		state++;
	}

	private IEnumerator CreateDialogue(string text)
	{
		dialogueAnimation.Stop();
		dialogue.rectTransform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
		yield return new WaitForSeconds(0.3f);
		dialogue.text = "";
		yield return new WaitForSeconds(1f);
		dialogue.rectTransform.DOScale(dialogueOriginalScale, 0.5f).SetEase(Ease.OutBack);
		StartCoroutine(dialogueEffect.TypeText(text, !DialogueManager.spikerTalking));
		yield return new WaitForSeconds(0.5f);
		dialogueAnimation.Play();
	}

	private IEnumerator CreateRecap()
	{
		stats.transform.position = new Vector3(originalStatPosition, stats.transform.position.y, 0f);
		stats.text = "\n\nAgainst: " + GameManager.Instance.opponentTeam.teamName + "\nFinal score: " + StatTracker.playerScore + " - " + StatTracker.opponentScore + "\nAces: " + StatTracker.aces + "\nBlocks: " + StatTracker.blocks + "\nPerfect Receives: " + StatTracker.perfectRecieves + "\nSpikes: " + StatTracker.spikes + "\nTips: " + StatTracker.tips;
		yield return new WaitForSeconds(1f);
		AudioManager.Instance.PlaySFX(soundLibrary.buttonWhoosh, 0.25f);
		stats.transform.DOMoveX(7.5f, 0.5f).SetEase(Ease.OutCubic);
		stats.transform.DORotate(new Vector3(0f, 0f, -1f), 0.5f).SetEase(Ease.OutCubic);
		stats.enabled = true;
		yield return null;
		StartCoroutine(DelayNextButton(2f));
	}

	private IEnumerator DelayNextButton(float time)
	{
		yield return new WaitForSeconds(time);
		nextButton.transform.DOMoveY(originalButtonPosition, 0.5f).SetEase(Ease.OutBack);
		yield return new WaitForSeconds(0.2f);
		nextButtonController.interactable = true;
	}

	private IEnumerator CreateUpgrade()
	{
		dialogue.text = "";
		yield return new WaitForSeconds(0.5f);
		bool flag = ((GameManager.gameNumber <= limitBreakLevel) ? ((GameManager.gameNumber + 1) % 2 == 0) : (GameManager.gameNumber % 2 == 0));
		if (GameManager.gameNumber == limitBreakLevel)
		{
			GameObject gameObject = Object.Instantiate(techniqueButton, new Vector3(0f, -1f, 0f), Quaternion.identity);
			buttons.Add(gameObject.GetComponent<ButtonController>());
			gameObject.GetComponent<TechniqueButton>().technique = limitBreakTechnique;
		}
		else if (flag)
		{
			Vector3 spawnPosition = new Vector3(3f, 0f, 0f);
			Logo_technique oldTechnique = null;
			for (int i = 0; i < 3; i += 2)
			{
				GameObject gameObject2 = Object.Instantiate(techniqueButton, spawnPosition * ((float)i * 2.5f - 2.5f) + new Vector3(0f, -1f, 0f), Quaternion.identity);
				buttons.Add(gameObject2.GetComponent<ButtonController>());
				Logo_technique logo_technique = techniques[Random.Range(0, techniques.Capacity)];
				List<Technique> allTechniques = GameManager.Instance.playerStats.GetAllTechniques();
				bool flag2 = true;
				while (flag2)
				{
					logo_technique = techniques[Random.Range(0, techniques.Capacity)];
					flag2 = false;
					foreach (Technique item in allTechniques)
					{
						if (item == logo_technique.GetTechnique())
						{
							flag2 = true;
						}
					}
					if (oldTechnique == logo_technique)
					{
						flag2 = true;
					}
				}
				gameObject2.GetComponent<TechniqueButton>().technique = logo_technique;
				oldTechnique = logo_technique;
				yield return new WaitForSeconds(0.1f);
			}
		}
		else
		{
			Vector3 spawnPosition = new Vector3(5f, 0f, 0f);
			for (int i = 0; i < 3; i++)
			{
				GameObject gameObject3 = Object.Instantiate(upgradeButton, spawnPosition * ((float)i * 2f - 2f) + new Vector3(0f, 1f, 5f), Quaternion.identity);
				buttons.Add(gameObject3.GetComponent<ButtonController>());
				indexNum += Random.Range(1, 7);
				if (indexNum > upgrades.Capacity - 1)
				{
					indexNum -= upgrades.Capacity - 1;
				}
				gameObject3.GetComponent<UpgradeButton>().myPair = upgrades[indexNum];
				yield return new WaitForSeconds(0.1f);
			}
			StatChartUI.transform.DOMoveY(-9.5f, 0.6f).SetEase(Ease.OutBack);
		}
		cards.enabled = true;
		foreach (ButtonController button in buttons)
		{
			cards.buttons.Add(button);
		}
		cards.Activate();
		yield return new WaitForSeconds(1f);
		skipButtonController.interactable = true;
		skipButton.transform.DOMoveX(25.2f, 0.5f).SetEase(Ease.OutBack);
		while (!gotUpgrade && stillOnUpgrade)
		{
			yield return null;
		}
		statChart.SetPoints();
		skipButtonController.interactable = false;
		skipButton.transform.DOMoveX(45.2f, 0.5f).SetEase(Ease.InBack);
		foreach (ButtonController button2 in buttons)
		{
			button2.interactable = false;
			if (!button2.wasClicked)
			{
				button2.NotUsed();
			}
		}
		if (stillOnUpgrade)
		{
			MakeState();
		}
	}

	private IEnumerator LoadGame()
	{
		yield return new WaitForSeconds(4f);
		GameManager.gameNumber++;
		GameManager.Instance.transition.Play();
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene("Game");
	}
}
