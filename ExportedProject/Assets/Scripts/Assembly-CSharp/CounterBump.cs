using UnityEngine;

[CreateAssetMenu(menuName = "Technique/CounterBump")]
public class CounterBump : Technique
{
	[SerializeField]
	private GameObject effect;

	[SerializeField]
	private BallModifier dragModifier;

	public override void OnBump(BallMovement ball, Transform position)
	{
		Rigidbody2D component = position.GetComponent<Rigidbody2D>();
		if (Mathf.Abs(component.linearVelocity.x) > 30f)
		{
			float num = (0f - position.position.x) / Mathf.Abs(position.position.x);
			if (0f - component.linearVelocityX / Mathf.Abs(component.linearVelocityX) == num && Mathf.Abs(component.linearVelocityX) > 5f)
			{
				Object.Instantiate(effect, position.position, Quaternion.identity);
				CameraController.Shake(0.05f);
				component.linearVelocity *= 0.5f;
				ball.GetComponent<Rigidbody2D>().linearVelocity /= 1.2f;
			}
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
