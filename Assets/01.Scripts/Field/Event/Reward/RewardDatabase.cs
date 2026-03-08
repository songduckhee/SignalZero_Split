using System.Collections.Generic;
using UnityEngine;

public class RewardDatabase : MonoBehaviour //세션아이디를 통해 가져온 리워드프로파일이름으로 리워드프로파일을 가져오는 스크립트
{
	[SerializeField] private List<RewardProfile> profiles;

	private Dictionary<string, RewardProfile> _map;

	private void Awake()
	{
		_map = new Dictionary<string, RewardProfile>();

		foreach (var p in profiles)
		{
			if (p == null)
			{
				continue;
			}

			// ✅ "RW_Reject" 같은 id를 RewardProfile 안에 따로 두는 걸 추천
			// (에셋 이름 바꿔도 안전)
			_map[p.profileId] = p; // 자동 딕셔너리 완성
		}
	}

	public RewardProfile Get(string id)
	{
		if (_map.TryGetValue(id, out var profile))
		{
			return profile;
		}

		Debug.LogError($"RewardProfile not found. id={id}");
		return null;
	}
}
