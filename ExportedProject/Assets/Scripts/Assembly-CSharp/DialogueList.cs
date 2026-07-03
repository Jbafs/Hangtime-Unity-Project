using System;
using System.Collections.Generic;

[Serializable]
public struct DialogueList
{
	public List<DialogueOptions> dialogueOptions;

	public string GetDialogue(string teamName)
	{
		foreach (DialogueOptions dialogueOption in dialogueOptions)
		{
			if (dialogueOption.teamName == teamName)
			{
				return dialogueOption.GetDialogue();
			}
		}
		return "";
	}

	public string GetDialogue(int gameNum)
	{
		foreach (DialogueOptions dialogueOption in dialogueOptions)
		{
			if (dialogueOption.teamName == gameNum.ToString())
			{
				return dialogueOption.GetDialogue();
			}
		}
		return "";
	}
}
