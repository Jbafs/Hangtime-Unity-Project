using UnityEngine;

public class EffectManager : MonoBehaviour
{
	[SerializeField]
	private GameObject spotLights;

	[SerializeField]
	private AudioSource cymballSwell;

	private void Start()
	{
		if (GameManager.gameNumber == 3 || GameManager.gameNumber >= 6)
		{
			spotLights.SetActive(value: true);
		}
		AudioManager.Instance.PlaySFX(cymballSwell.clip, 0.1f);
	}
}
