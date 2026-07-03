using UnityEngine;

public class SpikerInput : MonoBehaviour
{
	public static float maxHeight;

	private PlayerController player;

	private float defensePosition;

	private Transform ballTransform;

	private float ballSide;

	private float mySide;

	private Rigidbody2D rb;

	private float currentServeVariance;

	private bool canServe;

	private bool shouldFloat;

	private float jumpTimer;

	private float currentSpikeAim;

	private float serveWait;

	[Header("Spike aim: 1 (long) - 5 (cut)")]
	[SerializeField]
	private Vector2 spikeAim;

	[Header("Behaviour")]
	[SerializeField]
	private bool dontJumpEarly;

	[SerializeField]
	private bool smartAim;

	[SerializeField]
	private float defenceLevel;

	[SerializeField]
	private float agressiveness;

	[SerializeField]
	private int tipChance;

	[SerializeField]
	private bool canSpikeQuicks;

	[SerializeField]
	private bool doubleServer;

	[SerializeField]
	private bool defensiveStructure;

	[SerializeField]
	private bool canSpikeFirstTouch;

	[Header("Serving")]
	[SerializeField]
	private float serveHeight;

	[SerializeField]
	private float serveVariance;

	[SerializeField]
	private float maxFloatHeight;

	private Rigidbody2D ballRb;

	private void Start()
	{
		player = base.gameObject.GetComponent<PlayerController>();
		shouldFloat = player.stats.GetStat("FloatServe").GetLevel() > player.stats.GetStat("SpinServe").GetLevel();
		rb = base.gameObject.GetComponent<Rigidbody2D>();
		defenceLevel += Random.Range(-1f, 1f);
		defensePosition = base.transform.position.x;
		mySide = base.transform.position.x / Mathf.Abs(base.transform.position.x);
	}

	private void GetComponents()
	{
		ballTransform = GameManager.Instance.ball.transform;
		ballRb = ballTransform.GetComponent<Rigidbody2D>();
		ballTransform.GetComponent<BallMovement>().opponent = GetComponent<PlayerController>();
	}

	private void Update()
	{
		if (ballTransform == null)
		{
			GetComponents();
		}
		jumpTimer -= Time.deltaTime;
		ballSide = ballTransform.position.x / Mathf.Abs(ballTransform.position.x);
		if (ballSide == mySide)
		{
			if (player.IsServing())
			{
				doServe();
				return;
			}
			if (player.getGrounded())
			{
				if (BallMovement.lastTouched == 1)
				{
					if (BallMovement.predictedPosition > 20f)
					{
						moveGeneral(20f + Mathf.Sin(Time.time * 7f) * 2f);
					}
					else if (Mathf.Abs(ballRb.linearVelocityX) < 15f)
					{
						moveTowards(ballTransform.position.x + 3f + Mathf.Sin(Time.time * 6f) * 6f);
					}
					else
					{
						moveTowards(BallMovement.predictedPosition + Mathf.Sin(Time.time * 8f) * 3f);
					}
				}
				else if (defensiveStructure)
				{
					moveTowards(BallMovement.precisePredictedPosition);
				}
				else
				{
					moveTowards(BallMovement.predictedPosition);
				}
			}
			else
			{
				moveTowards(ballTransform.position.x + currentSpikeAim);
			}
			if (player.getGrounded() && jumpTimer < 0f && (BallMovement.lastTouched == 1 || canSpikeFirstTouch) && (BallMovement.jumpForSpike || (BallMovement.jumpForQuick && canSpikeQuicks && rb.linearVelocityX < -4f && player.setTimer < 0f) || (BallMovement.jumpForHighSpike && player.GetJumpPower() > 70f && !dontJumpEarly)) && (Mathf.Abs(ballTransform.position.x - base.transform.position.x) < 10f || (canSpikeFirstTouch && Mathf.Abs(ballTransform.position.x - base.transform.position.x) < 20f)))
			{
				player.UpInputPressed();
				jumpTimer = 1f;
				currentSpikeAim = Random.Range(spikeAim.x, spikeAim.y);
				if (BallMovement.jumpForQuick && canSpikeQuicks)
				{
					currentSpikeAim = spikeAim.y;
				}
				if (smartAim && base.transform.position.x > 8f)
				{
					currentSpikeAim -= -1.5f;
				}
			}
			if (!player.getGrounded() && Vector3.Distance(base.transform.position, ballTransform.position) < 6f + agressiveness && (base.transform.position.y > (player.GetJumpPower() - 61f) / 2f || (rb.linearVelocityY > 0f && base.transform.position.y > (player.GetJumpPower() - 61f) / 3f)) && (Mathf.Abs(Mathf.Abs(ballTransform.position.x - base.transform.position.x) - currentSpikeAim) < 0.1f || player.GetJumpPower() < 65f))
			{
				if ((Random.Range(1, tipChance) == 1 && !TutorialManager.onTutorial) || base.transform.position.y < 2f)
				{
					player.DownInputPressed();
				}
				else
				{
					player.UpInputPressed();
				}
			}
			canServe = false;
		}
		else
		{
			moveGeneral(defensePosition + Mathf.Sin(Time.time * defenceLevel) * 2f);
		}
	}

	private void doServe()
	{
		if (player.getGrounded())
		{
			if (serveWait < 0f)
			{
				moveTowards(33f);
			}
			else
			{
				moveGeneral(41f);
			}
			if (rb.linearVelocityX < 0f && base.transform.position.x < 34f && base.transform.position.x > 33f && jumpTimer < 0f && serveWait < 0f)
			{
				player.UpInputPressed();
				jumpTimer = 0.3f;
				canServe = true;
				currentServeVariance = Random.Range(0f - serveVariance, serveVariance);
				if (doubleServer)
				{
					shouldFloat = Random.Range(1, 3) == 1;
				}
			}
			if (base.transform.position.x > 33f)
			{
				serveWait -= Time.deltaTime;
			}
			else
			{
				serveWait = 1.25f;
			}
		}
		else if (shouldFloat)
		{
			moveGeneral(29f);
			if (((rb.linearVelocityY < 15f && canServe) || (base.transform.position.y > maxFloatHeight + currentServeVariance && maxFloatHeight != 0f)) && canServe)
			{
				player.DownInputPressed();
				canServe = false;
			}
		}
		else
		{
			moveTowards(0f);
			if (base.transform.position.y < serveHeight + currentServeVariance && rb.linearVelocityY < 0f && canServe)
			{
				player.UpInputPressed();
				canServe = false;
			}
		}
	}

	private void moveTowards(float target)
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

	private void moveGeneral(float target)
	{
		if (Mathf.Abs(base.transform.position.x - target) > 0.6f)
		{
			moveTowards(target);
		}
		else
		{
			player.SetXInput(0f);
		}
	}
}
