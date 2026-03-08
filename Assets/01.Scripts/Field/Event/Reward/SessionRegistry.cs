using System.Collections.Generic;
using UnityEngine;

public class SessionRegistry : MonoBehaviour // 세션아이디별로 리워드 아이디를 저장해놓는 스크립트
{
	public static SessionRegistry Instance;
	private Dictionary<string, string> _rewardMap = new();

	private void Awake()
	{
		Instance = this;
	}

	public void Register(string sessionId, string rewardProfileId)
	{
		_rewardMap[sessionId] = rewardProfileId;
	}

	public string GetRewardProfileId(string sessionId) //rewardProfileId out
	{
		if (sessionId == null)
		{
			return null;
		}
		if (_rewardMap.TryGetValue(sessionId, out var rewardProfileId))
		{
			return rewardProfileId;
		}
		return null;
	}
}
