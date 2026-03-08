using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleGenerator //옵스타클이랑 스페이스쉽 생성하는 스크립트
{
    [SerializeField] private GameObject[] _obstaclePrefabs;
	[SerializeField] private ObstacleLayoutSettings _layoutSettings;
	[SerializeField] private GameObject[] _spaceship;
	private int shipCount = 15;

	public ObstacleGenerator(GameObject[] obstaclePrefabs,ObstacleLayoutSettings layoutSettings, GameObject[] spaceship)
	{
		_obstaclePrefabs = obstaclePrefabs;
		_layoutSettings = layoutSettings;
		_spaceship = spaceship;
	}

	public List<GameObject> SpawnObstacle(Transform parents , List<GameObject> spawnedFields, bool isUnDestroyed = false, int? spawncount = null )
	{
		List<GameObject> spawnedObstacles = new List<GameObject>();
		for (int i = 0; i < spawnedFields.Count; i++)
		{
			int spawnCount =  spawncount??  Random.Range(0, 5);
			for ( int q = 0;  q < spawnCount; q++ )
			{
				int num = Random.Range(0, _obstaclePrefabs.Length);

				GameObject obstacle = _obstaclePrefabs[num];
				GameObject spawnObstacle = GameObject.Instantiate(obstacle, parents);
				DestructibleObstacle destructibleObstacle = spawnObstacle.GetComponent<DestructibleObstacle>();
				destructibleObstacle.isUnDestroyed = isUnDestroyed;
				if(isUnDestroyed == true)
				{
					Collider col = spawnObstacle.GetComponent<Collider>();
					if(col != null)
					{
						col.enabled = false;
					}
					destructibleObstacle.enabled = false;
				}
				spawnObstacle.transform.position = _layoutSettings.GetPosition(i ,spawnedFields);
				spawnObstacle.transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);

				//spwnObstacle에 인덱스 추가

				spawnedObstacles.Add(spawnObstacle);
			}
		}
		return spawnedObstacles;
	}


	public List<GameObject> SpawnSpaceShip(Transform parents, List<GameObject> spawnedFields)
	{
		List<GameObject> shipList = new List<GameObject>();
		for (int i = 0; i < shipCount; i++)
		{
			int spawnCount = Random.Range(1, 3);
			int fieldIndex = Random.Range (0, spawnedFields.Count);
			for (int q = 0; q < spawnCount; q++)
			{
				int num = Random.Range(0, _spaceship.Length);
				ObstacleShipEnter pop = _spaceship[num].GetComponent<ObstacleShipEnter>();
				GameObject selectShip = _spaceship[num];
				GameObject spawnedSpaceShip = GameObject.Instantiate(selectShip, parents);
				int random = Random.Range(-10,21);
				float offsetY = 2f;
				float offsetX = random;
				spawnedSpaceShip.transform.position = _layoutSettings.GetPosition(fieldIndex, spawnedFields) + new Vector3(offsetX, offsetY, 0);
				spawnedSpaceShip.transform.rotation = Quaternion.Euler(0f,Random.Range(0,360f),0f);
				shipList.Add(spawnedSpaceShip);
			}
		}
		return shipList;

	}
}
