using System.Collections;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
	public static bool jumpForSpike;

	public static bool jumpForHighSpike;

	public static bool jumpForQuick;

	public static bool serving;

	public static bool freeBall;

	public static float bonusGravity;

	private bool giveAce;

	public static float positionVariation;

	[SerializeField]
	private float variationChange;

	private Rigidbody2D rb;

	private float originalGravity;

	private Vector3 originalScale;

	private float serveTimer;

	private float blockTimer;

	private int perfectRecieve;

	private float COURT_SIZE = 34f;

	private float ballSide;

	public static int lastTouched;

	public static int touchCounter;

	public static bool rallyDone;

	private bool gameOver;

	private AudioSource jetSound;

	public static int sideServing;

	public static float predictedPosition;

	public static float precisePredictedPosition;

	private float squashTime;

	private bool serveBall;

	private Coroutine floatRoutine;

	[Header("Ai")]
	[SerializeField]
	private Transform predictedTool;

	[SerializeField]
	private OpponentDialogue opponentDialogue;

	[Header("Servers")]
	[SerializeField]
	private PlayerController player;

	[SerializeField]
	private PlayerController CoopTeammate;

	public PlayerController opponent;

	private bool cantInteract;

	private bool switchServe;

	[Header("Stats")]
	[SerializeField]
	private float setSpeed;

	[SerializeField]
	private float originalSPikePower;

	[SerializeField]
	private float distanceChange;

	public Vector2 tipVelocity;

	[Header("Glyphs")]
	public GameObject ServeGlyph;

	public GameObject BlockGlyph;

	public GameObject BumpGlyph;

	[Header("Effects")]
	[SerializeField]
	private LimitBreakEffects limitBreakEffects;

	[SerializeField]
	private Animation screenFlash;

	[SerializeField]
	private ParticleSystem bigEffect;

	[SerializeField]
	private ParticleSystem hitEffect;

	[SerializeField]
	private ParticleSystem bumpEffect;

	[SerializeField]
	private ParticleSystem pointEffect;

	[SerializeField]
	private ParticleSystem bigPointEffect;

	[SerializeField]
	private ParticleSystem outPointEffect;

	[SerializeField]
	private ParticleSystem tipEffect;

	[SerializeField]
	private TrailRenderer myTrail;

	[SerializeField]
	private ParticleSystem spinServeEffect;

	[SerializeField]
	private ParticleSystem floatServeEffect;

	[SerializeField]
	private SpriteRenderer squashSprite;

	[SerializeField]
	private SpriteRenderer ballSprite;

	[SerializeField]
	private Color normalColor;

	[SerializeField]
	private Color backColor;

	[SerializeField]
	private ParticleSystem thirdTouch;

	private bool limitServe;

	[Header("Modifiers")]
	public BallModifier activeModifier;

	private float modifierTime;

	[SerializeField]
	private BallModifier floatModifier;

	[Header("Audio")]
	[SerializeField]
	private SoundLibrary soundLibrary;

	[SerializeField]
	private RandomSound humanSounds;

	private void Start()
	{
		bonusGravity = 0f;
		rallyDone = false;
		originalScale = base.transform.localScale;
		rb = base.gameObject.GetComponent<Rigidbody2D>();
		originalGravity = rb.gravityScale;
		jetSound = base.gameObject.GetComponent<AudioSource>();
		jumpForSpike = false;
		jumpForHighSpike = false;
		jumpForQuick = false;
		freeBall = false;
		serving = false;
		StartCoroutine(setPredictedPosition());
		GameManager.Instance.OnStart(base.gameObject);
		if (GameManager.Instance.opponentTeam != null)
		{
			StartCoroutine(opponentDialogue.DelayedDialogue(GameManager.Instance.opponentTeam.introDialogue, 0.5f, GameManager.Instance.opponentTeam.talkPitch));
		}
		if (!TutorialManager.onTutorial || TitleController.onTitle)
		{
			player.StartServe();
		}
	}

	private IEnumerator setPredictedPosition()
	{
		yield return new WaitForSeconds(0.1f);
		predictedPosition = base.transform.position.x + rb.linearVelocityX * (base.transform.position.y + 8f) / (float)Random.Range(50, 80);
		precisePredictedPosition = base.transform.position.x + rb.linearVelocityX * ((base.transform.position.y + 8f) * (1f + rb.linearVelocityY / 60f)) / 20f;
		predictedTool.position = new Vector2(precisePredictedPosition, predictedTool.position.y);
		StartCoroutine(setPredictedPosition());
	}

	private void FixedUpdate()
	{
		float magnitude = rb.linearVelocity.magnitude;
		SetGravity(magnitude);
		Resize(magnitude);
		AiInteraction();
		UpdateTimers();
		SetSound();
		SetCollider(rb.linearVelocityY);
		if (serveTimer < 0f)
		{
			myTrail.enabled = true;
		}
		base.transform.up = rb.linearVelocity;
		ballSide = base.transform.position.x / Mathf.Abs(base.transform.position.x);
	}

	private void SetCollider(float yVelocity)
	{
		if ((rb.linearVelocityY > 0f && touchCounter >= 3 && freeBall) || serving)
		{
			base.gameObject.layer = 2;
		}
		else
		{
			base.gameObject.layer = 7;
		}
	}

	private void Update()
	{
		if (activeModifier != null)
		{
			modifierTime -= Time.deltaTime;
			if (modifierTime <= 0f)
			{
				activeModifier.Remove(rb);
				activeModifier = null;
			}
		}
	}

	private void SetSound()
	{
		if (rb.linearVelocity.magnitude > 70f)
		{
			jetSound.volume = rb.linearVelocity.magnitude / 200f / GameManager.Instance.saveData.sfxVolume;
		}
		else
		{
			jetSound.volume = 0f;
		}
	}

	private void UpdateTimers()
	{
		serveTimer -= Time.deltaTime;
		serving = serveTimer > 0f;
		blockTimer -= Time.deltaTime;
		squashTime -= Time.deltaTime;
		if (cantInteract && ((lastTouched == -1 && base.transform.position.x > 0f) || (lastTouched == 1 && base.transform.position.x < 0f)))
		{
			cantInteract = false;
		}
	}

	public float GetServeTimer()
	{
		return serveTimer;
	}

	public void ApplyModifier(BallModifier modifier)
	{
		if (activeModifier != null)
		{
			activeModifier.Remove(rb);
		}
		activeModifier = modifier;
		modifierTime = modifier.duration;
		modifier.Apply(rb);
	}

	private void Resize(float speed)
	{
		Vector3 vector = originalScale;
		if (squashTime > 0f)
		{
			squashSprite.enabled = true;
			ballSprite.enabled = false;
		}
		else
		{
			squashSprite.enabled = false;
			ballSprite.enabled = true;
		}
		vector = ((!(speed > 50f)) ? originalScale : (originalScale + new Vector3(0f - speed, speed) / 120f));
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, vector, Time.deltaTime * 5f);
	}

	private void AiInteraction()
	{
		bool num = rb.linearVelocity.y > -3f && rb.linearVelocity.y < 6f && base.transform.position.y > 10f && base.transform.position.y < 19f;
		bool flag = rb.linearVelocity.magnitude < 60f;
		bool flag2 = base.transform.position.x > -14f && base.transform.position.x < 18f;
		jumpForSpike = num && flag && flag2;
		jumpForHighSpike = rb.linearVelocity.y > 0f && rb.linearVelocityY < 15f && base.transform.position.y > 9f && flag && flag2;
		jumpForQuick = rb.linearVelocity.y > 55f && Mathf.Abs(rb.linearVelocityX) < 10f && base.transform.position.x < 14f;
	}

	private void SetGravity(float speed)
	{
		if (speed > 30f)
		{
			rb.gravityScale = originalGravity + speed / 20f + bonusGravity;
		}
		else
		{
			rb.gravityScale = originalGravity + bonusGravity;
		}
	}

	private void setTouchCounter(float direction)
	{
		if ((float)lastTouched == direction)
		{
			touchCounter = 0;
		}
		touchCounter++;
	}

	public float Set(float direction, float distanceFromNet, Vector2 hitPower, float bumpPower, AbilityBooleans abilityBooleans)
	{
		freeBall = false;
		giveAce = false;
		if (serveTimer > 0f)
		{
			return -1000f;
		}
		if (base.transform.position.x > 0f)
		{
			if (direction == 1f)
			{
				return 0f;
			}
		}
		else if (base.transform.position.x < 0f && direction == -1f)
		{
			return 0f;
		}
		if (serveBall && Random.Range(1, 4) == 1 && limitServe)
		{
			CameraController.freezeTime(0.005f);
			lastTouched = (int)(0f - direction);
			limitServe = false;
		}
		setTouchCounter(direction);
		if (blockTimer > 0f || rallyDone || (cantInteract && direction == (float)(-lastTouched)) || (touchCounter > 3 && !TutorialManager.onTutorial) || (serveBall && (float)lastTouched != direction))
		{
			return 0f;
		}
		lastTouched = (int)(0f - direction);
		if (activeModifier != null && touchCounter < 3 && touchCounter < 3)
		{
			activeModifier.OnBump(rb);
		}
		serveBall = false;
		if (touchCounter >= 3 && !TutorialManager.onTutorial)
		{
			rb.linearVelocity = new Vector2(direction * 16f + (0f - base.transform.position.x), setSpeed / 1.3f);
			if (distanceFromNet < 11f)
			{
				rb.linearVelocity += new Vector2(0f, 15f - distanceFromNet);
			}
			freeBall = true;
			thirdTouch.Play();
		}
		else if (distanceFromNet < 14f)
		{
			rb.linearVelocity = new Vector2(0f, setSpeed);
		}
		else if (distanceFromNet < 22f)
		{
			rb.linearVelocity = new Vector2(direction * 9f, setSpeed);
		}
		else if (distanceFromNet < 28f)
		{
			rb.linearVelocity = new Vector2(direction * 15f, setSpeed);
		}
		else if (distanceFromNet < 35f)
		{
			rb.linearVelocity = new Vector2(direction * 22f, setSpeed);
		}
		else if (distanceFromNet < 60f && abilityBooleans.riskySet)
		{
			rb.linearVelocity = new Vector2(direction * 80f, setSpeed / 1.1f);
		}
		else if (distanceFromNet < 42f)
		{
			rb.linearVelocity = new Vector2(direction * 29f, setSpeed + 2f);
		}
		else
		{
			rb.linearVelocity = new Vector2(direction * 60f, setSpeed);
			freeBall = true;
		}
		if (hitPower.magnitude > 65f)
		{
			float x = Random.Range(hitPower.magnitude / 3f, hitPower.magnitude) * (0f - direction) / bumpPower;
			float y = -13f + 3f * bumpPower;
			rb.linearVelocity += new Vector2(x, y);
			bumpEffect.Play();
			perfectRecieve++;
			CameraController.Rotate((0f - direction) * 0.75f);
			CameraController.zoom(4f);
			if ((Random.Range(1, 6) == 1 && perfectRecieve > 2) || (Random.Range(1, 4) == 1 && abilityBooleans.perfectBump))
			{
				humanSounds.PlaySound();
				bigEffect.Play();
				perfectRecieve = 0;
				rb.linearVelocity -= new Vector2(x, y);
				CameraController.freezeTime(0.02f);
				GameManager.Instance.fanController.setCheer(4f);
				AudioManager.Instance.PlaySFX(soundLibrary.perfectBump, 0.35f);
				if (base.transform.position.x < 0f)
				{
					StatTracker.perfectRecieves++;
				}
				if (Random.Range(1, 3) == 1)
				{
					Object.Instantiate(BumpGlyph, base.transform.position, Quaternion.identity);
				}
			}
			AudioManager.Instance.PlaySFX(soundLibrary.bump, 0.3f);
			if (LimitBreakEffects.limitBump && base.transform.position.x < 0f)
			{
				Object.Instantiate(limitBreakEffects.bumpEffect, base.transform.position, Quaternion.identity);
				CameraController.zoom(0.5f);
				limitBreakEffects.LimitEffect();
				CameraController.freezeTime(0.008f);
			}
			if (bumpPower > 10f)
			{
				floatModifier.Remove(rb);
				if (floatRoutine != null)
				{
					StopCoroutine(floatRoutine);
				}
			}
			return hitPower.x;
		}
		AudioManager.Instance.PlaySFX(soundLibrary.set, 0.35f);
		return 0f;
	}

	public void Spike(Vector3 playerPosition, float power, float direction, AbilityBooleans abilitiyBooleans, Rigidbody2D playerRb, bool velocityAim = false)
	{
		if (touchCounter > 2 && !TutorialManager.onTutorial)
		{
			if ((!freeBall || direction == (float)lastTouched) && (!freeBall || !abilitiyBooleans.canSpikeFreeBalls))
			{
				return;
			}
		}
		else if ((freeBall && direction == (float)lastTouched && !abilitiyBooleans.canSpikeFreeBalls) || serveBall)
		{
			return;
		}
		if (cantInteract)
		{
			return;
		}
		setTouchCounter(0f - direction);
		squashTime = 0.002f;
		humanSounds.PlaySound();
		AudioManager.Instance.PlaySFX(soundLibrary.spike, 0.4f);
		CameraController.Shake(0.24f);
		CameraController.freezeTime(0.007f);
		GameManager.Instance.fanController.setCheer(1f);
		lastTouched = (int)direction;
		if (direction == -1f)
		{
			StatTracker.spikes++;
			if (LimitBreakEffects.limitSpike)
			{
				Object.Instantiate(limitBreakEffects.spikeEffect, base.transform.position, Quaternion.identity);
				limitBreakEffects.LimitEffect();
				CameraController.Shake(0.6f);
				CameraController.freezeTime(0.011f);
			}
			if (rb.linearVelocityY > 0f)
			{
				AchievementManager.I.Unlock(AchievementManager.Id.QuickAttack);
			}
		}
		if (velocityAim)
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, 120f * direction);
			float num = 1f - (power - 1f) / 2f;
			base.transform.Rotate(0f, 0f, num * playerRb.linearVelocityX * direction * -1f);
		}
		else
		{
			float num2 = Mathf.Abs(base.transform.position.x - playerPosition.x);
			base.transform.eulerAngles = new Vector3(0f, 0f, 90f * direction);
			base.transform.Rotate(0f, 0f, num2 * distanceChange * direction);
			base.transform.Rotate(0f, 0f, (power - 1f) * 6f * direction);
		}
		rb.linearVelocity = base.transform.up * power * originalSPikePower;
		hitEffect.Play();
	}

	public void tip(float direction)
	{
		if (touchCounter > 2 && !TutorialManager.onTutorial)
		{
			if (!freeBall || 0f - direction == (float)lastTouched)
			{
				return;
			}
		}
		else if ((freeBall && 0f - direction == (float)lastTouched) || serveBall)
		{
			return;
		}
		if (!cantInteract)
		{
			setTouchCounter(direction);
			lastTouched = (int)direction * -1;
			tipEffect.Play();
			rb.linearVelocity = new Vector2(tipVelocity.x * direction, tipVelocity.y);
			if (direction == 1f)
			{
				StatTracker.tips++;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		floatModifier.Remove(rb);
		if (collision.gameObject.tag == "Ground" && !rallyDone)
		{
			StartCoroutine(GetPoint((0f - base.transform.position.x) / Mathf.Abs(base.transform.position.x)));
		}
		if (collision.gameObject.tag == "Player")
		{
			Block(collision);
		}
		if (collision.gameObject.tag == "Untagged")
		{
			AudioManager.Instance.PlaySFX(soundLibrary.net, 0.5f);
		}
	}

	private void PlayCheer()
	{
	}

	private void Block(Collision2D collision)
	{
		CameraController.freezeTime(0.01f);
		if (rb.linearVelocity.magnitude > 50f || collision.gameObject.GetComponent<AbilityBooleans>().absoluteBlock)
		{
			bigEffect.Play();
			AudioManager.Instance.PlaySFX(soundLibrary.block, 0.4f);
			hitEffect.Play();
			blockTimer = 0.3f;
			if (collision.transform.position.x < 0f)
			{
				StatTracker.blocks++;
				GameManager.Instance.saveData.numberOfBlocks += 1f;
				Debug.Log("Block #" + GameManager.Instance.saveData.numberOfBlocks);
				if (LimitBreakEffects.limitBlock)
				{
					Object.Instantiate(limitBreakEffects.blockEffect, base.transform.position, Quaternion.identity);
					CameraController.Shake(0.4f);
					limitBreakEffects.LimitEffect();
				}
				if (GameManager.Instance.saveData.numberOfBlocks >= 200f)
				{
					AchievementManager.I.Unlock(AchievementManager.Id.FiveBlocks);
				}
			}
			if (Random.Range(1, 6) == 1)
			{
				Object.Instantiate(BlockGlyph, base.transform.position, Quaternion.identity);
			}
			if (collision.gameObject.GetComponent<AbilityBooleans>().absoluteBlock)
			{
				rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY - 45f);
				CameraController.freezeTime(0.015f);
			}
		}
		else
		{
			AudioManager.Instance.PlaySFX(soundLibrary.tool, 0.3f);
			tipEffect.Play();
		}
		touchCounter = 0;
		if (collision.gameObject.transform.position.x > 0f)
		{
			lastTouched = 1;
		}
		else
		{
			lastTouched = -1;
		}
		if (activeModifier != null)
		{
			activeModifier.OnBlock(rb);
		}
	}

	public void Hold(Vector3 position)
	{
		base.transform.position = position;
		rb.linearVelocity *= 0f;
		serveTimer = 0.03f;
		myTrail.enabled = false;
		base.transform.localScale = originalScale;
		giveAce = true;
		cantInteract = true;
	}

	public IEnumerator GetPoint(float direction)
	{
		if (base.transform.position.x < 10f && Random.Range(1, 5) != 1)
		{
			positionVariation = 0f - variationChange;
		}
		else if (base.transform.position.x > 23f && Random.Range(1, 5) != 1)
		{
			positionVariation = variationChange / 2f;
		}
		else
		{
			positionVariation = Random.Range(0f - variationChange, variationChange);
		}
		CameraController.Shake(0.2f);
		GameManager.Instance.fanController.setCheer(3f);
		CameraController.Rotate(0f - direction);
		modifierTime = 0f;
		rallyDone = true;
		PlayCheer();
		PlayerController nextServer;
		if (Mathf.Abs(base.transform.position.x) < COURT_SIZE)
		{
			if (rb.linearVelocity.magnitude > 70f)
			{
				KillEffect(bigPointEffect, 0.03f, 15f, 0.3f);
				GameManager.Instance.fanController.setCheer(2f);
				CameraController.Rotate((0f - direction) * 1.5f);
				CameraController.Shake(0.5f);
				if (touchCounter == 0)
				{
					GameManager.Instance.fanController.setCheer(2f);
				}
			}
			else
			{
				KillEffect(pointEffect, 0.01f, 10f, 0.2f);
			}
			nextServer = ((ballSide != 1f) ? opponentGetPoint() : playerGetPoint());
			if (giveAce)
			{
				if (base.transform.position.x > 0f)
				{
					StatTracker.aces++;
				}
				CameraController.Rotate((0f - direction) * 2f);
				AudioManager.Instance.PlaySFX(soundLibrary.ace, 0.1f);
				if (Random.Range(1, 5) == 1)
				{
					Object.Instantiate(ServeGlyph, base.transform.position, Quaternion.identity);
				}
				if (StatTracker.aces == 5)
				{
					AchievementManager.I.Unlock(AchievementManager.Id.ThreeAcesMatch);
				}
			}
			AudioManager.Instance.PlaySFX(soundLibrary.pointIn, 0.2f);
			yield return null;
		}
		else
		{
			GameManager.Instance.fanController.setCheer(-3f);
			KillEffect(outPointEffect, 0.03f, 5f, 0.1f);
			nextServer = ((lastTouched != 1) ? opponentGetPoint() : playerGetPoint());
			AudioManager.Instance.PlaySFX(soundLibrary.pointOut);
			if (Mathf.Abs(base.transform.position.x) > 68f)
			{
				AudioManager.Instance.PlaySFX(soundLibrary.windowSmash, 0.25f);
			}
		}
		yield return null;
		if (gameOver)
		{
			if (PlayerBlocked())
			{
				AchievementManager.I.Unlock(AchievementManager.Id.WinWithBlock);
			}
			screenFlash.Play();
			AudioManager.Instance.PlaySFX(soundLibrary.perfectBump, 0.2f);
			CameraController.freezeTime(0.1f);
			if (GameManager.gameNumber != 6)
			{
				_ = nextServer != player;
			}
			CameraController.Shake(0.9f);
			GameManager.Instance.fanController.stopCheer();
			CameraController.zoom(-30f);
			AudioManager.Instance.PlaySFX(soundLibrary.applause, 0.2f);
			if (nextServer == player)
			{
				opponentDialogue.PlayDialogue(GameManager.Instance.opponentTeam.loseDialogue, GameManager.Instance.opponentTeam.talkPitch);
			}
			else
			{
				opponentDialogue.PlayDialogue(GameManager.Instance.opponentTeam.winDialogue, GameManager.Instance.opponentTeam.talkPitch);
			}
			yield return new WaitForSeconds(0.2f);
			CameraController.zoom(10f);
			yield return new WaitForSeconds(0.05f);
			GameManager.Instance.fanController.setCheer(7f);
			for (float i = 0f; i < 4f; i += Time.deltaTime)
			{
				yield return null;
				CameraController.Shake(0.15f);
				GameManager.Instance.fanController.setCheer(Time.deltaTime * 2f);
			}
			yield return new WaitForSeconds(2f);
			yield break;
		}
		base.transform.localScale = originalScale;
		if (TutorialManager.onTutorial)
		{
			if (TutorialManager.tutorialPart == 2)
			{
				playerGetPoint().StartServe();
			}
			else
			{
				myTrail.enabled = false;
				yield return null;
				base.transform.position = TutorialManager.ballReset;
				rb.linearVelocity = new Vector2(1f, 1f);
				yield return null;
				myTrail.enabled = true;
			}
		}
		else
		{
			nextServer.StartServe();
		}
		if (nextServer == player)
		{
			sideServing = -1;
		}
		else
		{
			sideServing = 1;
		}
		yield return null;
		rallyDone = false;
	}

	private void KillEffect(ParticleSystem effect, float freezeTime, float zoom, float shake)
	{
		effect.Play();
		CameraController.freezeTime(freezeTime);
		CameraController.zoom(zoom);
		CameraController.Shake(shake);
	}

	private bool PlayerBlocked()
	{
		if (blockTimer > -0.1f)
		{
			return base.transform.position.x > 0f;
		}
		return false;
	}

	private PlayerController playerGetPoint()
	{
		gameOver = GameManager.Instance.PlayerGotPoint();
		opponentDialogue.TryDialogue(GameManager.Instance.opponentTeam.lostPointDialogue, GameManager.Instance.opponentTeam.talkPitch);
		if (GameManager.Instance.numberOfPlayers == 2)
		{
			if (switchServe)
			{
				switchServe = false;
				return player;
			}
			switchServe = true;
			return CoopTeammate;
		}
		return player;
	}

	private PlayerController opponentGetPoint()
	{
		gameOver = GameManager.Instance.OpponentGotPoint();
		opponentDialogue.TryDialogue(GameManager.Instance.opponentTeam.gotPointDialogue, GameManager.Instance.opponentTeam.talkPitch);
		return opponent;
	}

	public void Serve(float direction, float power, float playerYVelocity, AbilityBooleans abilityBooleans)
	{
		serveBall = true;
		lastTouched = (int)(0f - direction);
		touchCounter = 0;
		squashTime = 0.0015f;
		humanSounds.PlaySound();
		base.transform.eulerAngles = new Vector3(0f, 0f, (0f - direction) * 90f);
		rb.linearVelocity = base.transform.up * power;
		rb.linearVelocity += new Vector2((0f - playerYVelocity) * direction, playerYVelocity);
		spinServeEffect.Play();
		CameraController.Shake(0.3f);
		if (LimitBreakEffects.limitSpinServe && base.transform.position.x < 0f)
		{
			Object.Instantiate(limitBreakEffects.spinServeEffect, base.transform.position, Quaternion.identity);
			limitBreakEffects.LimitEffect();
			CameraController.Shake(0.4f);
			CameraController.freezeTime(0.025f);
			limitServe = true;
		}
		if (rb.linearVelocityY > 0f && abilityBooleans.skyServe)
		{
			rb.linearVelocity *= new Vector2(0.7f, 4f);
		}
		AudioManager.Instance.PlaySFX(soundLibrary.spinServe, 0.25f);
		if (abilityBooleans.hybridServe)
		{
			floatRoutine = StartCoroutine(doFloat(250f, hybrid: true));
		}
	}

	public void FloatServe(float direction, float power, float yVlocity)
	{
		serveBall = true;
		lastTouched = (int)(0f - direction);
		touchCounter = 0;
		base.transform.eulerAngles = new Vector3(0f, 0f, 90f * (0f - direction));
		if (LimitBreakEffects.limitFloatServe && base.transform.position.x < 0f)
		{
			limitServe = true;
			Object.Instantiate(limitBreakEffects.floatServeEffect, base.transform.position, Quaternion.identity);
			limitBreakEffects.LimitEffect();
			CameraController.Shake(0.2f);
			CameraController.freezeTime(0.02f);
		}
		if (power < 115f)
		{
			rb.linearVelocity = 40f * base.transform.up;
		}
		else if (power < 135f)
		{
			rb.linearVelocity = 45f * base.transform.up;
		}
		else
		{
			rb.linearVelocity = 55f * base.transform.up;
		}
		rb.linearVelocity += new Vector2(0f, yVlocity / 5f);
		ApplyModifier(floatModifier);
		floatRoutine = StartCoroutine(doFloat(power, hybrid: false));
		floatServeEffect.Play();
		AudioManager.Instance.PlaySFX(soundLibrary.floatServe, 0.4f);
	}

	private IEnumerator doFloat(float power, bool hybrid)
	{
		while (Mathf.Abs(base.transform.position.x) > 1f)
		{
			yield return null;
		}
		for (int i = 0; i < 2; i++)
		{
			float wobble = Random.Range(-1, 2);
			if (i == 1)
			{
				wobble = -1f;
			}
			if (hybrid && wobble == 1f)
			{
				wobble = 0f;
			}
			for (float j = 0f; j < 0.2f; j += Time.deltaTime)
			{
				if (wobble == 1f)
				{
					rb.linearVelocity += new Vector2(0f, power / 1.5f * wobble) * Time.deltaTime;
				}
				else
				{
					rb.linearVelocity += new Vector2(0f, power * wobble) * Time.deltaTime;
				}
				yield return null;
			}
		}
	}
}
