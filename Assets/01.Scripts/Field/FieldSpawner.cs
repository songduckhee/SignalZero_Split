using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FieldSpawner
{
	private readonly GameObject _prefab;
	private readonly GridLayoutSettings _layout;

	public FieldSpawner(GameObject prefab, GridLayoutSettings layout)
	{
		_prefab = prefab;
		_layout = layout;
	}

	public List<GameObject> SpawnAll(Transform parent)
	{
		List<GameObject> result = new List<GameObject>();
		int total = _layout.TotalCount;

		for (int i = 0; i < total; i++)
		{
			GameObject field = Object.Instantiate(_prefab, parent);
			field.transform.localPosition = _layout.GetPosition(i);
			int x = _layout.GetGridPosition(i).x;
			int y = _layout.GetGridPosition(i).y;
			field.name = $"section_{x},{y}";
			//Section section = field.GetComponent<Section>();
			//section.init(i);
			result.Add(field);
		}

		return result;
	}
}








