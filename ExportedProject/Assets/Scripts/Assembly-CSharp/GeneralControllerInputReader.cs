using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralControllerInputReader : MonoBehaviour
{
	public bool controllerConnected;

	public Vector2 joystickInput;

	public event Action buttonClickedInput;

	public event Action pauseInput;

	public void OnJoystickMove(InputAction.CallbackContext ctx)
	{
		if (base.isActiveAndEnabled && base.gameObject.activeInHierarchy)
		{
			joystickInput = ctx.ReadValue<Vector2>();
		}
	}

	public void OnButtonPressed(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			this.buttonClickedInput?.Invoke();
		}
	}

	public void OnStartPress(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			this.pauseInput?.Invoke();
		}
	}

	private void Update()
	{
		controllerConnected = Gamepad.all.Count > 0;
	}
}
