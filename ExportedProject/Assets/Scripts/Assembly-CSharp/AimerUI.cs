using UnityEngine;

public class AimerUI : MonoBehaviour
{
	[SerializeField]
	private Transform playerTransform;

	private SpriteRenderer mySprite;

	[SerializeField]
	private PlayerController player;

	private void Start()
	{
		mySprite = base.gameObject.GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		float num = Mathf.Abs(playerTransform.position.x - base.transform.position.x);
		base.transform.eulerAngles = new Vector3(0f, 0f, -90f);
		base.transform.Rotate(0f, 0f, num * 15f * -1f);
		base.transform.Rotate(0f, 0f, (player.GetSpikePower() - 1f) * -6f);
		if (Vector3.Distance(base.transform.position, playerTransform.position) < 12f && player.ShowIndicator())
		{
			mySprite.enabled = true;
		}
		else
		{
			mySprite.enabled = false;
		}
	}
}
