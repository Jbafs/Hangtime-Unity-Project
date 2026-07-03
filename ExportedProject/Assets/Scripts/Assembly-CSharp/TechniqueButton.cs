using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TechniqueButton : OnClick
{
	public Logo_technique technique;

	public TextMeshPro title;

	public TextMeshPro description;

	public TextMeshPro flavourText;

	[SerializeField]
	private ParticleSystem collected;

	private Vector2 aimPosition;

	private Vector2 originalScale;

	[SerializeField]
	private SoundLibrary soundLibrary;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private void Start()
	{
		AudioManager.Instance.PlaySFX(soundLibrary.buttonWhoosh, 0.25f);
		originalScale = base.transform.localScale;
		aimPosition = base.transform.position;
		base.transform.position = aimPosition + new Vector2(0f, 26f);
		base.transform.DOMoveY(aimPosition.y, 1f).SetEase(Ease.OutExpo);
		base.transform.DORotate(new Vector3(0f, 0f, (0f - base.transform.position.x) / 2f), 0.3f).SetEase(Ease.OutCubic);
		title.text = technique.title;
		description.text = technique.description;
		flavourText.text = technique.flavourText;
		if (technique.sprite != null)
		{
			spriteRenderer.sprite = technique.sprite;
		}
	}

	public override void Click()
	{
		GameManager.Instance.playerStats.techniques.Add(technique.GetTechnique());
		collected.Play();
		UpgradeManager.gotUpgrade = true;
		StartCoroutine(CollectedAnimation());
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
