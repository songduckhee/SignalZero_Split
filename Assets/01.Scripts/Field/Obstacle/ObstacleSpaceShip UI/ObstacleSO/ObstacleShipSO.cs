using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObstacleShipSO : ScriptableObject
{
	public List<string> dialogues;
	public ObstacleEventType eventType;
	public int ButtonCount;
}
