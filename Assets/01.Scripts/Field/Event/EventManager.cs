using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EventType
{
	none = 0,
	// 섹터 1
	raidersWarning = 1,
	surpriseInspection = 2,
	cleanupZone = 3,
	mysteriousSignal = 4,
	decoy = 6,
	suspiciousWeaponTrade = 7,
	repairScam = 8,
	ruleAnnouncement = 9,
	covertSupplyShip = 10,
	covertSupplyShip2 = 11,
	covertSupplyShip3 = 12,
	imperialTradeLine = 13,
	blackMarket = 14,
	dockedRescueShip = 15,
	signalRecorder = 16,
	rescueSignal = 17,
	// 섹터 2
	ghostRescueSignal = 18,
	ImperialControl = 19,
	trackingSignal = 20,
	imperialScientist = 21,
	imperialHelpRequest = 22,
	imperialSentry = 23,
	ghostSignal = 24,
	dockedRescueShip2 = 29,
	dockedCargoShip = 30,
	ghostShip = 31,
	covertSupplyShip4 = 32,
	covertSupplyShip5 = 33,
	covertSupplyShip6 = 34, // 2번 나오게됨

	// 섹터 3
	ghostRescueSignal2 = 25,
	limboWarning = 26,
	imperialAdvice = 27,
	blasphemousEntity = 28,
	ghostShip2 = 35,
	ghostShip3 = 36,
}
public enum ObstacleEventType
{
	blackMarketCape,
	imperialTradeLine,
	dockedRescueShip,
	rescueSignal,
	signalRecorder

}

public struct SpawnRequest
{
	public string eventName;
	public string sessionId;
	public string? rewardProfileId; //받아야할 리워드 프로파일 아이디를 제출함
	public Vector3 position;
}
public struct SpawnMonster
{
	public int sectorCount;
	public string monsterId; // 발칸 스폰1, 발칸 스폰 2, 발칸 스폰 3;
	public Vector3 position;
}
public class EventManager : MonoBehaviour
{
	public static EventManager Instance { get; private set; }


	[SerializeField] List<EventDialogueSO> eventSO;

	[SerializeField] public EventDialogue clickDialogue;
	[SerializeField] public ShipDialogue ShipDialogue;
	[SerializeField] public ShopDialogue ShopDialogue;

	[Header("이벤트 ui")]
	public GameObject Panel_EventUI;
	public Animator EventUIAnimator;

	[Header("이벤트 자료")]

	[SerializeField] Dictionary<EventType, EventDialogueSO> _eventDialogue = new Dictionary<EventType, EventDialogueSO>();
	[SerializeField] Dictionary<EventType, EventBase> _eventImplementation = new Dictionary<EventType, EventBase>();


	private Section _targetSection;
	public Section TargetSection { get { return _targetSection; } set { _targetSection = value; } }
	private Section _currentSection;
	public Section CurrentSection { get { return _currentSection; } }

	public Section currentEventSection;

	[Header("우주선 ui")]
	public GameObject DerelictShipObject;
	public GameObject DerelictShipUI;
	[Header("우주선 애니메이터")]
	public Animator ObstacleShipUIAnimator;
	[Header("우주선 UI text")]
	public TextMeshProUGUI DerelictShipUIText;

	public BlueSpot BlueSpot;

	[Header("교신 버튼")]
	public UnityEngine.UI.Button contactShip;
	public UnityEngine.UI.Button contactPlayer;
	private TextMeshProUGUI conShipText;
	private TextMeshProUGUI conPlayerText;

	[Header("블루스팟 타이머")]
	public GameObject TimerObject;
	public TextMeshProUGUI Timer;
	public TextMeshProUGUI ContactTimer;

	[Header("시그널 재밍 불리언")]
	public bool isAccept = false;

	[Header("아이템 SO")]
	public ItemData Cradit;
	[SerializeField] List<ItemData> WeaponItems;
	//일반 몬스터
	public event Action SpawnCommonMonster;

	// 이벤트 몬스터 - 스폰은 딕셔너리에 스트링 키에 값으로 집어 넣은뒤 EventBase에서 스폰

	public event Action<SpawnRequest> OnSpawnRequested;
	[SerializeField] Dictionary<int, SpawnMonster> OnMonsterGetSpawn = new Dictionary<int, SpawnMonster>();

