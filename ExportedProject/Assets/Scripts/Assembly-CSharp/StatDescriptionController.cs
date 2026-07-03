using System.Collections.Generic;
using UnityEngine;

public class StatDescriptionController : MonoBehaviour
{
	[SerializeField]
	private List<StatDescription> descriptionsNonStatic;

	[SerializeField]
	private static List<StatDescription> descriptions;

	private void Awake()
	{
		descriptions = descriptionsNonStatic;
	}

	public static string GetDescription(string stat)
	{
		foreach (StatDescription description in descriptions)
		{
			if (description.name.ToUpper() == stat.ToUpper())
			{
				return description.description;
			}
		}
		return "";
	}
}
