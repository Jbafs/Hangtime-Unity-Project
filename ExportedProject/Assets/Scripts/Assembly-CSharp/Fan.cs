using UnityEngine;

public class Fan : MonoBehaviour
{
	[SerializeField]
	private FanController fanController;

	[SerializeField]
	private float rotationSpeed;

	private Vector3 originalScale;

	[SerializeField]
	private float sizeChange;

	private void Start()
	{
		Random.Range(0, GameManager.gameNumber * 2 + 2);
		originalScale = base.transform.localScale;
		rotationSpeed *= Random.Range(0.9f, 1.1f);
		sizeChange *= Random.Range(0.9f, 1.1f);
	}

	private void Update()
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * rotationSpeed)) * 7f * (fanController.cheerAmount + 0.25f);
		base.transform.localScale = originalScale + new Vector3(1f, 1f) * Mathf.Sin(Time.time * sizeChange) * (fanController.cheerAmount + 0.25f) * 0.03f;
	}
}
