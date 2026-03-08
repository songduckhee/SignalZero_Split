using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
	[Header("사용할스크립트들")]
	[SerializeField] private ObstacleGenerator obstacleGenerator; //옵스타클이랑 스페이스쉽생성하는 스크립트
	[SerializeField] private ObstacleLayoutSettings obstacleLayoutSettings = new ObstacleLayoutSettings();
	[SerializeField] private ObstacleVisibilityController obstacleObjectPool = new ObstacleVisibilityController();
	[Header("프리펩들")]
	[SerializeField] private GameObject[] _obstaclePrefabs;
	[SerializeField] private GameObject[] _spaceShipPrefabs;
	[Header("리스트들")]
	[SerializeField] private List<GameObject> spawnedObstacles;
	[SerializeField] private List<GameObject> spawnedSpaceShips;
	[SerializeField] private List <GameObject> spawnedUnDestroyObstacles;
	[SerializeField] private List<GameObject> spawnedBackgroundObstacles;

	[SerializeField] private Transform obstaclePool;
	[SerializeField] private Transform spaceShipPool;
	[SerializeField] private Transform unhideObstaclePool;
	[SerializeField] private Transform backgroundObstaclePool;

	[SerializeField] private List<ObstacleShipSO> obstacleShipSO;

	[Header("이펙트")]
	public GameObject explosionEffect;

	public GameObject player;
	private float timer = 0f;
	[SerializeField] bool isHide;
	[SerializeField] float Yoffset = -15f;
	[SerializeField] int backgroundSpawnCount = 30;

	private bool isSpawned;

	private void Awake()
	{
		obstacleGenerator = new ObstacleGenerator(_obstaclePrefabs, obstacleLayoutSettings,_spaceShipPrefabs);
	}
	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= 0.5f && isSpawned == false)
		{
			obstacleObjectPool.showObstacle(player, spawnedObstacles);
			//obstacleObjectPool.showObstacle(player, spawnedSpaceShips);
			obstacleObjectPool.showObstacle(player, spawnedUnDestroyObstacles);
			obstacleObjectPool.showObstacle(player, spawnedBackgroundObstacles);
			timer = 0f;
		}
	}

	public void Init()
	{
		List<GameObject> spawnedFields = FieldManager.Instance.SpawnedFields;
		List<GameObject> obstacles = obstacleGenerator.SpawnObstacle(obstaclePool,spawnedFields);
		spawnedObstacles.AddRange(obstacles);
		//List<GameObject> spaceShips = obstacleGenerator.SpawnSpaceShip(spaceShipPool,spawnedFields);
		//spawnedSpaceShips.AddRange(spaceShips);
		List<GameObject> unhideObstacles = obstacleGenerator.SpawnObstacle(unhideObstaclePool,spawnedFields,true);
		spawnedUnDestroyObstacles.AddRange(unhideObstacles);
		List<GameObject> backgroundObstacle = obstacleGenerator.SpawnObstacle(backgroundObstaclePool,spawnedFields,true,backgroundSpawnCount);
		spawnedBackgroundObstacles.AddRange(backgroundObstacle);
		obstacleLayoutSettings.ObstaclePositionChange(spawnedBackgroundObstacles, Yoffset);
		obstacleLayoutSettings.ObstaclePositionChange(spawnedUnDestroyObstacles, Yoffset);
		obstacleLayoutSettings.ObstacleScaleChange(spawnedBackgroundObstacles,1,10);
		obstacleLayoutSettings.ObstacleScaleChange(spawnedObstacles,1,10);

		obstacleObjectPool.hideAllObstacle(spawnedObstacles);
		obstacleObjectPool.hideAllObstacle(spawnedSpaceShips);
		obstacleObjectPool.hideAllObstacle(spawnedUnDestroyObstacles);
		obstacleObjectPool.hideAllObstacle(spawnedBackgroundObstacles);
		isSpawned = false;
	}
	public void ClearAllObstacles()
	{
		foreach ( GameObject obstacle in spawnedObstacles)
		{
			if (obstacle != null)
			{
				Destroy(obstacle);
			}
		}
		spawnedObstacles.Clear();
		foreach (GameObject obstacle in spawnedSpaceShips)
		{
			if(obstacle != null)
			{
				Destroy(obstacle);
			}
		}
		spawnedSpaceShips.Clear();
		foreach (GameObject obstacle in spawnedUnDestroyObstacles)
		{
			if (obstacle != null)
			{
				Destroy(obstacle);
			}
		}
		spawnedUnDestroyObstacles.Clear();
		foreach (GameObject obstacle in spawnedBackgroundObstacles)
		{
			if (obstacle != null)
			{
				Destroy(obstacle);
			}
		}
		spawnedBackgroundObstacles.Clear();
		isSpawned = true;
	}
	public void RebuildObstacle()
	{
		ClearAllObstacles();
		Init();
	}


	public void RemoveObstacleFromList(GameObject obstacle)
	{
		spawnedObstacles.Remove(obstacle);
	}

	public void RespawnObstacle()
	{

	}


}

