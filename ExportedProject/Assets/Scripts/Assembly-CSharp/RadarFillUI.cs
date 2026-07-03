using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class RadarFillUI : Graphic
{
	[Tooltip("UI points in ANGLE ORDER (clockwise or CCW) in the SAME hierarchy as this Graphic.")]
	public List<Transform> points = new List<Transform>();

	[Tooltip("Optional center override. If null, uses this rect's center.")]
	public Transform centerOverride;

	public bool isVisible;

	[SerializeField]
	private float updateDelay;

	[SerializeField]
	private float epsilon = 0.5f;

	private Canvas _canvas;

	protected override void Awake()
	{
		base.Awake();
		_canvas = GetComponentInParent<Canvas>();
		StartCoroutine(SlowUpdate());
	}

	private IEnumerator SlowUpdate()
	{
		if (updateDelay == 0f)
		{
			yield return null;
		}
		else
		{
			yield return new WaitForSeconds(updateDelay);
		}
		Refresh();
		StartCoroutine(SlowUpdate());
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
		if (points == null || points.Count < 3 || !isVisible)
		{
			return;
		}
		Vector2 vector = (centerOverride ? WorldToLocal(centerOverride.position) : base.rectTransform.rect.center);
		vh.AddVert(vector, color, Vector2.zero);
		for (int i = 0; i < points.Count; i++)
		{
			Vector2 vector2 = WorldToLocal(points[i].position);
			Vector2 vector3 = vector2 - vector;
			if (vector3.sqrMagnitude < epsilon * epsilon)
			{
				vector2 = vector + ((vector3.normalized == Vector2.zero) ? new Vector2(epsilon, 0f) : (vector3.normalized * epsilon));
			}
			vh.AddVert(vector2, color, Vector2.zero);
		}
		int count = points.Count;
		for (int j = 1; j <= count; j++)
		{
			int idx = 0;
			int idx2 = j;
			int idx3 = ((j >= count) ? 1 : (j + 1));
			vh.AddTriangle(idx, idx2, idx3);
		}
	}

	private Vector2 WorldToLocal(Vector3 worldPos)
	{
		Camera cam = (_canvas ? _canvas.worldCamera : null);
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, worldPos);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, cam, out var localPoint);
		return localPoint;
	}

	public void Refresh()
	{
		SetVerticesDirty();
	}
}
