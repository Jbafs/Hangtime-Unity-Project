using System;

[Serializable]
public class SaveData
{
	public bool tutorialCompleted;

	public int trophies;

	public float musicVolume = 0.15f;

	public float sfxVolume = 0.7f;

	public float sfxSliderPosition = 0.7f;

	public float numberOfBlocks;

	public static SaveData NewDefault()
	{
		return new SaveData();
	}

	public void ResetData()
	{
		trophies = 0;
		tutorialCompleted = false;
		musicVolume = 0.15f;
		sfxVolume = 0.7f;
		numberOfBlocks = 0f;
	}
}
