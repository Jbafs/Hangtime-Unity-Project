using UnityEngine;

[CreateAssetMenu(menuName = "Technique/ComeBack")]
public class ComeBack : Technique
{
	[SerializeField]
	private GameObject effect;

	public override float OnBlockJump()
	{
		return 0f;
	}

	public override void OnBump(BallMovement ball, Transform position)
	{
	}

	public override float OnJump(Transform position)
	{
		if (MatchPoint(position))
		{
			Object.Instantiate(effect, position.position, Quaternion.identity);
			return 6f;
		}
		return 0f;
	}

	public override void OnServe(BallMovement ball, Transform position)
	{
	}

	public override float OnSpike(BallMovement ball, Rigidbody2D rb, Transform position)
	{
		if (MatchPoint(position))
		{
			Object.Instantiate(effect, position.position, Quaternion.identity);
			return 0.4f;
		}
		return 0f;
	}

	private bool MatchPoint(Transform position)
	{
		if (GameManager.Instance.playerPoints == GameManager.Instance.matchLength - 1 || GameManager.Instance.opponentPoints == GameManager.Instance.matchLength - 1)
		{
			return true;
		}
		return false;
	}

	public override void OnStart(GameObject thisObject)
	{
	}

	public override void OnTip(BallMovement ball)
	{
	}
}
