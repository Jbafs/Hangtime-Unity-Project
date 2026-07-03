using UnityEngine;

public class FanController : MonoBehaviour
{
	public float cheerAmount;

	public float cheerConstant;

	[SerializeField]
	private float cheerDecay;

	[SerializeField]
	private GameObject[] crowd;

	[SerializeField]
	private AudioSource cheerSound;

	private void Start()
	{
		GameManager.Instance.fanController = this;
		int num = 0;
		GameObject[] array = crowd;
		foreach (GameObject gameObject in array)
		{
			if (num > GameManager.gameNumber)
			{
				gameObject.SetActive(value: false);
			}
			num++;
		}
	}

	private void Update()
	{
		if (cheerConstant == 0f)
		{
			cheerAmount *= 1f - Time.deltaTime * cheerDecay;
		}
		else
		{
			cheerAmount = cheerConstant;
		}
		cheerSound.volume = Mathf.Lerp(cheerSound.volume, cheerAmount / 15f, Time.deltaTime * 3f);
	}

	public void setCheer(float amount)
	{
		cheerAmount += amount;
	}

	public void stopCheer()
	{
		cheerAmount = 0f;
	}
}
