using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleLayoutSettings
{

	private float offset = 100f;
	private float y = 0.5f;

	public Vector3 GetPosition(int i, List<GameObject> _spawnedFields) // 있는 섹션의 랜덤한 좌표를 뽑아내주는 함수
	{
		float sectionX = _spawnedFields[i].transform.position.x;
		float sectionZ = _spawnedFields[i].transform.position.z;

		float x = Random.Range(sectionX - offset, sectionX + offset);
		float z = Random.Range(sectionZ - offset, sectionZ + offset);

		Vector3 _position = new Vector3(x, y, z);
		return _position;
	}
	public void ObstaclePositionChange(List<GameObject> obstaclePositions, float Yoffset)
	{
		foreach (GameObject obstacle in obstaclePositions)
		{
			obstacle.transform.localPosition += new Vector3(0, Yoffset, 0);
		}
	}
	public void ObstacleScaleChange(List<GameObject> obstacleScale, int minSize,int maxSize)
	{
		foreach(GameObject obstacle in obstacleScale)
		{
			int random = Random.Range(minSize, maxSize);
			obstacle.transform.localScale = Vector3.one * 0.1f * random;
		}
	}
}
