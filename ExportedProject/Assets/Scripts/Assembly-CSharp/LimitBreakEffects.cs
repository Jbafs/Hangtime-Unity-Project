using UnityEngine;

public class LimitBreakEffects : MonoBehaviour
{
	public static bool limitSpike;

	public static bool limitBump;

	public static bool limitJump;

	public static bool limitSpinServe;

	public static bool limitFloatServe;

	public static bool limitBlock;

	[SerializeField]
	private Animation limitDarkness;

	[SerializeField]
	private SoundLibrary soundLibrary;

	public GameObject spikeEffect;

	public GameObject bumpEffect;

	public GameObject jumpEffect;

	public GameObject spinServeEffect;

	public GameObject floatServeEffect;

	public GameObject blockEffect;

	public static void ResetLimits()
	{
		limitSpike = false;
		limitBump = false;
		limitJump = false;
		limitSpinServe = false;
		limitFloatServe = false;
		limitBlock = false;
	}

	public void LimitEffect()
	{
		limitDarkness.Play();
		AudioManager.Instance.PlaySFX(soundLibrary.limitBreak, 0.35f);
	}
}
