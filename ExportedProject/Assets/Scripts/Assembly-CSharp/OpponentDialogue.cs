using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentDialogue : MonoBehaviour
{
	private int dialogueCounter;

	private int lastDialogue;

	[SerializeField]
	private TypeWriterEffect dialogueText;

	private Animation dialogueEnd;

	private void Start()
	{
		dialogueEnd = base.gameObject.GetComponent<Animation>();
	}

	public void TryDialogue(List<string> dialogue, float pitch = 1f)
	{
		dialogueCounter++;
		if (dialogueCounter == 2)
		{
			dialogueCounter = 0;
			int num = Random.Range(0, dialogue.Capacity);
			int num2 = 0;
			while (num == lastDialogue && num2 < 10)
			{
				num2++;
				num = Random.Range(0, dialogue.Capacity);
			}
			PlayDialogue(dialogue[num], pitch);
			lastDialogue = num;
		}
	}

	public void PlayDialogue(string dialogue, float pitch = 1f)
	{
		if (!TutorialManager.onTutorial && !TitleController.onTitle)
		{
			dialogueText.StartTypeText(dialogue, setter: false, 0.3f, pitch);
			StartCoroutine(DelayedClear());
		}
	}

	private IEnumerator DelayedClear()
	{
		yield return new WaitForSeconds(2f);
		dialogueEnd.Play();
		yield return new WaitForSeconds(0.1f);
		StartCoroutine(dialogueText.TypeText("", setter: false));
	}

	public IEnumerator DelayedDialogue(string dialogue, float delay, float pitch = 1f)
	{
		yield return new WaitForSeconds(delay);
		PlayDialogue(dialogue, pitch);
	}
}
