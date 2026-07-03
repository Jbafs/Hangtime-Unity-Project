using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
	private static string path = Application.persistentDataPath + "/save.json";

	public static void Save(SaveData data)
	{
		try
		{
			string contents = JsonUtility.ToJson(data, prettyPrint: true);
			File.WriteAllText(path, contents);
			Debug.Log("[SaveSystem] Saved to " + path);
		}
		catch (Exception arg)
		{
			Debug.LogError($"[SaveSystem] Save failed: {arg}");
		}
	}

	public static SaveData Load()
	{
		try
		{
			if (!File.Exists(path))
			{
				Debug.Log("[SaveSystem] No save found, creating new one");
				return SaveData.NewDefault();
			}
			SaveData result = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
			Debug.Log("[SaveSystem] Loaded from " + path);
			return result;
		}
		catch (Exception arg)
		{
			Debug.LogError($"[SaveSystem] Load failed: {arg}");
			return SaveData.NewDefault();
		}
	}

	public static void DeleteSave()
	{
	}
}
