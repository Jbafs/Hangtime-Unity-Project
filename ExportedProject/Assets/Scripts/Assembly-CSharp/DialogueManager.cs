using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	public static bool spikerTalking;

	public GameObject spikerBubble;

	public GameObject setterBubble;

	public static int dialogueCounter;

	[SerializeField]
	private DialogueList winIntroSetter;

	[SerializeField]
	private DialogueList winIntroSpiker;

	[SerializeField]
	private List<string> winIntrosGenericSetter;

	[SerializeField]
	private List<string> winIntroGenericSpiker;

	[SerializeField]
	private List<string> prepSetter;

	[SerializeField]
	private List<string> prepSpiker;

	[SerializeField]
	private DialogueList outroSetter;

	[SerializeField]
	private DialogueList outroSpiker;

	[SerializeField]
	private List<string> outroSpikerGeneric;

	[SerializeField]
	private List<string> outroSetterGeneric;

	private void UpdateCounters()
	{
		dialogueCounter += Random.Range(1, 3);
		spikerTalking = !spikerTalking;
		spikerBubble.SetActive(spikerTalking);
		setterBubble.SetActive(!spikerTalking);
	}

	public string GetWinIntro()
	{
		UpdateCounters();
		string text = "";
		text = ((!spikerTalking) ? winIntroSetter.GetDialogue(GameManager.Instance.opponentTeam.teamName) : winIntroSpiker.GetDialogue(GameManager.Instance.opponentTeam.teamName));
		if (text.Equals(""))
		{
			text = ((!spikerTalking) ? winIntrosGenericSetter[dialogueCounter % winIntrosGenericSetter.Count] : winIntroGenericSpiker[dialogueCounter % winIntroGenericSpiker.Count]);
		}
		return text;
	}

	public string GetPrep()
	{
		UpdateCounters();
		if (spikerTalking)
		{
			return prepSpiker[dialogueCounter % prepSpiker.Capacity];
		}
		return prepSetter[dialogueCounter % prepSetter.Capacity];
	}

	public string GetOutro()
	{
		UpdateCounters();
		string text = "";
		text = ((!spikerTalking) ? outroSetter.GetDialogue(GameManager.gameNumber) : outroSpiker.GetDialogue(GameManager.gameNumber));
		if (text.Equals(""))
		{
			text = ((!spikerTalking) ? outroSetterGeneric[dialogueCounter % outroSetterGeneric.Count] : outroSpikerGeneric[dialogueCounter % outroSpikerGeneric.Count]);
		}
		return text;
	}
}
