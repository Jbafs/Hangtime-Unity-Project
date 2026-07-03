using System;
using UnityEngine;

[Serializable]
public class Logo_technique
{
	[SerializeField]
	private Technique myTechnique;

	public string title;

	public string description;

	public string flavourText;

	public Sprite sprite;

	public Technique GetTechnique()
	{
		return myTechnique;
	}
}
