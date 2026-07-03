using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueOptions
{
	public string teamName;

	public List<string> dialogue;

	public string GetDialogue()
	{
		return dialogue[UnityEngine.Random.Range(0, dialogue.Count)];
	}
}
