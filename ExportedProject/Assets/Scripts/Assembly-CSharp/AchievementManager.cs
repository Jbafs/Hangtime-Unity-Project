using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	public enum Id
	{
		WinWithBlock = 0,
		LoseToPractice = 1,
		MaxOutBump = 2,
		WinNoSpikes = 3,
		BeatTenzio = 4,
		ThreeAcesMatch = 5,
		LoseToKozuki = 6,
		FiveBlocks = 7,
		QuickAttack = 8,
		TWoPlayer = 9,
		ThreeWinStreak = 10,
		WinGame = 11,
		Comeback = 12
	}

	public static AchievementManager I;

	private static readonly Dictionary<Id, string> Api = new Dictionary<Id, string>
	{
		{
			Id.WinWithBlock,
			"ACH_WIN_WITH_BLOCK"
		},
		{
			Id.LoseToPractice,
			"ACH_LOSE_TO_PRACTICE"
		},
		{
			Id.MaxOutBump,
			"ACH_MAX_OUT_BUMP"
		},
		{
			Id.WinNoSpikes,
			"ACH_WIN_NO_SPIKES"
		},
		{
			Id.BeatTenzio,
			"ACH_BEAT_TENZIO"
		},
		{
			Id.ThreeAcesMatch,
			"ACH_3_ACES_MATCH"
		},
		{
			Id.LoseToKozuki,
			"ACH_LOSE_TO_KOZUKI"
		},
		{
			Id.FiveBlocks,
			"ACH_FIVE_BLOCKS"
		},
		{
			Id.QuickAttack,
			"ACH_HIT_QUICK_ATTACK"
		},
		{
			Id.TWoPlayer,
			"TWO_PLAYER_WIN"
		},
		{
			Id.ThreeWinStreak,
			"ACE_FIRST_POINT"
		},
		{
			Id.WinGame,
			"BEAT_SHIROGANE"
		},
		{
			Id.Comeback,
			"THREE_POINT_COMEBACK"
		}
	};

	private readonly HashSet<string> _unlockedThisSession = new HashSet<string>();

	private void Awake()
	{
		if (I != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		I = this;
		Object.DontDestroyOnLoad(base.gameObject);
		if (!SteamOk())
		{
			return;
		}
		foreach (KeyValuePair<Id, string> item in Api)
		{
			if (SteamUserStats.GetAchievement(item.Value, out var pbAchieved) && pbAchieved)
			{
				_unlockedThisSession.Add(item.Value);
			}
		}
	}

	public void Unlock(Id id)
	{
		if (!SteamOk() || TutorialManager.onTutorial)
		{
			return;
		}
		string text = Api[id];
		if (!_unlockedThisSession.Contains(text))
		{
			if (SteamUserStats.GetAchievement(text, out var pbAchieved) && pbAchieved)
			{
				_unlockedThisSession.Add(text);
			}
			else if (SteamUserStats.SetAchievement(text))
			{
				SteamUserStats.StoreStats();
				_unlockedThisSession.Add(text);
			}
		}
	}

	public void SetStat(string statApiName, int value, bool store = true)
	{
		if (SteamOk())
		{
			SteamUserStats.SetStat(statApiName, value);
			if (store)
			{
				SteamUserStats.StoreStats();
			}
		}
	}

	public void AddToStat(string statApiName, int delta, bool store = true)
	{
		if (SteamOk())
		{
			SteamUserStats.GetStat(statApiName, out int pData);
			SteamUserStats.SetStat(statApiName, pData + delta);
			if (store)
			{
				SteamUserStats.StoreStats();
			}
		}
	}

	private static bool SteamOk()
	{
		return SteamManager.Initialized;
	}
}
