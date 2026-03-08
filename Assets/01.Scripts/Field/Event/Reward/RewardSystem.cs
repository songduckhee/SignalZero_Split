using UnityEngine;
using UnityEngine.Rendering;

public class RewardSystem : MonoBehaviour
{
	public static RewardSystem Instance;

	[SerializeField] private RewardDatabase rewardDatabase;
	[SerializeField] private SessionRegistry sessionRegistry;

	private void Awake()
	{
		Instance = this;
		rewardDatabase = GetComponent<RewardDatabase>();
		sessionRegistry = GetComponent<SessionRegistry>();
	}

	// ✅ 몬스터가 죽을 때 호출되는 함수
	public void OnMonsterKilled(string sessionId)
	{
		if (string.IsNullOrEmpty(sessionId))
		{
			return;
		}

		// 1) 세션에 매핑된 보상 규칙 ID 찾기
		string rewardProfileId = sessionRegistry.GetRewardProfileId(sessionId);

		if (string.IsNullOrEmpty(rewardProfileId))
		{
			Debug.LogWarning($"No rewardProfileId for sessionId={sessionId}");
			return;
		}

		// 2) 보상 규칙 데이터 가져오기
		RewardProfile profile = rewardDatabase.Get(rewardProfileId);

		if (profile == null)
		{
			return;
		}

		// 3) 실제 지급
		PayReward(profile);
	}

	private void PayReward(RewardProfile profile)
	{
		if (profile.scrapPerKill != 0)
		{
			FighterManager.Instance.upgradeManager.GetScrap(profile.scrapPerKill);
		}

		if (profile.creditPerKill != 0)
		{
			CraditReward(profile.scrapPerKill);
		}
		if (!string.IsNullOrWhiteSpace(profile.dropItemCode))
		{

			WeaponReward(profile.dropItemCode);
		}
	}

	//크레딧을 주는 방식
	public void CraditReward(int count)
	{
		ItemData cradit = EventManager.Instance.Cradit;
		for (int i = 0; i < count; i++)
		{
			InventoryManager.Instance.FighterGetItem(cradit);
		}
	}
	public void WeaponReward(string itemName)
	{
		ItemData weapon = EventManager.Instance.FindItemData(itemName);
		InventoryManager.Instance.FighterGetItem(weapon);
	}
}
