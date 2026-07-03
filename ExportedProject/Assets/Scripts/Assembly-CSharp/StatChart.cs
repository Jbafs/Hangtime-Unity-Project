using TMPro;
using UnityEngine;

public class StatChart : MonoBehaviour
{
	[SerializeField]
	private Transform[] points;

	[SerializeField]
	private TextMeshPro[] statNames;

	[SerializeField]
	private Transform backgroundHex;

	[SerializeField]
	private GameObject limitBreakEffect;

	private int counter;

	private bool doneLimitEffect;

	[SerializeField]
	private Color limitColor;

	private void Start()
	{
		SetPoints();
	}

	public void SetPoints()
	{
		Transform[] array = points;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.localPosition = Vector3.zero;
		}
		counter = 0;
		movePoint("Bump");
		movePoint("Spike");
		movePoint("Jump");
		movePoint("Block");
		movePoint("FloatServe");
		movePoint("SpinServe");
	}

	private void movePoint(string name)
	{
		points[counter].Translate(points[0].transform.up * (GameManager.Instance.playerStats.GetStat(name).GetLevel() + 1f), Space.Self);
		if (GameManager.Instance.playerStats.GetStat(name).GetLevel() == 4f)
		{
			statNames[counter].color = limitColor;
		}
		counter++;
	}

	private void Update()
	{
		if (GameManager.statOverload)
		{
			backgroundHex.transform.localScale = new Vector3(14f, 14f);
		}
		else
		{
			backgroundHex.transform.localScale = new Vector3(11f, 11f);
		}
	}
}
