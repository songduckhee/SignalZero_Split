using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface BossDie
{
	event Action OnDie;
}

public class FieldManager : MonoBehaviour
{
	[Header("필드 프리팹들")]
	[SerializeField]
	private GameObject fieldPrefab;

	[Header("그리드 레이아웃 설정")]
	[SerializeField]
	private GridLayoutSettings layoutSettings = new GridLayoutSettings();
	public ObstacleManager obstacleManager;

	[Header("섹션 배열")]
	[SerializeField] public Section[,] sections;

	[Header("블루스팟 열위치")]
	[SerializeField] private int blueSpotMinRow = 5;
	[Header("이동 목적지 블루스팟 섹션 위치  != 섹션내 블루스팟")]
	private Vector3 blueSpotPos = new Vector3(2500f, 0, 2500f);

	[Header("섹션내에 블루스팟위치랑 섹션스크립트")]
	public Section blueSpotSection;

	[Header("블루스팟이 있는 섹션의 위치")]
	[SerializeField] private Vector2Int sectionLocation;

	[Header("몬스터랑 이벤트타입 갯수")]
	[SerializeField] private int monsterSectionCount = 40;
	[SerializeField] private int eventSectionCount = 10; //10

	[Header("현재 섹터 숫자")]
	[SerializeField] private int sectorCount = 1;
	public int SectorCount
	{
		get { return sectorCount; }
		set
		{
			sectorCount = Mathf.Clamp(value, 1, 3);
		}
	}

	// 몬스터섹션에서 나오는 몬스터
	public event Action<Vector3> MonsterSpawn;

	//예전 스폰보스몬스터(구)
	public event Action SpawnBoss;
	public event Action Section2Boss;
	public event Action Section3Boss;

	[Header("싱글톤")]
	public static FieldManager Instance;

	public List<GameObject> SpawnedFields
	{
		get { return _spawnedFields; }
	}
	[Header("예비")]
	public GameObject spaceShip;

	[Header("스카이박스 메테리얼")]
	public Material Sector1Material;
	public Material Sector2Material;
	public Material Sector3Material;

	[Header("블루스팟 이동코루틴")]
	public Coroutine spaceshipTransfer;
	[Header("블루스팟 UI")]
	public BlueSpot blueSpot;
	[Header("블루스팟 구조물")]
	public List<GameObject> blueSpotStructure;
	private GameObject _spawnedStructure;
	private readonly List<GameObject> _spawnedFields = new List<GameObject>();
	private FieldSpawner spawner;
	private EventSelector eventAttachment = new EventSelector();
	private List<Section> EventTypeSection = new List<Section>();

	[Header("블루스팟 불값")]
	public bool isBlueSpot;
	public bool clearBluespotMission = false;
	public bool isBossSpawned = false;

	public event Action inBlueSpot;
	public event Action SectorChange; // 섹터가 변했을때

	public event Action StartInit;
	public event Action EndInit;

	private void Awake()
	{
		Instance = this;
		spawner = new FieldSpawner(fieldPrefab, layoutSettings);
	}


	private void Start()
	{
		Init();
	}

	public void Init()
	{
		ClearFields();

		List<GameObject> spawned = spawner.SpawnAll(transform);

		_spawnedFields.Clear();
		_spawnedFields.AddRange(spawned);

		obstacleManager.Init();

		int width = layoutSettings.Width;
		int height = layoutSettings.Height;

		sections = new Section[width, height];


		//스폰된 섹션들을 [0,0] ~ [9,9] 사이 배열로 _sections에 분류 추가
		for (int i = 0; i < spawned.Count; i++)
		{
			GameObject spawnSection = spawned[i];
			Section section = spawnSection.GetComponent<Section>();

			Vector2Int gridPos = layoutSettings.GetGridPosition(i);
			section.init(i, gridPos);
			sections[gridPos.x, gridPos.y] = section;
			section.SetType(SectionType.None);
			section.SetEventType(EventType.none);
		}

		//블루스팟, 이벤트 타입 정해주는 함수 실행
		PlaceBlueSpot();
		PlaceMonsterAndEventSections();
		SetEventType();

		//보스 죽었을때 실행할 이벤트 구독
		if (StartInit != null)
		{
			StartInit.Invoke();
		}
		if (EndInit != null)
		{
			EndInit.Invoke();
		}
		if (FindAnyObjectByType<ShipMover>() != null)
		{
			spaceShip = FindAnyObjectByType<ShipMover>().gameObject;
		}
		//blueSpot.blueSpotButton.onClick.AddListener(ResetSector);
		//이미 버튼에 addListener가 있어서 주석처리
		ResetBool();
		passedTime = 0;
	}

