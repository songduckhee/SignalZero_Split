using JetBrains.Annotations;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Section : MonoBehaviour
{
	[SerializeField] private float spawnRadius = 30f;
	[SerializeField] private bool _isVisited;
	public bool hasBlueSpot;
	public bool isVisited
	{
		get { return _isVisited; }
		set { _isVisited = value; }
	}
	[SerializeField] private Vector3 centerPos;
	[Header("위치좌표")]
	public Vector2Int gridPos;
	[Header("섹션인덱스(넘버)")]
	[SerializeField] private int _index;
	public int Index
	{
		get { return _index; }
	}

	public bool hasEvent;
	public bool monsterSpawned;
	public bool EventMonsterSpawned;

	public bool clearEvent = false;
	public bool accessBlueSpot = false;
	public bool spawnStructure = false;

	[Header("섹션 타입")]
	[SerializeField] private SectionType _type;
	public SectionType Type { get { return _type; } }
	[SerializeField] private EventType _eventType;
	public EventType eventType
	{
		get { return _eventType; }
		set { _eventType = value; }
	}

	Coroutine waitCor;
	Coroutine CheckCor;
	Coroutine MonsterCor;
	Coroutine CheckMon;



	public void SetType(SectionType type)
	{
		_type = type;
	}

	public void SetEventType(EventType type)
	{
		_eventType = type;
	}
	public void SetEventTypeRandom()
	{
		int i = UnityEngine.Random.Range(1, System.Enum.GetValues(typeof(EventType)).Length) + 1;
		_eventType = (EventType)i;
	}

	private void OnEnable()
	{
		centerPos = transform.position;
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player") && FighterManager.Instance.isLaunched == true && startMonsterSpawn == false)
		{
			GameObject player = (GameObject)collision.gameObject;
			_isVisited = true;
			if (Type == SectionType.Monster && monsterSpawned != true)
			{
				Debug.Log("플레이어 트리거 엔터 몬스터 전투시작");
				RunArrivalMonster(player);

			}
		}
	}
	public void RunArrivalEvent()
	{
		// 실제 이벤트 내용 여기서 실행
		switch (Type)
		{
			case SectionType.BlueSpot:
				Debug.Log($"{name} 블루스팟 이벤트 실행!");
				if (FieldManager.Instance.clearBluespotMission == false)
				{
					EventManager.Instance.contactShip.onClick.RemoveAllListeners();
					EventManager.Instance.contactShip.onClick.AddListener(() => FieldManager.Instance.StartTransfertoBluespot());
					EventManager.Instance.EnableContactShip();
				}
				else
				{
					EventManager.Instance.contactShip.onClick.RemoveAllListeners();
					EventManager.Instance.contactShip.onClick.AddListener(() => FieldManager.Instance.OnTransferEvent());
					EventManager.Instance.EnableContactShip();
				}

				//FieldManager.Instance.StartTransfertoBluespot();
				//EventManager.Instance.InvokeBossMon();


				// 블루스팟 전용 처리
				break;

			case SectionType.Event:

				EventManager.Instance.DeleteShipRemoveListener();
				if (clearEvent == false)
				{
					waitCor = StartCoroutine(waitShip());
					EventManager.Instance.currentEventSection = this;

					Debug.Log($"{name} {eventType} 이벤트 실행!");
				}
				// 일반 이벤트 처리
				break;

			case SectionType.Monster:
				EventManager.Instance.DeleteShipRemoveListener();
				break;

			case SectionType.None:
			default:
				EventManager.Instance.DeleteShipRemoveListener();
				// 아무 일도 없음
				break;
		}
	}

	IEnumerator waitShip()
	{
		while (FighterManager.Instance.isLaunched != false)
		{
			if (EventManager.Instance.CurrentSection != EventManager.Instance.TargetSection)
			{
				Debug.Log($"currentSection != TargetSection");
				EventManager.Instance.DeleteShipRemoveListener();

				yield break;
			}

			yield return null;
		}

		//TODO: 호위기가 안으로 들어왔을때 UI 띄우기
		if (EventManager.Instance.contactShip.onClick != null)
		{
			EventManager.Instance.contactShip.onClick.RemoveAllListeners();
		}
		EventManager.Instance.contactShip.onClick.AddListener(() => EventManager.Instance.ShowUI(eventType, this));
		EventManager.Instance.EnableContactShip();
		//EventManager.Instance.ShowUI(eventType,this);
		waitCor = StartCoroutine(waitCheckOutsideEscort());

	}
	IEnumerator waitCheckOutsideEscort()
	{
		while (FighterManager.Instance.isLaunched == false)// 안에있을때 
		{
			if (EventManager.Instance.CurrentSection != EventManager.Instance.TargetSection)
			{
				EventManager.Instance.DeleteShipRemoveListener();
				//EventManager.Instance.CloseAllEventUI();
				yield break;
			}

			yield return null;
		}
		Debug.Log("isEscortInside == true"); // 밖으로 나갔을때
											 //EventManager.Instance.currentEventSection = null;
		EventManager.Instance.CloseEventUI();

		if (clearEvent != true)
		{
			waitCor = StartCoroutine(waitShip());
		}
	}

	public void init(int index, Vector2Int pos)
	{
		_index = index;
		gridPos = pos;
	}
	bool startMonsterSpawn = false;
	public void RunArrivalMonster(GameObject Target)
	{
		if (Type == SectionType.Monster && monsterSpawned == false)
		{

			//FieldManager.Instance.InvokeMonsterSpawn(this.transform.position);
			MonsterCor = StartCoroutine(CheckMonster(Target));
			// 몬스터 소환/전투 시작
			// 몬스터 섹션에 들어왔고 계속 들어와있는지 아닌지
		}
	}

	IEnumerator CheckMonster(GameObject target)
	{
		yield return new WaitForSecondsRealtime(2.7f);
		Debug.Log($"{name} 몬스터 스폰 타이머 5초 지남!");
		Section section = FieldManager.Instance.GetSectionAtPosition(target.transform.position);
		if (section != this)
		{
			yield break;
		}
		int index = EventManager.Instance.OutIndex();
		//SpawnSquare(this.centerPos);
		if(monsterSpawned != true)
		{
			EventManager.Instance.spawnMonsterEvent(index, this.centerPos);
			Debug.Log($"{name} 몬스터 스폰!");

			monsterSpawned = true;
			CheckMon = StartCoroutine(RespawnMon(index, this.centerPos));
			Debug.Log($"[{index}]");
		}
		else
		{
			yield break;
		}

			while (true)
			{
				Section section2 = FieldManager.Instance.GetSectionAtPosition(target.transform.position);
				if (section2 != this)
				{
					Debug.Log("나갔다");
					StopCoroutine(CheckMon);
					yield break;
				}
				yield return null;
			}
	}
	IEnumerator RespawnMon(int index, Vector3 pos)
	{
		Debug.Log($"{name} 재스폰 시작");
		for (int i = 0; i < 2; i++)
		{
			yield return new WaitForSeconds(15f);
			Debug.Log($"{name} 15초뒤 스폰");
			//SpawnSquare(this.centerPos);
			EventManager.Instance.spawnMonsterEvent(index, this.centerPos);//15초뒤 스폰
		}
	}

	public void SpawnSquare(Vector3 pos)
	{
		GameObject square = GameObject.CreatePrimitive(PrimitiveType.Cube);
		square.transform.position = pos;
		square.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void SetEventTrue()
	{
		clearEvent = true;
	}

	public void ResetBool()
	{
		hasEvent = false;
		clearEvent = false;

		monsterSpawned = false;
		EventMonsterSpawned = false;


		accessBlueSpot = false;
		spawnStructure = false;

	}
}
