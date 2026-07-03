using UnityEngine;

[CreateAssetMenu(menuName = "Technique/Speedster")]
public class Speedster : Technique
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
		return 0f;
	}

	public override void OnStart(GameObject thisObject)
	{
		thisObject.GetComponent<PlayerController>().moveSpeed += 0.6f;
		thisObject.GetComponent<PlayerController>().dragAmount = 4f;
	}

	public override void OnTip(BallMovement ball)
	{
	}

	public override float OnBlockJump()
	{
		return 0f;
	}
}
