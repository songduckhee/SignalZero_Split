using UnityEngine;

[CreateAssetMenu(menuName = "Game/Reward Profile")]
public class RewardProfile : ScriptableObject
{
	[Header("ID")]
	public string profileId;   // 예: "RW_Reject"

	[Header("Kill Reward")]
	public int scrapPerKill;
	public int creditPerKill;

	[Header("Drop")]
	public string dropItemCode;
}
