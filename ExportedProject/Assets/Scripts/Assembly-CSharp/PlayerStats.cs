using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	[Header("Stats")]
	public List<StatUpgrade> statUpgrades;

	public List<Technique> techniques;

	private void Start()
	{
	}

	public StatUpgrade GetStat(string statName)
	{
		return statUpgrades.Find((StatUpgrade s) => s.statName == statName);
	}

	public List<StatUpgrade> GetAllStats()
	{
		return statUpgrades;
	}

	public List<Technique> GetAllTechniques()
	{
		return techniques;
	}

	public void setStats(List<StatUpgrade> stats, List<Technique> otherTechniques)
	{
		foreach (StatUpgrade stat in stats)
		{
			statUpgrades.Add(stat);
		}
		foreach (Technique otherTechnique in otherTechniques)
		{
			techniques.Add(otherTechnique);
		}
	}
}
