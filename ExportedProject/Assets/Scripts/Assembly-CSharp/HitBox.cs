using UnityEngine;

public class HitBox : MonoBehaviour
{
	public enum Action
	{
		Set = 0,
		Spike = 1,
		Tip = 2
	}

	private PlayerController player;

	public Action action;

	private float timer;

	public bool playerPositionBased;

	private void Start()
	{
		player = base.gameObject.GetComponentInParent<PlayerController>();
	}

	private void Update()
	{
		timer -= Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!(collision.gameObject.tag == "Ball"))
		{
			return;
		}
		switch (action)
		{
		case Action.Set:
			if (collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity.y < 0f)
			{
				player.DoSet();
			}
			break;
		case Action.Spike:
			if (playerPositionBased)
			{
				player.DoSpike(player.transform.position);
			}
			else
			{
				player.DoSpike(base.transform.position);
			}
			break;
		case Action.Tip:
			player.DoTip();
			break;
		}
		base.gameObject.SetActive(value: false);
	}
}
