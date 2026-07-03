using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PoseController : MonoBehaviour
{
	private int num = -1;

	[SerializeField]
	private List<AnimationClip> poses;

	private Animation animation;

	private float originalScale;

	private void Start()
	{
		animation = base.gameObject.GetComponent<Animation>();
		originalScale = base.transform.localScale.x;
	}

	public void NextPose()
	{
		base.transform.localScale /= 2f;
		base.transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack);
		num++;
		animation.clip = poses[num];
		animation.Play();
	}
}
