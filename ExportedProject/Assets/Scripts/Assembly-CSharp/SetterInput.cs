using UnityEngine;
using UnityEngine.InputSystem;

public class SetterInput : MonoBehaviour
{
	private PlayerController player;

	private float defensePosition;

	private Transform ball;

	private float ballSide;

	private bool shouldBlock;

	private float mySide;

	private float blockTimer;

	private float currentBlockVariation;

	private float controllerReadBlockTimer;

	private Rigidbody2D ballRb;

	[Header("Behaviour")]
	[SerializeField]
	private bool anchoredPosition;

	[SerializeField]
	private bool defensiveStructure;

	[SerializeField]
	private float defenceLevel;

	[SerializeField]
	private int blockChance;

	[SerializeField]
	private int blockTiming;

	[SerializeField]
	private float blockVariation;

	[SerializeField]
	private bool readBlock;

	[SerializeField]
	private float blockRange;

	[SerializeField]
	private Transform teammate;

	private float quickTimer;

	private void Start()
	{
		defenceLevel += Random.Range(-1f, 1f);
		defensePosition = base.transform.position.x;
		mySide = base.transform.position.x / Mathf.Abs(base.transform.position.x);
		player = base.gameObject.GetComponent<PlayerController>();
		ball = GameObject.FindGameObjectWithTag("Ball").transform;
		ballRb = ball.GetComponent<Rigidbody2D>();
		if (readBlock)
		{
			InputActionMap gameplay = GameManager.Instance.inputManager.Gameplay;
			gameplay.FindAction("UpP1").performed += SetReadBlockTimer;
			gameplay.FindAction("UpP2").performed += SetReadBlockTimer;
		}
	}

	private void OnDestroy()
	{
		InputActionMap gameplay = GameManager.Instance.inputManager.Gameplay;
		gameplay.FindAction("UpP1").performed -= SetReadBlockTimer;
		gameplay.FindAction("UpP2").performed -= SetReadBlockTimer;
	}

	private void SetReadBlockTimer(InputAction.CallbackContext ctx)
	{
		controllerReadBlockTimer = 0.2f;
	}

	private void Update()
	{
		controllerReadBlockTimer -= Time.deltaTime;
		quickTimer -= Time.deltaTime;
		float num = ballSide;
		ballSide = ball.position.x / Mathf.Abs(ball.position.x);
		blockTimer -= Time.deltaTime;
		if (num != ballSide && ballSide == -1f)
		{
			shouldBlock = Random.Range(1, blockChance) != 1;
			currentBlockVariation = Random.Range(0f - blockVariation, blockVariation);
			blockTimer = 1f;
		}
		if (BallMovement.serving || BallMovement.freeBall)
		{
			shouldBlock = false;
		}
		if (ballSide == mySide && !BallMovement.serving && !BallMovement.jumpForSpike)
		{
			if (!(Mathf.Abs(BallMovement.precisePredictedPosition) > 34f) || (float)BallMovement.lastTouched == mySide)
			{
				if (defensiveStructure && !TutorialManager.onTutorial)
				{
					moveGeneral(BallMovement.precisePredictedPosition);
				}
				else
				{
					moveGeneral(ball.position.x);
				}
			}
		}
		else if (shouldBlock && player.attackDirection == -1f)
		{
			moveGeneral(1.5f);
			if (ballSide != mySide && blockTimer < 0f && ((ballRb.linearVelocity.y < (float)(-blockTiming) + currentBlockVariation && ball.position.x > 0f - blockRange && !readBlock) || (readBlock && Input.GetKeyDown(KeyCode.UpArrow)) || (readBlock && Input.GetKeyDown(KeyCode.W)) || (readBlock && controllerReadBlockTimer > 0f)))
			{
				player.UpInputPressed();
				blockTimer = 1f;
			}
		}
		else if (anchoredPosition)
		{
			moveGeneral(defensePosition + Mathf.Sin(Time.time * defenceLevel) * 2f);
		}
		else
		{
			moveGeneral(defensePosition + Mathf.Sin(Time.time * defenceLevel) * 2f + BallMovement.positionVariation);
		}
	}

	private void moveGeneral(float target)
	{
		if (Mathf.Abs(base.transform.position.x - target) > 0.6f)
		{
			if (target > base.transform.position.x)
			{
				player.SetXInput(1f);
			}
			else
			{
				player.SetXInput(-1f);
			}
		}
		else
		{
			player.SetXInput(0f);
		}
	}
}
