using DG.Tweening;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
	public ButtonGroup group;

	[SerializeField]
	private OnClick onClick;

	public bool isHovering;

	private float originalScale;

	public bool interactable;

	public bool wasClicked;

	private float startDelay = 0.3f;

	[SerializeField]
	private bool singleButton;

	[SerializeField]
	private SoundLibrary soundLibrary;

	[SerializeField]
	private bool dontAllowOnController;

	private void Awake()
	{
		originalScale = base.transform.localScale.x;
		if (singleButton)
		{
			GameManager.Instance.ControllerInput.buttonClickedInput += Press;
			isHovering = true;
		}
	}

	private void Update()
	{
		startDelay -= Time.unscaledDeltaTime;
	}

	private void OnEnable()
	{
		wasClicked = false;
		interactable = true;
	}

	private void OnMouseEnter()
	{
		Selected();
		Debug.Log("Select");
	}

	private void OnMouseExit()
	{
		Deselected();
	}

	private void OnMouseDown()
	{
		Press();
		Debug.Log("Press");
	}

	public void Selected()
	{
		if (interactable)
		{
			isHovering = true;
			base.transform.DOScale(originalScale * 1.1f, 0.2f).SetEase(Ease.OutBack).SetUpdate(isIndependentUpdate: true);
			AudioManager.Instance.PlaySFX(soundLibrary.buttonHover, 0.5f);
		}
	}

	public void Deselected()
	{
		isHovering = false;
		if (interactable)
		{
			base.transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack).SetUpdate(isIndependentUpdate: true);
		}
	}

	public void Press()
	{
		if ((!dontAllowOnController || !GameManager.Instance.ControllerInput.controllerConnected) && !(startDelay > 0f) && interactable && !wasClicked && isHovering)
		{
			AudioManager.Instance.PlaySFX(soundLibrary.buttonClick, 0.5f);
			onClick.Click();
			isHovering = false;
			wasClicked = true;
			if (group != null)
			{
				group.DisactivateButtons();
			}
		}
	}

	public void NotUsed()
	{
		base.transform.DOScale(originalScale / 1.5f, 0.2f).SetEase(Ease.OutBack).SetUpdate(isIndependentUpdate: true);
		base.transform.DOMoveY(base.transform.position.y + 26f, 1f).SetEase(Ease.InBack).SetUpdate(isIndependentUpdate: true);
	}

	private void OnDisable()
	{
		GameManager.Instance.ControllerInput.buttonClickedInput -= Press;
	}
}
