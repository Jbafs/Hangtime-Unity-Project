using UnityEngine;

public class TrophySpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject trophy;

	private void Start()
	{
		if (TitleController.onTitle)
		{
			for (int i = 0; i < GameManager.Instance.saveData.trophies; i++)
			{
				base.transform.position = new Vector3(Random.Range(4, 10), 4f, 0f);
				Object.Instantiate(trophy, base.transform.position, Quaternion.identity).transform.Rotate(0f, 0f, Random.Range(-30, 30));
			}
		}
	}
}
