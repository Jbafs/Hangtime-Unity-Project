using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalInputManager : MonoBehaviour
{
	public PlayerController player1;

	public PlayerController player2;

	[SerializeField]
	private GameObject controllerErrorMessage;

	private void OnEnable()
	{
		if (!TitleController.onTitle)
		{
			StartCoroutine(DelayedEnable());
		}
	}

	private IEnumerator DelayedEnable()
	{
		yield return null;
		InputActionMap gameplay = GameManager.Instance.inputManager.Gameplay;
		GameManager.OnPlayerLose += HandleLoss;
		gameplay.FindAction("MoveP1").performed += OnMoveP1;
		gameplay.FindAction("UpP1").performed += OnUpP1;
		gameplay.FindAction("DownP1").performed += OnDownP1;
		if (GameManager.Instance.numberOfPlayers == 2)
		{
			gameplay.FindAction("MoveP2").performed += OnMoveP2;
			gameplay.FindAction("UpP2").performed += OnUpP2;
			gameplay.FindAction("DownP2").performed += OnDownP2;
		}
		else
		{
			gameplay.FindAction("MoveP3").performed += OnMoveP1;
			gameplay.FindAction("UpP3").performed += OnUpP1;
			gameplay.FindAction("DownP3").performed += OnDownP1;
		}
	}

	private bool FromCorrectController(InputAction.CallbackContext ctx, int playerNumber)
	{
		if (Gamepad.all.Count < 2)
		{
			return true;
		}
		Gamepad gamepad = Gamepad.all[playerNumber - 1];
		return ctx.control.device == gamepad;
	}

	private void OnMoveP1(InputAction.CallbackContext ctx)
	{
		if (!(player1 == null) && FromCorrectController(ctx, 1))
		{
			player1.SetXInput(ctx.ReadValue<Vector2>().x);
		}
	}

	private void OnUpP1(InputAction.CallbackContext ctx)
	{
		if (!(player1 == null) && FromCorrectController(ctx, 1))
		{
			player1.UpInputPressed();
		}
	}

	private void OnDownP1(InputAction.CallbackContext ctx)
	{
		if (!(player1 == null) && FromCorrectController(ctx, 1))
		{
			player1.DownInputPressed();
		}
	}

	private void OnMoveP2(InputAction.CallbackContext ctx)
	{
		if (!(player2 == null) && FromCorrectController(ctx, 2))
		{
			player2.SetXInput(ctx.ReadValue<Vector2>().x);
		}
	}

	private void OnUpP2(InputAction.CallbackContext ctx)
	{
		if (!(player2 == null) && FromCorrectController(ctx, 2))
		{
			player2.UpInputPressed();
		}
	}

	private void OnDownP2(InputAction.CallbackContext ctx)
	{
		if (!(player2 == null) && FromCorrectController(ctx, 2))
		{
			player2.DownInputPressed();
		}
	}

	private void OnDisable()
	{
		if (!TitleController.onTitle)
		{
			InputActionMap gameplay = GameManager.Instance.inputManager.Gameplay;
			gameplay.FindAction("MoveP1").performed -= OnMoveP1;
			gameplay.FindAction("UpP1").performed -= OnUpP1;
			gameplay.FindAction("DownP1").performed -= OnDownP1;
			if (GameManager.Instance.numberOfPlayers == 2)
			{
				gameplay.FindAction("MoveP2").performed -= OnMoveP2;
				gameplay.FindAction("UpP2").performed -= OnUpP2;
				gameplay.FindAction("DownP2").performed -= OnDownP2;
			}
			else
			{
				gameplay.FindAction("MoveP3").performed -= OnMoveP1;
				gameplay.FindAction("UpP3").performed -= OnUpP1;
				gameplay.FindAction("DownP3").performed -= OnDownP1;
			}
		}
	}

	private void Start()
	{
		StartCoroutine(SetCoopMode(GameManager.Instance.numberOfPlayers == 2));
	}

	private IEnumerator SetCoopMode(bool enabled)
	{
		InputActionMap gameplay = GameManager.Instance.inputManager.Gameplay;
		InputAction inputAction = gameplay.FindAction("MoveP2");
		InputAction inputAction2 = gameplay.FindAction("UpP2");
		InputAction inputAction3 = gameplay.FindAction("DownP2");
		if (player2 != null)
		{
			player2.gameObject.SetActive(enabled);
		}
		if (enabled)
		{
			inputAction?.Enable();
			inputAction2?.Enable();
			inputAction3?.Enable();
			yield return null;
			if (Gamepad.all.Count < 2 && GameManager.Instance.ControllerInput.controllerConnected && !TitleController.onTitle)
			{
				controllerErrorMessage.SetActive(value: true);
			}
		}
		else
		{
			inputAction?.Disable();
			inputAction2?.Disable();
			inputAction3?.Disable();
		}
	}

	private void HandleLoss()
	{
		GameManager.Instance.inputManager.UpdateInputMaps(menuInputActive: true, "lose menu");
	}
}
