using System.Collections.Generic;
using UnityEngine;

public class TournamentLineUps : MonoBehaviour
{
	[SerializeField]
	private List<LineUp> lineUps;

	public static int currentLineUp;

	public void NewLineUp()
	{
		int num = Random.Range(0, lineUps.Count);
		int num2 = 0;
		while (num == currentLineUp && num2 < 10)
		{
			num = Random.Range(0, lineUps.Count);
			num2++;
		}
		currentLineUp = num;
		Debug.Log("Current line up: " + currentLineUp);
	}

	public GameObject GetTeam(int gameNumber)
	{
		return lineUps[currentLineUp].GetTeam(gameNumber);
	}
}