	// 기본 스폰 이벤트 몬스터
	private List<Action> EventMonsterList = new List<Action>();
	public event Action ScavengerRaid;//스캐반저
	public event Action CorsairRaid;//커세어
	public event Action TurretMalfunction;//포탑
	public event Action BoreMinerSpawn;//보어 마이너
	public event Action ImperialMerchantGuildShip;//제국 상단선

	public event Action<SpawnMonster> OnMonsterSetSpawn;
	public List<string> spawnSet = new List<string>();
	public int currentMonsterIndex = 0;
	// 보스 몬스터
	public event Action<Vector3> BossMonsterSpawn1;
	public event Action<Vector3> BossMonsterSpawn2;
	public event Action<Vector3> BossMonsterSpawn3;

	public List<GameObject> SpawnStructures = new List<GameObject>();
	public GameObject UI_Shop;
	public GameObject Panel_Inven;
	public GameObject Panel_Upgrade;



	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		Init();
	}

	private void Update()
	{
		bool isLaunched = FighterManager.Instance.isLaunched;
		if (FieldManager.Instance.isBlueSpot != true)
		{
			if (isLaunched == false)
			{
				contactShip.gameObject.SetActive(true);
				contactPlayer.gameObject.SetActive(false);

			}
			if (isLaunched == true)
			{
				contactShip.gameObject.SetActive(false);
				contactPlayer.gameObject.SetActive(false);
			}
		}
		else
		{
			return;
		}
	}

	void Init()
	{
		//SO dictionary Assignment 
		for (int i = 0; i < eventSO.Count; i++)
		{
			_eventDialogue[eventSO[i].EventType] = eventSO[i];
		}

		SetEventDictionary();
		SetEventMonList();

		DerelictShipObject.SetActive(true);
		DerelictShipUI.SetActive(false);
		Panel_EventUI.SetActive(false);
		clickDialogue.init();
		clickDialogue.ButtonClose();
		conShipText = contactShip != null ? contactShip.GetComponentInChildren<TextMeshProUGUI>() : null;
		conPlayerText = contactPlayer != null ? contactPlayer.GetComponentInChildren<TextMeshProUGUI>() : null;
		DeletePlayerRemoveListener();
		DeleteShipRemoveListener();
		TimerObject.SetActive(false);

		contactShip.gameObject.SetActive(true);
		contactPlayer.gameObject.SetActive(false);

		ShipDialogue.DataBuild();
		ShopDialogue.Init();
		TimertextFind();
		GameOver.instance.OnOver += CloseAllEventUI;
	}

	public void ResetList()
	{
		if (FieldManager.Instance.SectorCount == 1)
		{
			spawnSet = new List<string> { "발칸 스폰 1", "발칸 스폰 2", "발칸 스폰 3", "제국 스폰 1", "림보 스폰 1" };
		}
		else if (FieldManager.Instance.SectorCount == 2)
		{
			spawnSet = new List<string> { "발칸 스폰 2", "제국 스폰 1", "림보 스폰 1", "림보 스폰 2", "림보 스폰 3", "제국 스폰 2" };
		}
		else if (FieldManager.Instance.SectorCount == 3)
		{
			spawnSet = new List<string> { "림보 스폰", "제국 스폰 3", "제국 스폰 4", "레티콘 스폰", "레티콘 스폰 2" };
		}
	}

	void SetEventDictionary()
	{
		_eventImplementation.Add(EventType.raidersWarning, new RaidersWarning());
		_eventImplementation.Add(EventType.surpriseInspection, new SurpriseInspection());
		_eventImplementation.Add(EventType.cleanupZone, new cleanupZone());
		_eventImplementation.Add(EventType.mysteriousSignal, new mysteriousSignal());
		_eventImplementation.Add(EventType.decoy, new decoy());
		_eventImplementation.Add(EventType.suspiciousWeaponTrade, new SuspiciousWeaponTrade());
		_eventImplementation.Add(EventType.repairScam, new repairScam());
		_eventImplementation.Add(EventType.ruleAnnouncement, new ruleAnnouncement());
		_eventImplementation.Add(EventType.covertSupplyShip, new covertSupplyShip());
		_eventImplementation.Add(EventType.covertSupplyShip2, new covertSupplyShip2());
		_eventImplementation.Add(EventType.covertSupplyShip3, new covertSupplyShip3());
		_eventImplementation.Add(EventType.imperialTradeLine, new ImperialTradeLine());
		_eventImplementation.Add(EventType.blackMarket, new BlackMarket());
		_eventImplementation.Add(EventType.dockedRescueShip, new DockedRescueShip());
		_eventImplementation.Add(EventType.signalRecorder, new SignalRecorder());
		_eventImplementation.Add(EventType.rescueSignal, new RescueSignal());
		_eventImplementation.Add(EventType.ghostRescueSignal, new ghostRescueSignal());
		_eventImplementation.Add(EventType.ImperialControl, new ImperialControl());
		_eventImplementation.Add(EventType.trackingSignal, new trackingSignal());
		_eventImplementation.Add(EventType.imperialScientist, new imperialScientist());
		_eventImplementation.Add(EventType.imperialHelpRequest, new imperialHelpRequest());
		_eventImplementation.Add(EventType.imperialSentry, new imperialSentry());
		_eventImplementation.Add(EventType.ghostSignal, new ghostSignal());
		_eventImplementation.Add(EventType.ghostRescueSignal2, new ghostRescueSignal2());
		_eventImplementation.Add(EventType.limboWarning, new limboWarning());
		_eventImplementation.Add(EventType.imperialAdvice, new imperialAdvice());
		_eventImplementation.Add(EventType.blasphemousEntity, new blasphemousEntity());
		_eventImplementation.Add(EventType.dockedRescueShip2,new dockedRescueShip2());
		_eventImplementation.Add(EventType.dockedCargoShip,new dockedCargoShip());
		_eventImplementation.Add(EventType.ghostShip,new ghostShip());
		_eventImplementation.Add(EventType.ghostShip2, new ghostShip2());
		_eventImplementation.Add(EventType.ghostShip3, new ghostShip3());
		_eventImplementation.Add(EventType.covertSupplyShip4,new covertSupplyShip4());
		_eventImplementation.Add(EventType.covertSupplyShip5, new covertSupplyShip5());
		_eventImplementation.Add(EventType.covertSupplyShip6, new covertSupplyShip6());
	}
	public void TimertextFind()
	{
		if (TimerObject != null)
		{
			Timer = TimerObject.transform.Find("TimerText").GetComponent<TextMeshProUGUI>();
			ContactTimer = TimerObject.transform.Find("ContextTimer").GetComponent<TextMeshProUGUI>();
		}
	}
	public Action RandomEventMon()
	{
		int random = UnityEngine.Random.Range(0, EventMonsterList.Count);
		return EventMonsterList[random];
	}

	public ItemData FindItemData(string ItemName)
	{
		foreach (var Weapon in WeaponItems)
		{
			if (Weapon.itemName == ItemName)
			{
				return Weapon;
			}
			else
			{
				continue;
			}
		}
		int random = UnityEngine.Random.Range(0, WeaponItems.Count);
		return WeaponItems[random];
	}

	public void RequestSpawn(string spawnSetId, string rewardProfileId)
	{
		SpawnRequest req = new SpawnRequest
		{
			eventName = spawnSetId,
			rewardProfileId = rewardProfileId,
			sessionId = Guid.NewGuid().ToString(),
			position = currentEventSection.gameObject.transform.position
		};

		OnSpawnRequested?.Invoke(req);
	}

	public void MonsterSetSpawn(string monsterSetId, Vector3 position)
	{
		SpawnMonster spm = new SpawnMonster
		{
			sectorCount = FieldManager.Instance.SectorCount,
			monsterId = monsterSetId,
			position = position,

		};
		OnMonsterSetSpawn?.Invoke(spm);
	}
	public int OutIndex()
	{
		int current = currentMonsterIndex;
		currentMonsterIndex++;
		if (currentMonsterIndex == spawnSet.Count)
		{
			currentMonsterIndex = 0;
		}
		return current;
	}
	public void spawnMonsterEvent(int i, Vector3 position)
	{
		ResetList();
		int index = i;
		string spawnMonsterRandom = spawnSet[i];
		MonsterSetSpawn(spawnMonsterRandom, position);
	}
	void SetEventMonList() // 기본 이벤트 스폰 몬스터
	{
		EventMonsterList.Add(ScavengerRaid);
		EventMonsterList.Add(CorsairRaid);
		EventMonsterList.Add(TurretMalfunction);
		EventMonsterList.Add(BoreMinerSpawn);
		EventMonsterList.Add(ImperialMerchantGuildShip);
	}
	public void ShowUI(EventType eventType, Section section) //open 상태를 애니메이터의 디폴트값으로 두어서 키기만 해도 실행됨
	{
		EventDialogueSO eventSO = _eventDialogue[eventType];
		clickDialogue.ChangeSetting(eventSO.EventType, eventSO.Dialogue, eventSO, _eventImplementation[eventType]);
		clickDialogue.ApplyDialogue();
		Panel_EventUI.SetActive(true);
		clickDialogue.ButtonClose();
		currentEventSection = section;
		if (eventSO.spawnStructure != null && section.spawnStructure == false)
		{
			Vector3 SpawnPos = FieldManager.Instance.CalculatePos(section, 90, 120, -15);
			GameObject structure = Instantiate(eventSO.spawnStructure, SpawnPos, Quaternion.identity);
			SpawnStructures.Add(structure);
			section.spawnStructure = true;
		}
	}

	public void DestroyStructures()
	{
		foreach (GameObject structure in SpawnStructures)
		{
			Destroy(structure);
		}
		SpawnStructures.Clear();
	}

	public void CloseEventUI()
	{
		if (Panel_EventUI.activeSelf)
		{
			StartCoroutine(CloseEventDialogueUI());
		}
	}

	IEnumerator CloseEventDialogueUI()
	{
		EventUIAnimator.SetBool("Close", true);
		yield return new WaitUntil(() => EventUIAnimator.GetCurrentAnimatorStateInfo(0).IsName("Close"));
		yield return new WaitUntil(() => EventUIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
		clickDialogue.LivingDialogue();
		Panel_EventUI.SetActive(false);
	}

	public void CloseAllEventUI()
	{
		DerelictShipUI.SetActive(false);
		Panel_EventUI.SetActive(false);
		contactShip.gameObject.SetActive(false);
		contactPlayer.gameObject.SetActive(false);
		TimerObject.SetActive(false);
		ShopDialogue.CloseShop();
	}

	public void ChangeCurrentSection(Section section)
	{
		_currentSection = section;
	}
	public void EnableContactShip(Action action = null)
	{
		if (action != null)
		{
			contactShip.onClick.AddListener(action.Invoke);
		}
		contactShip.interactable = true;
		conShipText.color = contactShip.colors.normalColor;
	}
	public void EnableContactPlayer(Action action = null)
	{
		if (action != null)
		{
			contactPlayer.onClick.AddListener(action.Invoke);
		}
		contactPlayer.interactable = true;
		conPlayerText.color = contactPlayer.colors.normalColor;
	}

	public void DeleteShipRemoveListener() // 교신버튼 에드리스너 삭제
	{
		if (contactShip.onClick != null && FieldManager.Instance?.clearBluespotMission == false)
		{
			contactShip?.onClick.RemoveAllListeners();
		}
		contactShip.interactable = false;
		conShipText.color = contactShip.colors.disabledColor;

	}
	public void DeletePlayerRemoveListener() // 교신버튼 에드리스너 삭제
	{
		if (contactPlayer.onClick != null)
		{
			contactPlayer?.onClick.RemoveAllListeners();
		}
		contactPlayer.interactable = false;
		conPlayerText.color = contactPlayer.colors.disabledColor;
	}

	public void CloseObstacleShipUI()
	{
		StartCoroutine(CloseShipUI());
		ShipDialogue.ActiveButton(false);
	}
	IEnumerator CloseShipUI()
	{
		ObstacleShipUIAnimator.SetBool("Close", true);

		yield return new WaitUntil(() => ObstacleShipUIAnimator.GetCurrentAnimatorStateInfo(0).IsName("Close"));
		yield return new WaitUntil(() => ObstacleShipUIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
		DerelictShipUI.SetActive(false);
	}
	public void ReAddlistender()
	{
		if (FieldManager.Instance.isBlueSpot == false)
		{
			contactShip.onClick.AddListener(() => ShowUI(currentEventSection.eventType, currentEventSection));
			EnableContactShip();
		}
		else
		{
			contactShip.onClick.AddListener(() => FieldManager.Instance.blueSpot.OpenBlueSpotUI());
			contactShip.onClick.AddListener(() => contactShip.gameObject.SetActive(false));
			EnableContactShip();
		}
	}

	public void BossSpawn(Vector3 pos)
	{
		switch (FieldManager.Instance.SectorCount)
		{
			case 1:
				BossMonsterSpawn1?.Invoke(pos);
				break;
			case 2:
				BossMonsterSpawn2?.Invoke(pos);
				break;
			case 3:
				BossMonsterSpawn3?.Invoke(pos);
				break;
			default:
				break;

		}
	}

}
