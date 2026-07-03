using UnityEngine;

public class PauseController : MonoBehaviour
{
	public static bool paused;

	public GameObject pauseUI;

	[SerializeField]
	private AudioSource crowdMurmur;

	[SerializeField]
	private Transform camera;

	private PauseAnimation pauseAnimation;

	private float delayTimer;

	private float savedVolume;

	private void Start()
	{
		pauseAnimation = base.gameObject.GetComponent<PauseAnimation>();
		pauseUI.SetActive(value: false);
		GameManager.Instance.ControllerInput.pauseInput += Pause_Unpause;
	}

	public void Update()
	{
		delayTimer += Time.deltaTime;
		base.transform.position = new Vector3(camera.position.x, 0f, 0f);
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause_Unpause();
		}
		if (GameManager.gameOver)
		{
			paused = false;
		}
	}

	private void Pause_Unpause()
	{
		if (!TitleController.onTitle && !(delayTimer < 1f) && !GameManager.Instance.done)
		{
			if (paused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

	private void Pause()
	{
		GameManager.Instance.inputManager.UpdateInputMaps(menuInputActive: true, "pause ");
		paused = true;
		pauseUI.SetActive(value: true);
		StartCoroutine(pauseAnimation.PlayAnimation());
	}

	public void Resume()
	{
		GameManager.Instance.inputManager.UpdateInputMaps(menuInputActive: false, "unpause");
		paused = false;
		pauseUI.SetActive(value: false);
	}

	private void OnDisable()
	{
		GameManager.Instance.ControllerInput.pauseInput -= Pause_Unpause;
	}
}
