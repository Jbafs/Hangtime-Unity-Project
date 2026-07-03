using UnityEngine;

public class ParallaxMover : MonoBehaviour
{
	[SerializeField]
	private float shiftAmount;

	[Header("Non game cameras")]
	[SerializeField]
	private bool otherTransform;

	[SerializeField]
	private Transform target;

	private float originalPostionX;

	private void Start()
	{
		originalPostionX = base.transform.position.x;
	}

	private void FixedUpdate()
	{
		if (otherTransform)
		{
			base.transform.position = new Vector3(target.position.x / shiftAmount + originalPostionX, base.transform.position.y, 0f);
		}
		else
		{
			base.transform.position = new Vector3(CameraController.cameraTransform.position.x / shiftAmount, base.transform.position.y, 0f);
		}
	}
}
