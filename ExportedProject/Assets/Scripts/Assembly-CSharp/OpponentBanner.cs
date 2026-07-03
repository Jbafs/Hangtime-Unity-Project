using UnityEngine;

public class OpponentBanner : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer mySprite;

	public void Start()
	{
		if (GameManager.Instance.opponentTeam != null)
		{
			mySprite.sprite = GameManager.Instance.opponentTeam.banner;
		}
	}
}
