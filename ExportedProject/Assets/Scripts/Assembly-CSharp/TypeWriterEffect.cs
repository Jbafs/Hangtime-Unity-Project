using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriterEffect : MonoBehaviour
{
	[SerializeField]
	private float typeSpeed;

	[SerializeField]
	private TextMeshPro textUI;

	[SerializeField]
	private SoundLibrary soundLibrary;

	private Coroutine typingRoutine;

	public void StartTypeText(string fullText, bool setter, float delay = 0f, float pitch = 1f)
	{
		if (typingRoutine != null)
		{
			StopCoroutine(typingRoutine);
		}
		typingRoutine = StartCoroutine(TypeText(fullText, setter, delay, pitch));
	}

	public IEnumerator TypeText(string fullText, bool setter, float delay = 0f, float pitch = 1f)
	{
		textUI.text = "";
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < fullText.Length; i++)
		{
			char c = fullText[i];
			textUI.text += c;
			if (setter)
			{
				AudioManager.Instance.PlaySFX(soundLibrary.dialogueSpiker, 0.3f, 0.25f, pitch + 0.2f);
			}
			else
			{
				AudioManager.Instance.PlaySFX(soundLibrary.dialogueSpiker, 0.3f, 0.25f, pitch);
			}
			if (c == '?' || c == '.' || c == '!')
			{
				yield return new WaitForSeconds(0.5f);
			}
			if (c == ' ')
			{
				yield return new WaitForSeconds(0.03f);
			}
			yield return new WaitForSeconds(typeSpeed);
		}
	}
}