	public void ResetSector()
	{
		ResetBool();
		SectorCount++;
		DestroyBlueSpotStructure();
		ResetSectionType();
		PlaceBlueSpot();
		PlaceMonsterAndEventSections();
		SetEventType();
		ShipMover.Instance.MoveShipToStartPos();
		blueSpot.CloseBlueSpotUI();
		SectorChange?.Invoke();
		EventManager.Instance.DestroyStructures();
		EventManager.Instance.ShopDialogue.CloseShop();
		if (sectorCount == 2)
		{
			RenderSettings.skybox = Sector2Material;
			DynamicGI.UpdateEnvironment();
		}
		else if (sectorCount == 3)
		{
			RenderSettings.skybox = Sector3Material;
			DynamicGI.UpdateEnvironment();
		}
		EventManager.Instance.DeleteShipRemoveListener();
		EventManager.Instance.DeletePlayerRemoveListener();
	}

	private void ClearFields()
	{
		_spawnedFields.Clear();

		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			Transform child = transform.GetChild(i);
			Destroy(child.gameObject);
		}
	}

	private void PlaceBlueSpot()
	{
		List<Section> candidates = new List<Section>();

		foreach (GameObject field in _spawnedFields)
		{
			Section section = field.GetComponent<Section>();
			if (section == null)
			{
				continue;
			}

			if (section.gridPos.y >= blueSpotMinRow)
			{
				candidates.Add(section);
			}
		}

		if (candidates.Count == 0)
		{
			Debug.LogWarning("블루스팟을 놓을 섹션 후보가 없습니다.");
			return;
		}

		Section blueSection = candidates[UnityEngine.Random.Range(0, candidates.Count)];
		blueSection.hasBlueSpot = true;
		blueSection.SetType(SectionType.BlueSpot);
		sectionLocation = blueSection.gridPos;
		blueSpotSection = blueSection;
		Vector3 structurePos = new Vector3();
		if (SectorCount == 1 || SectorCount == 2)
		{
			structurePos = CalculatePos(blueSpotSection, 90, 120, -15);
		}
		else
		{
			structurePos = CalculatePos(blueSpotSection, 9, 10, -15);
		}

			GameObject bluespotstructure = Instantiate(blueSpotStructure[sectorCount - 1], structurePos, Quaternion.identity);
		if(SectorCount == 1 || SectorCount == 2)
		{
			bluespotstructure.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
			bluespotstructure.transform.localScale = Vector3.one * 0.15f;
		}
			_spawnedStructure = bluespotstructure;
	}

	private void DestroyBlueSpotStructure()
	{
		if (_spawnedStructure != null)
		{
			Destroy(_spawnedStructure);
		}
	}

	private void PlaceMonsterAndEventSections() //섹션을 생성할때 이벤트 섹션이랑 몬스터 섹션을 정해줘야되서 필드에 만듬
	{
		List<Section> candidates = new List<Section>();

		foreach (GameObject field in _spawnedFields)
		{
			Section section = field.GetComponent<Section>();

			if (section == null || section.hasBlueSpot)
			{
				continue;
			}
			if (section.gridPos == new Vector2Int(5, 0))
			{
				continue;
			}
			candidates.Add(section); // 블루스팟만뺀 섹션들 모음
		}

		Shuffle(candidates);

		int index = 0;
		//이벤트 섹션 먼저
		EventTypeSection.Clear();
		for (int i = 0; i < eventSectionCount && index < candidates.Count; i++)
		{
			candidates[index].SetType(SectionType.Event);
			EventTypeSection.Add(candidates[index]);
			index++;
		}
		//몬스터 섹션 (인덱스를 넣은 이유는 앞에 끝난 eventSection들의 인덱스를 빼고 넣으려고
		for (int i = 0; i < monsterSectionCount && index < candidates.Count; i++)
		{
			candidates[index].SetType(SectionType.Monster);
			candidates[index].SetEventType(EventType.none);
			index++;
		}
	}
	private void ResetSectionType()
	{
		foreach (GameObject field in _spawnedFields)
		{
			Section section = field.GetComponent<Section>();
			if (section == null)
			{
				continue;
			}
			section.SetType(SectionType.None);
			section.SetEventType(EventType.none);

			section.ResetBool();
		}
	}
	private void SetEventType()
	{
		if (EventTypeSection == null)
		{
			Debug.Log("이벤트타입섹션이 비어있음");
		}
		else
		{
			Shuffle(EventTypeSection);
		}
		Queue<int> sectorValue = new Queue<int>();
		int[] Sector1value = { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
		int[] Sector2value = { 14,17, 18, 19, 20, 21, 22, 23, 24,29,30,31,32,33,34,34 };
		int[] Sector3value = { 14, 25, 26, 27, 28,32,33,34,35,36,32,33,34,35,36,33,34 };
		if (sectorCount == 1)
		{
			foreach (int value in Sector1value)
			{
				sectorValue.Enqueue(value);
			}
		}
		else if (sectorCount == 2)
		{
			foreach (int value in Sector2value)
			{
				sectorValue.Enqueue(value);
			}
		}
		else
		{
			foreach (int value in Sector3value)
			{
				sectorValue.Enqueue(value);
			}
		}
		for (int i = 0; i < EventTypeSection.Count; i++)
		{

			int value = sectorValue.Dequeue();

			EventTypeSection[i].eventType = (EventType)value;
			sectorValue.Enqueue(value);
		}
	}

	private void Shuffle<T>(List<T> list) // 이벤트 구할때 랜덤으로 리스트를 뒤섞어주기위해서 만듬
	{
		for (int i = 0; i < list.Count; i++)
		{
			int randomIndex = UnityEngine.Random.Range(i, list.Count);
			(list[i], list[randomIndex]) = (list[randomIndex], list[i]);
		}
	}

	public Section GetSectionAtPosition(Vector3 position)
	{
		float tileSize = 200f;
		//미니맵 좌표 기준으로 10x10의 값으로 섹션의 포지션 값을 구하는 함수
		int xIndex = Mathf.RoundToInt(position.x / tileSize) + 5; // 만약 섹션 포지션 x가 -1000일때(맨끝쪽) /200 = -5. 여기서 +5하게되면 0 
		int zIndex = Mathf.RoundToInt(position.z / tileSize);

		if (xIndex < 0 || xIndex >= sections.GetLength(0) ||
			zIndex < 0 || zIndex >= sections.GetLength(1))
			return null;

		return sections[xIndex, zIndex];
	}

	public void InvokeMonsterSpawn(Vector3 section)
	{
		MonsterSpawn?.Invoke(section);
	}


	[SerializeField] private float battleDuration = 120f;
	private float passedTime;

	IEnumerator BlueSpotTeleport()
	{
		Debug.Log("블루스팟 보스몬스터 출현");
		//EventManager.Instance.contactShip.interactable = false;
		EventManager.Instance.DeleteShipRemoveListener();
		EventManager.Instance.TimerObject.SetActive(true);

		blueSpotSection.accessBlueSpot = true;


		//Vector3 structurePos = CalculatePos(40, 50, 6);
		//GameObject bluespotstructure = Instantiate(blueSpotStructure, structurePos, Quaternion.identity);
		//bluespotstructure.transform.localScale = Vector3.one * 0.1f;
		//Renderer renderer = bluespotstructure.GetComponent<Renderer>();
		//structureMaterial = renderer.material;
		if (isBossSpawned == false)
		{
			EventManager.Instance.BossSpawn(blueSpotSection.transform.position);
			isBossSpawned = true;
		}

		//StartCoroutine(materialAlphaChange(structureMaterial, 1, 20f));
		while (passedTime <= battleDuration && ShipMover.Instance.shipBodyDestoyed == false)
		{
			passedTime += Time.deltaTime;
			TimeSpan timeSpan = TimeSpan.FromSeconds(passedTime);
			float remainingTime = battleDuration - passedTime;
			TimeSpan remainTimeSpan = TimeSpan.FromSeconds(remainingTime);
			int hundredth = timeSpan.Milliseconds / 10;
			EventManager.Instance.Timer.text = string.Format("{0}:{1:00}:{2:00}", timeSpan.Minutes, timeSpan.Seconds, hundredth);
			if (EventManager.Instance.ContactTimer != null)
			{
				EventManager.Instance.ContactTimer.text = string.Format("{0}:{1:00}", remainTimeSpan.Minutes, remainTimeSpan.Seconds);
			}
			// TODO: UI 시간 갱신
			yield return null;
		}
		ActivateTransfer();
	}
	IEnumerator materialAlphaChange(Material material, float targetAlpha, float duration)
	{
		Color color = material.color;
		float startAlpha = color.a;
		float elapsed = 0f;
		color.a = startAlpha;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
			material.color = color;
			yield return null;

		}
		color.a = targetAlpha;
		material.color = color;

	}

	public Vector3 CalculatePos(Section section, int min, int max, int y)
	{
		List<int> value = new List<int>();
		for (int i = -max; i < max + 1; i++)
		{
			if (i > -min && i < min)
				continue;
			value.Add(i);
		}
		int x = UnityEngine.Random.Range(0, value.Count);
		int z = UnityEngine.Random.Range(0, value.Count);

		Vector3 sectionPos = section.gameObject.transform.position;
		return new Vector3(sectionPos.x + x, sectionPos.y + y, sectionPos.z + z);
	}

	public void StartTransfertoBluespot() // 블루스팟에 들어갔을때 실행되는 함수
	{
		spaceshipTransfer = StartCoroutine(BlueSpotTeleport());
		
	}
	public void StopTransfer()
	{
		if (spaceshipTransfer != null)
		{
			StopCoroutine(spaceshipTransfer);
		}
		//보스가 사라지거나 아니면 그냥 그대로 남아서 블루스팟 안에 갖혀있음
		//카운터가 사라짐
	}

	public void BossDie()
	{
		ActivateTransfer();
	}
	public void OnTransferEvent() //
	{
		passedTime = 0;
		isBossSpawned = false;
		isBlueSpot = true;
		blueSpot?.OpenBlueSpotUI();
		SoundManager.Instance.PlayBGM(BGMType.BlueSpot);
		EventManager.Instance.TimerObject.SetActive(false);
		inBlueSpot?.Invoke();
		if (ShipMover.Instance.shipBodyDestoyed != true)
		{
			ShipMover.Instance.gameObject.transform.position = blueSpotPos + new Vector3(0f, 2f, 0f); //버튼이 하는 일
			ShipMover.Instance.gameObject.transform.rotation = Quaternion.identity;
			EventManager.Instance.DeleteShipRemoveListener();
			EventManager.Instance.DeletePlayerRemoveListener();
		}
		else
		{
			Debug.Log("spaceShipDestroy!");
		}

	}
	public void ActivateTransfer()
	{
		if (ShipMover.Instance.shipBodyDestoyed == true)
		{
			return;
		}
		clearBluespotMission = true;
		StopTransfer();
		EventManager.Instance.contactShip.onClick.AddListener(OnTransferEvent);
		EventManager.Instance.EnableContactShip();
	}

	public void ResetBool()
	{
		isBlueSpot = false;
		clearBluespotMission = false;
		isBossSpawned = false;
	}
}

