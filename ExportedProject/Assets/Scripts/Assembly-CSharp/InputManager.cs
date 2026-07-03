using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	[SerializeField]
	private PlayerInput playerInput;

	public InputActionMap Gameplay;

	public InputActionMap menus;

	private void Awake()
	{
		Gameplay = playerInput.actions.FindActionMap("Gameplay", throwIfNotFound: true);
		menus = playerInput.actions.FindActionMap("Menus", throwIfNotFound: true);
	}

	private void Start()
	{
		StartCoroutine(InitializeControllerScheme());
	}

	private IEnumerator InitializeControllerScheme()
	{
		float start = Time.realtimeSinceStartup;
		yield return new WaitUntil(() => Time.realtimeSinceStartup > start);
		yield return new WaitForSecondsRealtime(0.05f);
		if (Gamepad.current != null)
		{
			playerInput.SwitchCurrentControlScheme(Gamepad.current);
			Debug.Log("Forced to detected Gamepad: " + Gamepad.current.displayName);
		}
		else
		{
			playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
			Debug.Log("Forced to Keyboard+Mouse");
		}
		UpdateInputMaps(menuInputActive: true, "Starting setup");
	}

	public void UpdateInputMaps(bool menuInputActive, string s)
	{
		if (menuInputActive)
		{
			playerInput.SwitchCurrentActionMap("Menus");
		}
		else
		{
			playerInput.SwitchCurrentActionMap("Gameplay");
		}
	}
}
