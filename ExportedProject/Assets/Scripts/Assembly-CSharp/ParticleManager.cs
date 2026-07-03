using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
	public GameObject sweatEffect;

	public GameObject jumpEffect;

	[SerializeField]
	private float poolAmount;

	private float poolCounter;

	private Queue<GameObject> jumpEffectQueue = new Queue<GameObject>();

	private void Start()
	{
		for (int i = 0; (float)i < poolAmount; i++)
		{
			GameObject gameObject = Object.Instantiate(jumpEffect, base.transform);
			jumpEffectQueue.Enqueue(gameObject);
			gameObject.SetActive(value: false);
		}
	}

	public void PlayJumpEffect(Transform playerTransform, Rigidbody2D rb)
	{
		GameObject gameObject = jumpEffectQueue.Dequeue();
		gameObject.SetActive(value: true);
		gameObject.transform.position = playerTransform.position;
		gameObject.transform.eulerAngles = new Vector3(0f, 0f, (0f - rb.linearVelocityX) / 1.5f);
		gameObject.transform.position -= new Vector3(0f, 2f);
		StartCoroutine(Disactivate(gameObject));
	}

	private IEnumerator Disactivate(GameObject effect)
	{
		yield return new WaitForSeconds(1f);
		effect.SetActive(value: false);
		jumpEffectQueue.Enqueue(effect);
	}
}
