using UnityEngine;

public class EventResult : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMonsterEventCleared(string eventName)
    {
        Debug.Log($"이벤트 클리어: {eventName}");    // 여기에 원하는 처리 추가}

        switch (eventName)
        {
            case "RaidersMonSpawn":
                FighterManager.Instance.upgradeManager.GetScrap(40);
                break;
            case "RaiderScavengerSpawn":
                break;
            case "DecoyScavengerSpawn":
                FighterManager.Instance.upgradeManager.GetScrap(30);
                break;
            case "SuspiciousMonSpawn":
                break;
            case "RuleScavengerSpawn":
                break;
            default:
                break;

		}

    }
}
