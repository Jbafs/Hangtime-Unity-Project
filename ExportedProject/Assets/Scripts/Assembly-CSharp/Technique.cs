using UnityEngine;

public abstract class Technique : ScriptableObject
{
	public float timer;

	public abstract void OnStart(GameObject thisObject);

	public abstract void OnBump(BallMovement ball, Transform position);

	public abstract float OnSpike(BallMovement ball, Rigidbody2D rb, Transform position);

	public abstract float OnJump(Transform position);

	public abstract void OnServe(BallMovement ball, Transform position);

	public abstract void OnTip(BallMovement ball);

	public abstract float OnBlockJump();
}
