using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PauseAnimation : MonoBehaviour
{
	[SerializeField]
	private Transform pauseText;

	[SerializeField]
	private Transform[] buttons;

	[SerializeField]
	private SpriteRenderer fade;

	public IEnumerator PlayAnimation()
	{
		pauseText.localScale = new Vector3(0.001f, 0.001f, 0f);
		pauseText.localPosition = new Vector3(-0.026f, 0.093f, 0f);
		pauseText.DOScale(0.0013f, 0.2f).SetEase(Ease.OutBack).SetUpdate(isIndependentUpdate: true);
		pauseText.DOLocalMoveX(0.026f, 0.3f).SetEase(Ease.OutBack).SetUpdate(isIndependentUpdate: true);
		Transform[] array = buttons;
		foreach (Transform button in array)
		{
			yield return null;
			yield return null;
			button.localScale = new Vector3(0f, 0f, 1f);
		}
		array = buttons;
		foreach (Transform button in array)
		{
			yield return new WaitForSecondsRealtime(0.08f);
			button.localPosition = new Vector3(-0.25f, button.localPosition.y, 0f);
			button.localScale = new Vector3(0.005f, 0.005f, 1f);
			button.DOLocalMoveX(-0.16f, 0.3f).SetEase(Ease.OutBack).SetUpdate(isIndependentUpdate: true);
			button.DOScale(0.02f, 0.2f).SetEase(Ease.OutBack).SetUpdate(isIndependentUpdate: true);
		}
	}
}
