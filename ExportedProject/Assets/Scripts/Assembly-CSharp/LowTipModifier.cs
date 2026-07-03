using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/LowTip")]
public class LowTipModifier : BallModifier
{
	[SerializeField]
	private GameObject bounceEffect;

	public override void Apply(Rigidbody2D ballRb)
	{
		ballRb.GetComponent<BallMovement>().tipVelocity = new Vector2(45f, -10f);
	}

	public override void Remove(Rigidbody2D ballRb)
	{
		ballRb.GetComponent<BallMovement>().tipVelocity = new Vector2(32f, 15f);
	}

	public override void OnBlock(Rigidbody2D ballRb)
	{
		ballRb.linearVelocity += new Vector2(-5f, 70f);
		Remove(ballRb);
		Object.Instantiate(bounceEffect, ballRb.transform.position, Quaternion.identity);
	}

	public override void OnBump(Rigidbody2D ballRb)
	{
	}
}
