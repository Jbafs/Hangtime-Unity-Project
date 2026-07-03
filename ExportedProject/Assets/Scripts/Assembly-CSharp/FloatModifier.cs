using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/FloatServeModifier")]
public class FloatModifier : BallModifier
{
	public override void Apply(Rigidbody2D ballRb)
	{
		ballRb.linearDamping = 0.1f;
		BallMovement.bonusGravity = -4f;
	}

	public override void Remove(Rigidbody2D ballRb)
	{
		ballRb.linearDamping = 0.4f;
		BallMovement.bonusGravity = 0f;
	}

	public override void OnBlock(Rigidbody2D ballRb)
	{
	}

	public override void OnBump(Rigidbody2D ballRb)
	{
		Remove(ballRb);
	}
}
