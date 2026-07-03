using TMPro;
using UnityEngine;

public class PointText : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro points;

	private void Update()
	{
		points.text = GameManager.Instance.playerPoints + " - " + GameManager.Instance.opponentPoints;
	}
}
