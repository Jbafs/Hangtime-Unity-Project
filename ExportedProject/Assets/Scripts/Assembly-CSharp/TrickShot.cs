using UnityEngine;

[CreateAssetMenu(menuName = "Technique/TrickShot")]
public class TrickShot : Technique
{
	[SerializeField]
	private GameObject effect;

	private int trickCounter;

	private bool loadedShot;

	public override void OnBump(BallMovement ball, Transform position)
	{
	}

	public override float OnJump(Transform position)
	{
		loadedShot = false;
		if (trickCounter < 0 && Random.Range(1, 3) == 1)
		{
			Object.Instantiate(effect, position.position, Quaternion.identity);
			CameraController.freezeTime(0.005f);
			loadedShot = true;
		}
		return 0f;
	}

	public override void OnServe(BallMovement ball, Transform position)
	{
	}

	public override float OnSpike(BallMovement ball, Rigidbody2D rb, Transform position)
	{
		if (loadedShot)
		{
			loadedShot = false;
			trickCounter = 2;
			Object.Instantiate(effect, ball.transform.position, Quaternion.identity);
			CameraController.freezeTime(0.005f);
			CameraController.Shake(0.15f);
			return 0.6f;
		}
		trickCounter--;
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
