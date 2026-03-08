using System;
using UnityEngine;

[Serializable]
public class GridLayoutSettings
{
	public int Width = 10;
	public int Height = 10;
	public int tileSize = 200;
	public int y= -1;

	public int TotalCount
	{
		get
		{
			return
				Width * Height;
		}
	}

	public Vector3 GetPosition(int index)
	{

		int x = index % Width - 5;
		int z = index / Height;
		// x는 -5 ~ 4, z는 0 ~ 9

		Vector3 pos = new Vector3
		(
			x * tileSize,
			y,
			z * tileSize

		);
		return pos;

	}

	public Vector2Int GetGridPosition(int index)
	{

		int x = index % Width;// 0에서 스폰카운트 99까지 들어가기때문에 맨마지막 나머지 숫자는 9
		int z = index / Height; // 0에서 스폰카운트 99까지 들어가기때문에 맨 마지막 나누기 숫자는 9
		// x는 0 ~ 9, z는 0 ~ 9

		Vector2Int pos = new Vector2Int
		(
			x ,
			z 

		);
		return pos;

	}
}
