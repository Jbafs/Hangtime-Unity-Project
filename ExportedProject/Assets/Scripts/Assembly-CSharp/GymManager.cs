using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class GymManager : MonoBehaviour
{
	[SerializeField]
	private GameObject[] gyms;

	private void OnEnable()
	{
		StartCoroutine(delayedActivation());
	}

	private IEnumerator delayedActivation()
	{
		yield return null;
		ClearChildren();
		GameObject gameObject = ((!TitleController.onTitle) ? Object.Instantiate(gyms[GameManager.gameNumber], base.transform) : Object.Instantiate(gyms[Random.Range(0, gyms.Length)], base.transform));
		AudioSource music = gameObject.GetComponent<AudioSource>();
		yield return null;
		if (!TitleController.onTitle)
		{
			GameManager.SetMusic(music.clip, music.volume);
		}
	}

	private void ClearChildren()
	{
		foreach (Transform item in base.transform)
		{
			Object.DestroyImmediate(item.gameObject);
		}
	}
}
