using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualPadHotkey : MonoBehaviour
{
	private void Update()
	{
		if (Keyboard.current.pKey.wasPressedThisFrame)
		{
			InputSystem.AddDevice<Gamepad>();
			Debug.Log("Added virtual gamepad!");
		}
	}
}
