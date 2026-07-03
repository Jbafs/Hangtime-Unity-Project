using UnityEngine;

public class SetMask : MonoBehaviour
{
	private void Start()
	{
		base.gameObject.GetComponent<SpriteMask>().sprite = base.gameObject.GetComponent<SpriteRenderer>().sprite;
	}
}
