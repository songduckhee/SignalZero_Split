using UnityEngine;

public class SpawnSessionTag : MonoBehaviour
{
	public string SessionId { get; private set; }

	// 스폰될 때 세션 ID를 심어주는 함수
	public void Init(string sessionId)
	{
		SessionId = sessionId;
	}
}
