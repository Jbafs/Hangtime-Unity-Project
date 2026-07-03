using UnityEngine;

public class SetTeamStat : MonoBehaviour
{
	private enum Team
	{
		Player = 0,
		Opponent = 1
	}

	[SerializeField]
	private Team team;

	private PlayerStats myStats;

	private void Start()
	{
		myStats = base.gameObject.GetComponent<PlayerStats>();
		if (team == Team.Player)
		{
			myStats.setStats(GameManager.Instance.playerStats.GetAllStats(), GameManager.Instance.playerStats.GetAllTechniques());
		}
	}
}
