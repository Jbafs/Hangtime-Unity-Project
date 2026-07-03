using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerInput : MonoBehaviour
{
	private PlayerController pc;

	private Vector2 moveInput;

	private void Awake()
	{
		pc = GetComponent<PlayerController>();
	}

	public void OnMove(InputAction.CallbackContext ctx)
	{
		if (!TitleController.onTitle && !GameManager.gameOver && GameManager.Instance.numberOfPlayers != 2)
		{
			moveInput = ctx.ReadValue<Vector2>();
			pc.SetXInput(Mathf.Clamp(moveInput.x * 2f, -1f, 1f));
		}
	}

	public void OnUpInput(InputAction.CallbackContext ctx)
	{
		if (!TitleController.onTitle && !GameManager.gameOver && GameManager.Instance.numberOfPlayers != 2 && ctx.performed)
		{
			pc.UpInputPressed();
		}
	}

	public void OnDownInput(InputAction.CallbackContext ctx)
	{
		if (!TitleController.onTitle && !GameManager.gameOver && GameManager.Instance.numberOfPlayers != 2 && ctx.performed)
		{
			pc.DownInputPressed();
		}
	}
}
