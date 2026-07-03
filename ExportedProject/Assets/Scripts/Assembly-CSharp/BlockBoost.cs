using UnityEngine;

[CreateAssetMenu(menuName = "Technique/BlockBoost")]
public class BlockBoost : Technique
{
	[SerializeField]
	private GameObject effect;

	public override float OnBlockJump()
	{
		if (BallMovement.touchCounter == 1)
		{
			CameraController.Shake(0.2f);
			timer = 0f;
			return 5f;
		}
		return 0f;
	}

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
	}
}
