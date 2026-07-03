using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UpgradeButton : OnClick
{
	public UpgradePair myPair;

	public TextMeshPro title;

	public TextMeshPro description;

	private Vector2 aimPosition;

	private bool gotPicked;

	[SerializeField]
	private ParticleSystem collected;

	private float originalScale;

	[SerializeField]
	private SoundLibrary soundLibrary;

	private string statNameDescription;

	private string fullDescription;

	private void Start()
	{
		AudioManager.Instance.PlaySFX(soundLibrary.buttonWhoosh, 0.25f);
		originalScale = base.transform.localScale.x;
		aimPosition = base.transform.position;
		base.transform.position = aimPosition + new Vector2(0f, 26f);
		base.transform.DOMoveY(aimPosition.y, 1f).SetEase(Ease.OutExpo);
		base.transform.DORotate(new Vector3(0f, 0f, (0f - base.transform.position.x) / 2f), 0.5f).SetEase(Ease.OutCubic);
		title.text = myPair.title;
		statNameDescription = myPair.description.Replace(" ", "\n").Replace("_", " ");
		description.text = statNameDescription;
		fullDescription = "";
		string[] array = myPair.description.Split(" ");
		foreach (string text in array)
		{
			fullDescription = fullDescription + StatDescriptionController.GetDescription(text.Replace("+", "").Replace("++", "")) + "\n\n";
		}
		fullDescription = fullDescription.Substring(0, fullDescription.Length - 2);
	}

	private void Update()
	{
		if (base.transform.localScale.x > 8.5f)
		{
			description.text = fullDescription;
		}
		else
		{
			description.text = statNameDescription;
		}
	}

	public override void Click()
	{
		foreach (string upgrade in myPair.upgrades)
		{
			GameManager.Instance.playerStats.GetStat(upgrade).Upgrade();
		}
		if (GameManager.Instance.playerStats.GetStat("Bump").GetLevel() >= 4f)
		{
			AchievementManager.I.Unlock(AchievementManager.Id.MaxOutBump);
		}
		UpgradeManager.gotUpgrade = true;
		collected.Play();
		StartCoroutine(CollectedAnimation());
		gotPicked = true;
		description.text = statNameDescription;
	}

	public IEnumerator CollectedAnimation()
	{
		base.transform.DORotate(new Vector3(0f, 0f, 0f), 0.1f).SetEase(Ease.InCubic);
		base.transform.DOScale(originalScale * 1.3f, 0.4f).SetEase(Ease.OutExpo);
		yield return new WaitForSeconds(0.4f);
		base.transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack);
		yield return new WaitForSeconds(0.5f);
		base.transform.DOMoveY(aimPosition.y - 26f, 0.3f).SetEase(Ease.InCubic);
	}
}
