using UnityEngine;

public class MultiplayerController : MonoBehaviour
{
	[SerializeField]
	private GameObject botSetter;

	[SerializeField]
	private GameObject player2Object;

	private void Start()
	{
		if (GameManager.Instance.numberOfPlayers == 2)
		{
			botSetter.SetActive(value: false);
		}
		else
		{
			player2Object.SetActive(value: false);
		}
	}
}
