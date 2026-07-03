using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rb;

	private bool onGround;

	private float inputX;

	private float velocityDirection;

	private float inputDirection;

	private bool actedInAir;

	private bool blocking;

	private BallMovement ball;

	private bool serving;

	private float landTimer;

	private bool canServe;

	public float COURT_SIZE = 35.5f;

	public bool setter;

	[SerializeField]
	private bool velocityAim;

	[Header("Limit Break Effects")]
	[SerializeField]
	private LimitBreakEffects limitBreakEffects;

	[Header("Movement Stats")]
	[SerializeField]
	private float riseSpeed;

	[SerializeField]
	private float hangSpeed;

	[SerializeField]
	private float fallSpeed;

	public float moveSpeed;

	public float dragAmount;

	public float setTimer;

	public PlayerStats stats;

	private float spike;

	private float jump;

	private float block;

	private float bump;

	private float spinServe;

	private float serveJump;

	private float floatServe;

	private AudioSource footSteps;

	private bool hasFootsteps;

	private bool startedSweating;

	[Header("Movement")]
	[SerializeField]
	private Transform jumpCheckPosition;

	[SerializeField]
	private LayerMask groundLayer;

	[Header("Ball interaction")]
	public float attackDirection;

	[SerializeField]
	private bool dontScaleBlock;

	[SerializeField]
	private GameObject setHitBox;

	[SerializeField]
	private GameObject spikeHitBox;

	[SerializeField]
	private BoxCollider2D blockHitBox;

	[SerializeField]
	private GameObject tipHitBox;

	[SerializeField]
	private Transform servePoint;

	[Header("Techniques")]
	public List<Technique> techniques;

	private AbilityBooleans abilityBooleans;

	[Header("Animation")]
	[SerializeField]
	private Animator animator;

	private Transform playerSprite;

	[Header("Sounds")]
	[SerializeField]
	private SoundLibrary soundLibrary;

	private void Start()
	{
		footSteps = base.gameObject.GetComponent<AudioSource>();
		if ((bool)footSteps)
		{
			hasFootsteps = true;
		}
		abilityBooleans = base.gameObject.GetComponent<AbilityBooleans>();
		StartCoroutine(setStats());
		ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMovement>();
		rb = base.gameObject.GetComponent<Rigidbody2D>();
		if (base.transform.position.x < 0f)
		{
			attackDirection = 1f;
		}
		else
		{
			attackDirection = -1f;
		}
		rb.gravityScale = 5f;
		canServe = false;
		Physics2D.gravity = new Vector2(0f, -9.81f);
		playerSprite = animator.transform;
		DifficultyHitBoxRescale();
	}

	private IEnumerator setStats()
	{
		yield return null;
		stats = GetComponentInParent<PlayerStats>();
		spike = stats.GetStat("Spike").GetCurrentValue();
		jump = stats.GetStat("Jump").GetCurrentValue();
		block = stats.GetStat("Block").GetCurrentValue();
		bump = stats.GetStat("Bump").GetCurrentValue();
		spinServe = stats.GetStat("SpinServe").GetCurrentValue();
		serveJump = stats.GetStat("ServeJump").GetCurrentValue();
		floatServe = stats.GetStat("FloatServe").GetCurrentValue();
		if (setter)
		{
			jump = 50f;
		}
		foreach (Technique technique in stats.techniques)
		{
			techniques.Add(technique);
		}
		foreach (Technique technique2 in techniques)
		{
			technique2.OnStart(base.gameObject);
		}
	}

	private void FixedUpdate()
	{
		SetStates();
		BallInteraction();
		SetAnimation();
		SetPhysics();
		MoveHorizontally();
		foreach (Technique technique in techniques)
		{
			technique.timer -= Time.fixedDeltaTime;
		}
	}

	private void Update()
	{
		if (serving)
		{
			ball.Hold(servePoint.position);
		}
	}

	private void DifficultyHitBoxRescale()
	{
		if (GameManager.gameNumber >= 4 && GameManager.gameNumber != 6 && attackDirection == -1f && GameManager.gameNumber != 8)
		{
			setHitBox.GetComponent<BoxCollider2D>().size *= new Vector2(2f, 1f);
		}
	}

	private void SetAnimation()
	{
		animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX) / 10f);
		if (!serving)
		{
			playerSprite.eulerAngles = new Vector3(0f, 0f, (0f - rb.linearVelocity.x) / 2f);
		}
		else
		{
			playerSprite.eulerAngles = Vector3.zero;
		}
	}

	private void BallInteraction()
	{
		setHitBox.SetActive(onGround);
		blockHitBox.enabled = blocking;
	}

	private void MakeBlock(float bonus)
	{
		if (!dontScaleBlock)
		{
			blockHitBox.size = new Vector2(blockHitBox.size.x, (block - 73f + bonus) / 5f + 4f);
		}
		blockHitBox.offset = new Vector2(0f, (block - 73f + bonus) / 9f + 0.3f);
	}

	public bool ShowIndicator()
	{
		if (!actedInAir && !onGround)
		{
			return !blocking;
		}
		return false;
	}

	public float GetSpikePower()
	{
		return spike;
	}

	private void SetStates()
	{
		SetGrounded();
		velocityDirection = rb.linearVelocityX / Mathf.Abs(rb.linearVelocityX);
		landTimer -= Time.deltaTime;
		setTimer -= Time.deltaTime;
		if (inputX != 0f)
		{
			inputDirection = inputX;
		}
	}

	private void SetPhysics()
	{
		SetGravity(rb.linearVelocityY);
		if (onGround)
		{
			ApplyHorizontalDrag(dragAmount);
		}
		else
		{
			ApplyHorizontalDrag(dragAmount * 2f);
		}
	}

	private void MoveHorizontally()
	{
		rb.linearVelocity += new Vector2(moveSpeed * inputX, 0f) * Time.deltaTime * 100f;
		if (onGround && inputX != velocityDirection && !onGround)
		{
			rb.linearVelocity += new Vector2(moveSpeed * inputX, 0f) * Time.deltaTime * 100f;
		}
	}

	private void SetGrounded()
	{
		bool flag = true;
		if (onGround)
		{
			flag = false;
		}
		onGround = Physics2D.BoxCast(jumpCheckPosition.position, new Vector2(1f, 1f), 0f, Vector2.zero, 0f, groundLayer);
		if (onGround && flag)
		{
			actedInAir = false;
			blocking = false;
			animator.ResetTrigger("Jump");
			animator.ResetTrigger("Block");
			animator.ResetTrigger("Spike");
			animator.SetTrigger("Land");
			canServe = false;
		}
		if (hasFootsteps)
		{
			if (onGround && GameManager.Instance.saveData.sfxVolume < 100f)
			{
				Debug.Log(GameManager.Instance.saveData.sfxVolume);
				footSteps.volume = Mathf.Abs(rb.linearVelocityX / 135f);
			}
			else
			{
				footSteps.volume = 0f;
			}
		}
	}

	private void SetGravity(float yVelocity)
	{
		if (blocking)
		{
			if (yVelocity < -2f)
			{
				rb.gravityScale = fallSpeed * 1.5f;
			}
			else if (yVelocity < 3f)
			{
				rb.gravityScale = hangSpeed / 2f;
			}
			else
			{
				rb.gravityScale = riseSpeed * 2f;
			}
		}
		else if (yVelocity < -3f || actedInAir)
		{
			rb.gravityScale = fallSpeed;
		}
		else if (yVelocity < 2f)
		{
			rb.gravityScale = hangSpeed;
		}
		else
		{
			rb.gravityScale = riseSpeed;
		}
	}

	private void ApplyHorizontalDrag(float amount)
	{
		rb.linearVelocity *= new Vector2(1f - Time.deltaTime * amount, 1f);
	}

	public void UpInputPressed()
	{
		if (PauseController.paused)
		{
			return;
		}
		if (onGround)
		{
			if (serving)
			{
				float num = 0f;
				if (rb.linearVelocityX > 10f)
				{
					num = 3f;
				}
				if (Mathf.Abs(base.transform.position.x) > COURT_SIZE - num)
				{
					GameManager.Instance.particles.PlayJumpEffect(base.transform, rb);
					Jump(serveJump);
					animator.ResetTrigger("Land");
					animator.SetTrigger("Serve Jump");
					canServe = true;
					AudioManager.Instance.PlaySFX(soundLibrary.jump, 0.3f);
					if (StatTracker.spikes > 5 && !startedSweating)
					{
						startedSweating = true;
						Object.Instantiate(GameManager.Instance.particles.sweatEffect, base.transform).transform.localPosition = new Vector3(0f, 1f);
					}
				}
				return;
			}
			if ((ball.transform.position.x < 0f && attackDirection == -1f && !BallMovement.freeBall) || (ball.transform.position.x > 0f && attackDirection == 1f && ball.GetServeTimer() < -0.7f && !BallMovement.freeBall))
			{
				blocking = true;
				if (base.transform.position.x < 0f && LimitBreakEffects.limitBlock)
				{
					Object.Instantiate(limitBreakEffects.blockEffect, base.transform.position, Quaternion.identity);
					CameraController.freezeTime(0.01f);
				}
				float num2 = 0f;
				foreach (Technique technique in techniques)
				{
					num2 += technique.OnBlockJump();
				}
				MakeBlock(num2);
				Jump(block + num2);
				animator.ResetTrigger("Land");
				animator.SetTrigger("Block");
				AudioManager.Instance.PlaySFX(soundLibrary.blockJump, 0.4f);
				return;
			}
			if (base.transform.position.x < 0f && LimitBreakEffects.limitJump)
			{
				Object.Instantiate(limitBreakEffects.jumpEffect, base.transform.position, Quaternion.identity);
				limitBreakEffects.LimitEffect();
				CameraController.Shake(0.1f);
				CameraController.freezeTime(0.015f);
			}
			if (landTimer > 0f)
			{
				return;
			}
			float num3 = 0f;
			foreach (Technique technique2 in techniques)
			{
				num3 += technique2.OnJump(base.transform);
			}
			Jump(jump + num3);
			animator.ResetTrigger("Land");
			animator.SetTrigger("Jump");
			AudioManager.Instance.PlaySFX(soundLibrary.jump, 0.3f);
			GameManager.Instance.particles.PlayJumpEffect(base.transform, rb);
		}
		else
		{
			if (serving && canServe)
			{
				AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
				if (!currentAnimatorStateInfo.IsName("Serve Jump") && !currentAnimatorStateInfo.IsName("Hold Ball"))
				{
					return;
				}
				actedInAir = true;
				serving = false;
				ball.Serve(attackDirection, spinServe, rb.linearVelocityY, abilityBooleans);
				animator.ResetTrigger("Serve Jump");
				animator.SetTrigger("Spin Serve");
				{
					foreach (Technique technique3 in techniques)
					{
						technique3.OnServe(ball, base.gameObject.transform);
					}
					return;
				}
			}
			if (!actedInAir && !blocking)
			{
				actedInAir = true;
				StartCoroutine(HitBoxActive(spikeHitBox, 0.2f, 0.05f));
				AudioManager.Instance.PlaySFX(soundLibrary.swing, 0.6f);
				animator.SetTrigger("Spike");
			}
		}
	}

	public void DownInputPressed()
	{
		if (PauseController.paused || onGround)
		{
			return;
		}
		if (serving)
		{
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (!currentAnimatorStateInfo.IsName("Serve Jump") && !currentAnimatorStateInfo.IsName("Hold Ball"))
			{
				return;
			}
			actedInAir = true;
			ball.FloatServe(attackDirection, floatServe, rb.linearVelocityY);
			animator.ResetTrigger("Serve Jump");
			animator.SetTrigger("Float Serve");
			serving = false;
			{
				foreach (Technique technique in techniques)
				{
					technique.OnServe(ball, base.gameObject.transform);
				}
				return;
			}
		}
		if (!actedInAir && !blocking)
		{
			actedInAir = true;
			StartCoroutine(HitBoxActive(tipHitBox, 0.2f, 0.05f));
			AudioManager.Instance.PlaySFX(soundLibrary.tip, 0.2f);
			animator.SetTrigger("Tip");
		}
	}

	public bool getGrounded()
	{
		return onGround;
	}

	private IEnumerator HitBoxActive(GameObject hitBox, float time, float delay)
	{
		yield return new WaitForSeconds(delay);
		hitBox.SetActive(value: true);
		yield return new WaitForSeconds(time);
		hitBox.SetActive(value: false);
	}

	public void SetXInput(float input)
	{
		inputX = input;
	}

	private void Jump(float speed)
	{
		animator.ResetTrigger("Land");
		animator.ResetTrigger("Spike");
		animator.ResetTrigger("Tip");
		animator.ResetTrigger("Serve Jump");
		rb.linearVelocity = new Vector3(rb.linearVelocityX, speed, 0f);
	}

	public void DoSet()
	{
		if (serving || GameManager.Instance.done)
		{
			return;
		}
		landTimer = 0.1f;
		float num = ball.Set(attackDirection, Mathf.Abs(base.transform.position.x), ball.gameObject.GetComponent<Rigidbody2D>().linearVelocity, bump, abilityBooleans);
		if (num == -1000f)
		{
			return;
		}
		rb.linearVelocity += new Vector2(num, 0f);
		if (num == 0f)
		{
			animator.SetTrigger("Set");
		}
		else
		{
			animator.SetTrigger("Bump");
		}
		setTimer = 0.4f;
		foreach (Technique technique in techniques)
		{
			technique.OnBump(ball, base.transform);
		}
	}

	public void DoSpike(Vector3 position)
	{
		float num = 0f;
		foreach (Technique technique in techniques)
		{
			num += technique.OnSpike(ball, rb, base.transform);
		}
		ball.Spike(position, spike + num, 0f - attackDirection, abilityBooleans, rb, velocityAim);
	}

	public void DoTip()
	{
		foreach (Technique technique in techniques)
		{
			technique.OnTip(ball);
		}
		ball.tip(attackDirection);
	}

	public void StartServe()
	{
		serving = true;
		animator.SetTrigger("Hold Ball");
	}

	public bool IsServing()
	{
		return serving;
	}

	public float GetJumpPower()
	{
		return jump;
	}
}
