using UnityEngine;

public class QuitButton : OnClick
{
	public override void Click()
	{
		SaveSystem.Save(GameManager.Instance.saveData);
		Application.Quit();
	}
}
