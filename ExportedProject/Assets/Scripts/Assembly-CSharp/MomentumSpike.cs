using UnityEngine;

[CreateAssetMenu(menuName = "Technique/MomentumSpike")]
public class MomentumSpike : Technique
{
	[SerializeField]
	private GameObject effect;

	[SerializeField]
	private float velocityThreshold;

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
		float num = (0f - position.position.x) / Mathf.Abs(position.position.x);
		if (rb.linearVelocityX * num > velocityThreshold)
		{
			Object.Instantiate(effect, ball.transform.position, Quaternion.identity).transform.eulerAngles = new Vector3(0f, 0f, num * 90f);
			CameraController.freezeTime(0.001f);
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
