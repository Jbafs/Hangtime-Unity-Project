using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Drag")]
public class Drag : BallModifier
{
	public override void Apply(Rigidbody2D ballRb)
	{
		if (Mathf.Abs(ballRb.transform.position.x) < 20f)
		{
			BallMovement.bonusGravity = 15f;
			ballRb.linearVelocity *= new Vector2(1f, 1.5f);
		}
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
