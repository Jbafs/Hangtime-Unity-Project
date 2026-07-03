using System.Collections;
using UnityEngine;

public class TutorialDiagram : MonoBehaviour
{
	[SerializeField]
	private GameObject far;

	[SerializeField]
	private GameObject close;

	private Animation animation;

	private Vector3 originalScale;

	private void Awake()
	{
		animation = base.gameObject.GetComponent<Animation>();
		originalScale = base.transform.localScale;
		StartCoroutine(Swap());
		Stop();
	}

	public void Play()
	{
		base.transform.localScale = originalScale;
		animation.Play();
	}

	public void Stop()
	{
		animation.Stop();
		base.transform.localScale = new Vector3(0f, 0f, 0f);
	}

	private IEnumerator Swap()
	{
		yield return new WaitForSeconds(1.5f);
		if (far.activeSelf)
		{
			far.SetActive(value: false);
			close.SetActive(value: true);
		}
		else
		{
			far.SetActive(value: true);
			close.SetActive(value: false);
		}
		StartCoroutine(Swap());
	}
}
