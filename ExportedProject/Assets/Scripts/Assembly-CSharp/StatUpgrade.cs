using System;

[Serializable]
public class StatUpgrade
{
	public string statName;

	public int currentLevel;

	public float[] levels;

	public bool CanUpgrade()
	{
		if (currentLevel == 3 && !GameManager.statOverload)
		{
			return false;
		}
		return currentLevel < levels.Length - 1;
	}

	public void Upgrade()
	{
		if (!CanUpgrade())
		{
			return;
		}
		currentLevel++;
		if (currentLevel == 4)
		{
			switch (statName)
			{
			case "Spike":
				LimitBreakEffects.limitSpike = true;
				break;
			case "Bump":
				LimitBreakEffects.limitBump = true;
				break;
			case "Jump":
				LimitBreakEffects.limitJump = true;
				break;
			case "Block":
				LimitBreakEffects.limitBlock = true;
				break;
			case "SpinServe":
				LimitBreakEffects.limitSpinServe = true;
				break;
			case "FloatServe":
				LimitBreakEffects.limitFloatServe = true;
				break;
			}
		}
	}

	public float GetCurrentValue()
	{
		return levels[currentLevel];
	}

	public float GetLevel()
	{
		return currentLevel;
	}
}
