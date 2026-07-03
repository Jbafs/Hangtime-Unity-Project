using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
	public List<AudioClip> clips;

	private AudioSource audioSource;

	[SerializeField]
	private bool playSound;

	public void Start()
	{
		audioSource = base.gameObject.GetComponent<AudioSource>();
		audioSource.clip = clips[Random.Range(0, clips.Capacity)];
		if (playSound)
		{
			PlaySound(pitchShift: false);
		}
	}

	public void PlaySound(bool pitchShift = true)
	{
		audioSource.volume = 0.42f / GameManager.Instance.saveData.sfxVolume;
		if (pitchShift)
		{
			audioSource.pitch = Random.Range(0.96f, 1.04f);
		}
		audioSource.Play();
	}
}
