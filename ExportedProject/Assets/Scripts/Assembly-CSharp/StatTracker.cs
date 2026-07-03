using UnityEngine;

public class StatTracker : MonoBehaviour
{
	public static int playerScore;

	public static int opponentScore;

	public static int aces;

	public static int perfectRecieves;

	public static int blocks;

	public static int spikes;

	public static int tips;

	public static float runTimer;

	public static void ClearAllStats()
	{
		playerScore = 0;
		opponentScore = 0;
		aces = 0;
		perfectRecieves = 0;
		blocks = 0;
		spikes = 0;
		tips = 0;
	}

	private void Update()
	{
		runTimer += Time.deltaTime;
	}
}
