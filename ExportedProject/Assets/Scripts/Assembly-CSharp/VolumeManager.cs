using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
	public static float masterVolume;

	public float setMusicVolume;

	public float setSoundVolume;

	public Slider soundSlider;

	public Slider musicSlider;

	private void Start()
	{
		setSoundVolume = GameManager.Instance.saveData.sfxSliderPosition;
		soundSlider.value = GameManager.Instance.saveData.sfxSliderPosition;
		musicSlider.value = GameManager.Instance.saveData.musicVolume;
		setMusicVolume = GameManager.Instance.saveData.musicVolume;
		soundSlider.onValueChanged.AddListener(ChangeSound);
		musicSlider.onValueChanged.AddListener(ChangeMusic);
	}

	private void Update()
	{
		GameManager.Instance.saveData.sfxVolume = SoundVolToLinear(setSoundVolume, 0f, 1f, 4f, -200f, 6f, 2f);
		if (PauseController.paused || GameManager.Instance.recordingMode)
		{
			GameManager.Instance.saveData.musicVolume = 0f;
		}
		else
		{
			GameManager.Instance.saveData.musicVolume = setMusicVolume;
		}
		GameManager.Instance.saveData.sfxSliderPosition = setSoundVolume;
	}

	private void ChangeSound(float v)
	{
		setSoundVolume = v;
	}

	public void ChangeMusic(float v)
	{
		setMusicVolume = v;
	}

	public float SoundVolToLinear(float v, float vMin = 0f, float vPivot = 1f, float vMax = 4f, float minDb = -96f, float boostDb = 6f, float gammaDown = 1.6f, float gammaUp = 1f)
	{
		if (v <= 0f)
		{
			return 100f;
		}
		v = Mathf.Clamp(v, vMin, vMax);
		if (v >= vPivot)
		{
			float f = (v - vPivot) / (vMax - vPivot);
			f = Mathf.Pow(f, gammaDown);
			float num = Mathf.Lerp(0f, minDb, f);
			return Mathf.Pow(10f, num / 20f);
		}
		float f2 = (vPivot - v) / (vPivot - vMin);
		f2 = Mathf.Pow(f2, gammaUp);
		float num2 = Mathf.Lerp(0f, boostDb, f2);
		return Mathf.Pow(10f, num2 / 20f);
	}
}
