using UnityEngine;

[CreateAssetMenu(menuName = "Technique/LastStand")]
public class LastStand : Technique
{
	[SerializeField]
	private GameObject effect;

	public override void OnBump(BallMovement ball, Transform position)
	{
	}

	public override float OnJump(Transform position)
	{
		if (position.position.x / Mathf.Abs(position.position.x) != (float)BallMovement.sideServing)
		{
			Object.Instantiate(effect, position.transform.position, Quaternion.identity);
			return 5f;
		}
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
