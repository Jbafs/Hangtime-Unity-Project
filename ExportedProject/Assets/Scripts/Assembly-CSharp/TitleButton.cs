using UnityEngine;

public class TitleButton : OnClick
{
	[SerializeField]
	private TitleController title;

	[SerializeField]
	private int myNumberOfPlayers;

	public override void Click()
	{
		GameManager.Instance.numberOfPlayers = myNumberOfPlayers;
		StartCoroutine(title.StartGame());
		GameManager.Instance.ResetGame();
		PauseController.paused = false;
		StartCoroutine(GameManager.Instance.LoadGame());
	}
}
