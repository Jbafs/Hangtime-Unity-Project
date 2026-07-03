using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static Transform cameraTransform;

	[SerializeField]
	private Transform ball;

	[SerializeField]
	private Camera camera;

	[SerializeField]
	private float directionalShift;

	[SerializeField]
	private float shift;

	[SerializeField]
	private float maxCameraSize;

	[SerializeField]
	private float serveCameraSize;

	[SerializeField]
	private Transform player;

	[SerializeField]
	private float playerTarget;

	[SerializeField]
	private float limitRight;

	[SerializeField]
	private float limitLeft;

	private float originalSize;

	public static float cameraSize;

	public static float cameraZoom;

	public static float shake;

	private static float freezeTimer;

	public static float rotation;

	private void Start()
	{
		shake = 0f;
		originalSize = camera.orthographicSize;
		cameraZoom = 0f;
		cameraSize = originalSize;
		cameraTransform = base.transform;
	}

	private void FixedUpdate()
	{
		if (GameManager.gameOver)
		{
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, originalSize + 2f, Time.deltaTime * 5f);
			base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(-10f + Mathf.Sin(Time.time * 1.5f), Mathf.Sin(Time.time * 0.5f) * 0.3f, -10f), Time.deltaTime * 0.5f);
		}
		else
		{
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, cameraSize, Time.deltaTime * 50f);
			cameraSize = Mathf.Lerp(cameraSize, Math.Clamp(originalSize + cameraZoom + (ball.position.y - 5f) / 7f + Mathf.Abs(ball.position.x) / 10f, 5f, maxCameraSize), Time.deltaTime * 3f);
			cameraZoom *= 0.97f;
			Vector3 b = new Vector3(ball.position.x / 3f + shift + player.position.x * playerTarget, 1.5f, -10f);
			if (base.transform.position.x > limitRight)
			{
				base.transform.position = new Vector3(limitRight, base.transform.position.y, -10f);
			}
			if (base.transform.position.x < limitLeft)
			{
				base.transform.position = new Vector3(limitLeft, base.transform.position.y, -10f);
			}
			if (TitleController.onTitle)
			{
				b.x += Mathf.Sin(Time.time * 1.5f);
			}
			if (BallMovement.rallyDone)
			{
				b = new Vector3(0f, 0f, -10f);
			}
			if (BallMovement.lastTouched == -1)
			{
				b += new Vector3(directionalShift, 0f, 0f);
			}
			else
			{
				b -= new Vector3(directionalShift, 0f, 0f);
			}
			base.transform.position = Vector3.Lerp(base.transform.position, b, Time.deltaTime * 2f);
			if (BallMovement.serving && !TitleController.onTitle && !GameManager.gameOver)
			{
				cameraZoom = serveCameraSize;
			}
		}
		base.transform.position += new Vector3(UnityEngine.Random.Range(0f - shake, shake), UnityEngine.Random.Range(0f - shake, shake));
		shake *= 0.85f;
		rotation *= 0.99f;
		base.transform.eulerAngles = new Vector3(0f, 0f, rotation);
	}

	private void Update()
	{
		if (PauseController.paused)
		{
			Time.timeScale = 0f;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
		}
		else if (freezeTimer > 0f)
		{
			Time.timeScale = 0.1f;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
			freezeTimer -= Time.deltaTime;
		}
		else
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
		}
	}

	public static void zoom(float inputZoom)
	{
		cameraZoom = inputZoom;
	}

	public static void freezeTime(float amount)
	{
		freezeTimer += amount;
	}

	public static void Shake(float amount)
	{
		if (amount > shake)
		{
			shake = amount;
		}
	}

	public static void Rotate(float amount)
	{
		rotation = amount;
	}
}
