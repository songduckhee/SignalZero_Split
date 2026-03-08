using System.Collections.Generic;
using UnityEngine;

public class ObstacleVisibilityController
{
    [SerializeField] private float maxDistance = 300f;
    public void hideAllObstacle(List<GameObject> spawnedObstacles)
    {
        for (int i = 0; i < spawnedObstacles.Count; i++)
        {
            spawnedObstacles[i].SetActive(false);
        }
    }
    public void showObstacle(GameObject player,List<GameObject> spawnedObstacles)
    {
        foreach(GameObject obstacle in spawnedObstacles)
        {
            DestructibleObstacle? destructible = obstacle.GetComponent<DestructibleObstacle>();
			float distance = Vector3.Distance(player.transform.position , obstacle.transform.position);
            if(distance <= maxDistance )
            {
                if (destructible != null && destructible.isDestroy == true)
                {
                    obstacle.SetActive(false);
                    return;
                }
                obstacle.SetActive(true);
            }
            else
            {
                obstacle.SetActive(false);
            }
		}
    }
}
