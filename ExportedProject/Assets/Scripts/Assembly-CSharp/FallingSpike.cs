using UnityEngine;

[CreateAssetMenu(menuName = "Technique/FallingSpike")]
public class FallingSpike : Technique
{
	[SerializeField]
	private GameObject effect;

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
		if ((rb.linearVelocityX < 0f && position.position.x < 0f) || (rb.linearVelocityX > 9f && position.position.x > 0f))
		{
			Object.Instantiate(effect, ball.transform.position, Quaternion.identity);
			CameraController.Shake(0.1f);
			return 0.4f;
		}
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
