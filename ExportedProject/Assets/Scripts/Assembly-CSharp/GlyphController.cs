using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlyphController : MonoBehaviour
{
	[SerializeField]
	private float lifeTime;

	[SerializeField]
	private List<string> glyphs;

	private TextMeshPro text;

	private float originalScale;

	private AudioSource sound;

	private void Start()
	{
		sound = base.gameObject.GetComponent<AudioSource>();
		CameraController.Shake(0.7f);
		CameraController.freezeTime(0.02f);
		originalScale = base.transform.localScale.x;
		base.transform.localScale = new Vector3(0f, 0f, 0f);
		text = base.gameObject.GetComponent<TextMeshPro>();
		text.sortingOrder = -2;
		text.text = glyphs[Random.Range(0, glyphs.Capacity)];
		StartCoroutine(Effect());
	}

	private IEnumerator Effect()
	{
		yield return null;
		AudioManager.Instance.PlaySFX(sound.clip, 0.1f);
		yield return new WaitForSeconds(lifeTime);
		Object.Destroy(base.gameObject);
	}
}
