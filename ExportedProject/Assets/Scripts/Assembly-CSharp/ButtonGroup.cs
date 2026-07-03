using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGroup : MonoBehaviour
{
	public enum ScrollType
	{
		Vertical = 0,
		Horizontal = 1
	}

	[SerializeField]
	public List<ButtonController> buttons;

	private int selected;

	private float scrollCounter;

	[SerializeField]
	private ScrollType scrollType;

	[SerializeField]
	private int startIndex;

	[SerializeField]
	private bool singleSelectUpgrade;

	private void Start()
	{
		StartCoroutine(Activate());
	}

	private void OnEnable()
	{
		StartCoroutine(DelayEnable());
	}

	private IEnumerator DelayEnable()
	{
		yield return null;
		GameManager.Instance.ControllerInput.buttonClickedInput += Select;
	}

	private void OnDisable()
	{
		GameManager.Instance.ControllerInput.buttonClickedInput -= Select;
	}

	public IEnumerator Activate()
	{
		foreach (ButtonController button in buttons)
		{
			button.group = this;
		}
		yield return new WaitForSecondsRealtime(0.02f);
		if (singleSelectUpgrade && GameManager.gameNumber == 4)
		{
			selected = 0;
		}
		else
		{
			selected = startIndex;
		}
		if (GameManager.Instance.ControllerInput.controllerConnected)
		{
			buttons[selected].Selected();
			Debug.Log("Button: " + selected + " starts selected");
		}
	}

	private void Select()
	{
		buttons[selected].Press();
	}

	private void Update()
	{
		scrollCounter -= Time.unscaledDeltaTime;
		float num = 0f;
		if (scrollType == ScrollType.Horizontal)
		{
			num = 0f - GameManager.Instance.ControllerInput.joystickInput.x;
		}
		else if (scrollType == ScrollType.Vertical)
		{
			num = GameManager.Instance.ControllerInput.joystickInput.y;
		}
		if (num < -0.4f && scrollCounter < 0f)
		{
			ChangeSelect(1);
		}
		else if (num > 0.4f && scrollCounter < 0f)
		{
			ChangeSelect(-1);
		}
	}

	private void ChangeSelect(int addNum)
	{
		if ((selected != 0 || addNum >= 0) && (selected != buttons.Count - 1 || addNum <= 0))
		{
			buttons[selected].Deselected();
			selected += addNum;
			buttons[selected].Selected();
			scrollCounter = 0.2f;
		}
	}

	public void DisactivateButtons()
	{
		foreach (ButtonController button in buttons)
		{
			button.interactable = false;
		}
	}
}
