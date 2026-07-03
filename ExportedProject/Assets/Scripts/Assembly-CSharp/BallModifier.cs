using UnityEngine;

public abstract class BallModifier : ScriptableObject
{
	public float duration;

	public abstract void Apply(Rigidbody2D ballRb);

	public abstract void Remove(Rigidbody2D ballRb);

	public abstract void OnBlock(Rigidbody2D ballRb);

	public abstract void OnBump(Rigidbody2D ballRb);
}
