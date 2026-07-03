using UnityEngine;

[CreateAssetMenu(menuName = "Technique/StrongQuick")]
public class StrongQuick : Technique
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
		if (ball.GetComponent<Rigidbody2D>().linearVelocityY > 0f)
		{
			Object.Instantiate(effect, ball.transform.position, Quaternion.identity);
			return 0.3f;
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
