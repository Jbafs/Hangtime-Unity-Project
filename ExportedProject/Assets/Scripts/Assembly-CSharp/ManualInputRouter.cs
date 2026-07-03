using UnityEngine;
using UnityEngine.InputSystem;

public class ManualInputRouter : MonoBehaviour
{
	[SerializeField]
	private PlayerController player1;

	[SerializeField]
	private PlayerController player2;

	[Header("Keyboard Split Keys")]
	public Key p1Left = Key.LeftArrow;

	public Key p1Right = Key.RightArrow;

	public Key p1Up = Key.UpArrow;

	public Key p1Down = Key.DownArrow;

	public Key p2Left = Key.A;

	public Key p2Right = Key.D;

	public Key p2Up = Key.W;

	public Key p2Down = Key.S;

	private Keyboard kb;

	private void Awake()
	{
		kb = Keyboard.current;
	}

	private void Update()
	{
		int num = Mathf.Max(1, GameManager.Instance.numberOfPlayers);
		if (!TitleController.onTitle && !GameManager.gameOver && num != 1)
		{
			DriveWithKeyboard(player1, p1Left, p1Right, p1Up, p1Down);
			DriveWithKeyboard(player2, p2Left, p2Right, p2Up, p2Down);
		}
	}

	private void DriveWithKeyboard(PlayerController pc, Key left, Key right, Key up, Key down)
	{
		if (kb != null)
		{
			float xInput = 0f;
			if (kb[left].isPressed && !kb[right].isPressed)
			{
				xInput = -1f;
			}
			else if (kb[right].isPressed && !kb[left].isPressed)
			{
				xInput = 1f;
			}
			pc.SetXInput(xInput);
			if (kb[up].wasPressedThisFrame)
			{
				pc.UpInputPressed();
			}
			if (kb[down].wasPressedThisFrame)
			{
				pc.DownInputPressed();
			}
		}
	}

	private void DisablePlayerInputIfPresent(PlayerController pc)
	{
		if (!(pc == null))
		{
			PlayerInput component = pc.GetComponent<PlayerInput>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}
}
