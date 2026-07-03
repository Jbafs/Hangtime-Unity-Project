using UnityEngine;

[CreateAssetMenu(menuName = "Technique/LineCreep")]
public class LineCreep : Technique
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
		ball.GetComponent<Rigidbody2D>().linearVelocity *= 1.4f;
		Object.Instantiate(effect, ball.transform.position, Quaternion.identity).transform.eulerAngles = new Vector3(0f, 0f, ball.transform.position.x / Mathf.Abs(ball.transform.position.x) * -90f);
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
