using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/QuickDrop")]
public class QuickDrop : BallModifier
{
	[SerializeField]
	private float bonusGravityAmount;

	public override void Apply(Rigidbody2D ballRb)
	{
		BallMovement.bonusGravity = bonusGravityAmount;
	}

	public override void Remove(Rigidbody2D ballRb)
	{
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
