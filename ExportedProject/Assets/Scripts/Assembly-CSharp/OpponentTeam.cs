using System.Collections.Generic;
using UnityEngine;

public class OpponentTeam : MonoBehaviour
{
	public string teamName;

	private PlayerStats playerStats;

	public Sprite banner;

	public string introDialogue;

	public string loseDialogue;

	public string winDialogue;

	public List<string> gotPointDialogue;

	public List<string> lostPointDialogue;

	public float talkPitch;

	[SerializeField]
	private List<StatWeight> statWeights;

	public void Init(float skillPoints)
	{
		playerStats = base.gameObject.GetComponent<PlayerStats>();
		DistributeSkillPoints(skillPoints);
	}

	public string GotPointDialogue()
	{
		return gotPointDialogue[Random.Range(0, gotPointDialogue.Capacity)];
	}

	public string LostPointDialogue()
	{
		return lostPointDialogue[Random.Range(0, lostPointDialogue.Capacity)];
	}

	private void DistributeSkillPoints(float points)
	{
		for (int i = 0; (float)i < points; i++)
		{
			string randomWeightedStat = GetRandomWeightedStat();
			playerStats.GetStat(randomWeightedStat).Upgrade();
		}
	}

	private string GetRandomWeightedStat()
	{
		List<StatWeight> list = new List<StatWeight>();
		float num = 0f;
		foreach (StatWeight statWeight in statWeights)
		{
			float num2 = playerStats.GetStat(statWeight.statName).currentLevel;
			if (statWeight.weight > 0f && num2 < 3f)
			{
				list.Add(statWeight);
				num += statWeight.weight;
			}
		}
		if (list.Count == 0)
		{
			return statWeights[Random.Range(0, statWeights.Capacity)].statName;
		}
		float num3 = Random.Range(0f, num);
		float num4 = 0f;
		foreach (StatWeight item in list)
		{
			num4 += item.weight;
			if (num3 <= num4)
			{
				return item.statName;
			}
		}
		return list[list.Count - 1].statName;
	}
}
