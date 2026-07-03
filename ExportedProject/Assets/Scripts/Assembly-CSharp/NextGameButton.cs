using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NextGameButton : OnClick
{
	[SerializeField]
	private UpgradeManager upgradeManager;

	private float originalScale;

	[Header("skip button")]
	[SerializeField]
	private TextMeshPro buttonText;

	[SerializeField]
	private bool skipButton;

	private void Start()
	{
		originalScale = base.transform.localScale.x;
		if (skipButton)
		{
			if (GameManager.Instance.ControllerInput.controllerConnected)
			{
				buttonText.text = "Skip (Start)";
			}
			else
			{
				buttonText.text = "Skip";
			}
		}
	}

	public override void Click()
	{
		upgradeManager.MakeState();
		StartCoroutine(SelectedAnimation());
	}

	private IEnumerator SelectedAnimation()
	{
		base.transform.DOScale(originalScale * 1.4f, 0.2f).SetEase(Ease.OutExpo);
		yield return new WaitForSeconds(0.2f);
		base.transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack);
		yield return new WaitForSeconds(0.3f);
		base.transform.DOMoveY(-17.33f, 0.3f).SetEase(Ease.InCubic);
	}
}
