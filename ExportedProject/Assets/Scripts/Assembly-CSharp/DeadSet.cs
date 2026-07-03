using UnityEngine;

[CreateAssetMenu(menuName = "Technique/DeadSet")]
public class DeadSet : Technique
{
	[SerializeField]
	private GameObject effect;

	[SerializeField]
	private BallModifier dragModifier;

	public override void OnBump(BallMovement ball, Transform position)
	{
		if (Mathf.Abs(position.GetComponent<Rigidbody2D>().linearVelocity.x) < 30f)
		{
			ball.ApplyModifier(dragModifier);
		}
	}

	public override float OnJump(Transform position)
	{
		return 0f;
	}

	public override void OnServe(BallMovement ball, Transform position)
	{
	}

	public override float OnSpike(BallMovement ball, Rigidbody2D rb, Transform position)
	{
		return 0f;
	}

	public override void OnStart(GameObject thisObject)
	{
	}

	public override void OnTip(BallMovement ball)
	{
	}

	public override float OnBlockJump()
	{
		return 0f;
	}
}
