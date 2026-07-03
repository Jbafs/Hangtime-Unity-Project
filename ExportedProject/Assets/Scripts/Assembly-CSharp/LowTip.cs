using UnityEngine;

[CreateAssetMenu(menuName = "Technique/LowTip")]
public class LowTip : Technique
{
	[SerializeField]
	private GameObject effect;

	[SerializeField]
	private BallModifier lowTip;

	public override void OnBump(BallMovement ball, Transform position)
	{
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
		ball.ApplyModifier(lowTip);
	}

	public override float OnBlockJump()
	{
		return 0f;
	}
}
