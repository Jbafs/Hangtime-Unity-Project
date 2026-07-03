using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	public static bool onTutorial = true;

	public static int tutorialPart;

	[SerializeField]
	private TypeWriterEffect dialogue;

	[SerializeField]
	private GameObject ballObject;

	[SerializeField]
	private Transform setter;

	private BallMovement ballMovement;

	public static Vector2 ballReset;

	private AudioSource music;

	public static int pointCounter;

	[SerializeField]
	private TutorialDiagram diagram;

	private string upInput;

	private string downInput;

	private string xMovement;

	[SerializeField]
	private SpriteRenderer jumpInputControllerSprite;

	[SerializeField]
	private SpriteRenderer tipInputControllerSprite;

	[SerializeField]
	private SpriteRenderer joystickControllerSprite;

	private bool controllerConnected;

	private void Start()
	{
		controllerConnected = GameManager.Instance.ControllerInput.controllerConnected;
		music = base.gameObject.GetComponent<AudioSource>();
		ballMovement = ballObject.GetComponent<BallMovement>();
		if (!onTutorial || TitleController.onTitle)
		{
			base.gameObject.SetActive(value: false);
		}
		else
		{
			StartCoroutine(DoTutorial());
		}
	}

	private void Update()
	{
		if (controllerConnected)
		{
			upInput = "";
			downInput = "";
			xMovement = "";
		}
		else
		{
			upInput = "( UP ARROW )";
			downInput = "( DOWN ARROW )";
			xMovement = "( ARROW KEYS )";
		}
	}

	private void SetIcon(SpriteRenderer sprite, Vector3 position)
	{
		if (controllerConnected)
		{
			sprite.enabled = true;
		}
		sprite.transform.localPosition = position;
	}

	private IEnumerator DoTutorial()
	{
		bool moveOn = false;
		yield return null;
		yield return null;
		yield return null;
		ballObject.transform.position = new Vector2(-30f, 0f);
		ballObject.SetActive(value: false);
		GameManager.Instance.currentTeam.SetActive(value: false);
		GameManager.SetMusic(music.clip, music.volume);
		tutorialPart = 1;
		yield return new WaitForSeconds(3f);
		StartCoroutine(dialogue.TypeText("Been a while ay?\nBack from... retirement?", setter: false));
		yield return new WaitForSeconds(5f);
		StartCoroutine(dialogue.TypeText("You should warm up.", setter: false));
		yield return new WaitForSeconds(2f);
		SetIcon(jumpInputControllerSprite, new Vector3(-24f, -1.95f));
		SetIcon(joystickControllerSprite, new Vector3(-24.5f, 0.87f));
		StartCoroutine(dialogue.TypeText(xMovement + " to move,\n" + upInput + " to jump.", setter: false));
		while (!moveOn)
		{
			yield return null;
			moveOn = GameManager.Instance.ControllerInput.controllerConnected || CheckInput(KeyCode.RightArrow) || CheckInput(KeyCode.LeftArrow) || CheckInput(KeyCode.D) || CheckInput(KeyCode.A);
		}
		moveOn = false;
		if (GameManager.Instance.ControllerInput.controllerConnected)
		{
			yield return new WaitForSeconds(4f);
		}
		else
		{
			while (!moveOn)
			{
				yield return null;
				moveOn = CheckInput(KeyCode.UpArrow) || CheckInput(KeyCode.W);
			}
		}
		yield return new WaitForSeconds(2f);
		jumpInputControllerSprite.enabled = false;
		joystickControllerSprite.enabled = false;
		StartCoroutine(dialogue.TypeText("Heh. Lookin a bit rusty!", setter: false));
		yield return new WaitForSeconds(3f);
		StartCoroutine(dialogue.TypeText("Lets try some spikes.", setter: false));
		yield return new WaitForSeconds(3f);
		SetIcon(jumpInputControllerSprite, new Vector3(-24.6f, 0.1f));
		StartCoroutine(dialogue.TypeText(upInput + " midair to spike.", setter: false));
		yield return new WaitForSeconds(5f);
		jumpInputControllerSprite.enabled = false;
		StartCoroutine(dialogue.TypeText("WAHA you look stupid! ;)", setter: false));
		yield return new WaitForSeconds(3f);
		StartCoroutine(dialogue.TypeText("..Lets try with an actual ball.", setter: false));
		yield return new WaitForSeconds(2f);
		ballObject.transform.position = new Vector2(-10f, 20f);
		ballReset = new Vector2(-10f, 20f);
		ballObject.SetActive(value: true);
		yield return new WaitForSeconds(1f);
		StartCoroutine(dialogue.TypeText("", setter: false));
		yield return new WaitForSeconds(5f);
		StartCoroutine(dialogue.TypeText("Oh—timing!", setter: false));
		yield return new WaitForSeconds(3f);
		StartCoroutine(dialogue.TypeText("I like to jump right as the\nball starts to fall.", setter: false));
		yield return new WaitForSeconds(6f);
		StartCoroutine(dialogue.TypeText("", setter: false));
		pointCounter = 0;
		diagram.Play();
		StartCoroutine(dialogue.TypeText("Your velocity affects your aim - ", setter: false));
		while (pointCounter < 4)
		{
			yield return null;
		}
		diagram.Stop();
		StartCoroutine(dialogue.TypeText("Looking... \nNot bad actually!", setter: false));
		yield return new WaitForSeconds(4f);
		SetIcon(tipInputControllerSprite, new Vector3(-21.2f, -1.01f));
		StartCoroutine(dialogue.TypeText("I'll show you a trick:\n" + downInput + " to tip.", setter: false));
		yield return new WaitForSeconds(6f);
		tipInputControllerSprite.enabled = false;
		StartCoroutine(dialogue.TypeText("...use it wisely.", setter: false));
		pointCounter = 0;
		while (pointCounter < 3)
		{
			yield return null;
		}
		tutorialPart = 2;
		yield return new WaitForSeconds(2.5f);
		StartCoroutine(dialogue.TypeText("Lets talk serves.", setter: false));
		yield return new WaitForSeconds(2f);
		StartCoroutine(dialogue.TypeText("Spin Serves are like spiking!\n(From outside the court)", setter: false));
		yield return new WaitForSeconds(3f);
		StartCoroutine(dialogue.TypeText("", setter: false));
		pointCounter = 0;
		while (pointCounter < 3)
		{
			yield return null;
		}
		SetIcon(tipInputControllerSprite, new Vector3(-15.3f, -1.8f));
		StartCoroutine(dialogue.TypeText("Float serve's tend to swerve mid-air: \n" + downInput, setter: false));
		pointCounter = 0;
		while (pointCounter < 2)
		{
			yield return null;
		}
		tipInputControllerSprite.enabled = false;
		StartCoroutine(dialogue.TypeText("", setter: false));
		pointCounter = 0;
		while (pointCounter < 2)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		tutorialPart = 3;
		StartCoroutine(dialogue.TypeText("Sweet. Lets practice\nsome defence.", setter: false));
		ballReset = new Vector2(5f, 20f);
		yield return new WaitForSeconds(2f);
		StartCoroutine(dialogue.TypeText("Sup club sumi.", setter: false));
		GameManager.Instance.currentTeam.SetActive(value: true);
		yield return new WaitForSeconds(3f);
		StartCoroutine(dialogue.TypeText("Recieving: Don’t jump! Let the ball come to you.", setter: false));
		yield return new WaitForSeconds(4f);
		StartCoroutine(dialogue.TypeText("", setter: false));
		pointCounter = 0;
		while (pointCounter < 3)
		{
			yield return null;
		}
		StartCoroutine(dialogue.TypeText("Feeling bold?\nJump near the net to block.", setter: false));
		yield return new WaitForSeconds(6f);
		StartCoroutine(dialogue.TypeText("", setter: false));
		pointCounter = 0;
		while (pointCounter < 3)
		{
			yield return null;
		}
		tutorialPart = 4;
		pointCounter = 0;
		while (pointCounter < 1)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		StartCoroutine(dialogue.TypeText("Alright lets play\na real match!", setter: false));
		yield return new WaitForSeconds(2f);
		onTutorial = false;
		StartCoroutine(GameManager.Instance.LoadGame());
	}

	private bool CheckInput(KeyCode input)
	{
		return Input.GetKey(input);
	}
}
