using UnityEngine;

[CreateAssetMenu(menuName = "Technique/SpinSpike")]
public class SpinSpike : Technique
{
	[SerializeField]
	private GameObject effect;

	[SerializeField]
	private BallModifier quickDrop;

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
		ball.ApplyModifier(quickDrop);
		Object.Instantiate(effect, ball.transform.position, Quaternion.identity);
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
