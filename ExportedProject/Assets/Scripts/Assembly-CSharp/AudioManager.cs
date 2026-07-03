using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	[SerializeField]
	private int poolSize = 10;

	[SerializeField]
	private AudioSource sfxSourcePrefab;

	private List<AudioSource> sfxPool;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		InitializePool();
	}

	private void InitializePool()
	{
		sfxPool = new List<AudioSource>();
		for (int i = 0; i < poolSize; i++)
		{
			AudioSource audioSource = Object.Instantiate(sfxSourcePrefab, base.transform);
			audioSource.playOnAwake = false;
			sfxPool.Add(audioSource);
		}
	}

	public void PlaySFX(AudioClip clip, float volume = 1f, float pitchChange = 0.05f, float pitch = 1f)
	{
		AudioSource availableSource = GetAvailableSource();
		if (availableSource != null)
		{
			availableSource.clip = clip;
			availableSource.volume = volume / GameManager.Instance.saveData.sfxVolume;
			availableSource.pitch = Random.Range(0f - pitchChange, pitchChange) + pitch;
			availableSource.Play();
		}
	}

	private AudioSource GetAvailableSource()
	{
		foreach (AudioSource item in sfxPool)
		{
			if (!item.isPlaying)
			{
				return item;
			}
		}
		return null;
	}
}
