using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LineUp
{
	public List<GameObject> lineUp;

	public GameObject GetTeam(int num)
	{
		return lineUp[num];
	}
}
