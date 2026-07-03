using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/BlockBounce")]
public class BlockBounce : BallModifier
{
	public override void Apply(Rigidbody2D ballRb)
	{
		ballRb.GetComponent<BallMovement>().tipVelocity = new Vector2(60f, 0f);
	}

	public override void Remove(Rigidbody2D ballRb)
	{
		ballRb.GetComponent<BallMovement>().tipVelocity = new Vector2(32f, 15f);
	}

	public override void OnBlock(Rigidbody2D ballRb)
	{
		Remove(ballRb);
	}

	public override void OnBump(Rigidbody2D ballRb)
	{
	}
}
