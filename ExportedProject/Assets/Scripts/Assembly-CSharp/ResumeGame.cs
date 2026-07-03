using UnityEngine;

public class ResumeGame : OnClick
{
	[SerializeField]
	private PauseController pauseController;

	public override void Click()
	{
		pauseController.Resume();
	}
}
