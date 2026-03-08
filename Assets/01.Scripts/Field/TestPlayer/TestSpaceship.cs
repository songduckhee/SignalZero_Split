using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestSpaceship : MonoBehaviour
{
	[SerializeField] private FieldManager fieldManager;
	[SerializeField] private float moveSpeed = 5f;
	private Section _currentSection;

	private float speed = 4;
	private Vector3 _targetPos;
	private Section _targetSection;
	private bool _isMoving;
	private int spawnMonCount = 3;

	private Queue<Section> unspawnedSection;

	private void Awake()
	{
		unspawnedSection = new Queue<Section>();
	}


	void Update()
	{
		if (Input.GetMouseButtonDown(0) && !_isMoving)
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, 10000f))
			{
				Section section = hit.collider.GetComponent<Section>();
				if (section != null)
				{
					EventManager.Instance.TargetSection = section;
					_targetSection = section;
					_targetPos = section.transform.position; 
					_isMoving = true;
				}
				Vector3 shootingPos = transform.position + new Vector3(0f, -2f, 0f);
				Vector3 targetPos = _targetPos;
				Vector3 direction = (targetPos - shootingPos).normalized;
				float distance = Vector3.Distance(transform.position, targetPos);
				Debug.DrawRay(shootingPos, direction * distance, Color.blue, 20.0f);
				RaycastHit[] hitByRaySection = Physics.RaycastAll(shootingPos, direction, distance).OrderBy(hit =>  hit.distance).ToArray();
				//if(hitByRaySection.Length == 0)
				//{
				//	return;
				//}
				Debug.Log("Raycast test start");
				Debug.Log($"레이가 닿은 콜라이더의수 = {hitByRaySection.Length}");
				//bool didHit = Physics.Raycast(shootingPos, direction, out hit2, distance);
				//Debug.Log ($"_targetPos = {targetPos} , playerPos = {shootingPos}");
				//Debug.Log($"didHit={didHit}, from={shootingPos}, dir={direction}, dist={distance}");
				//if (Physics.Raycast(shootingPos, direction, out hit, distance))
				//{
				//	Debug.Log($"HIT: {hit.collider.name}");
				//	Debug.DrawLine(shootingPos, hit.point, Color.red, 2f);
				//}
				for (int i = 0; i < hitByRaySection.Length; i++)
				{
					Section inspectedSection = hitByRaySection[i].collider.GetComponent<Section>();
					if (inspectedSection == null)
						continue;
					if(inspectedSection.monsterSpawned == false && inspectedSection.Type == SectionType.Monster)
					{
						unspawnedSection.Enqueue(inspectedSection);
					}
				}
				for (int q = 0; q < spawnMonCount; q++)
				{
					if(unspawnedSection.Count == 0)
						break;
					
						Section hitSection = unspawnedSection.Dequeue();

						Debug.Log($"경로상에 있는 스폰 대기중인 몬스터섹션{hitSection}{q} 실행!");
						hitSection.RunArrivalMonster(ShipMover.Instance.gameObject);
					
					
				}
				unspawnedSection.Clear();
			}
		}

		if (_isMoving)
		{
			transform.position = Vector3.MoveTowards(transform.position, _targetPos, moveSpeed * Time.deltaTime * speed);

			Section sectionUnder = FieldManager.Instance.GetSectionAtPosition(transform.position);
			if (sectionUnder != null && sectionUnder != _currentSection)
			{
				_currentSection = sectionUnder;
				EventManager.Instance.ChangeCurrentSection(sectionUnder);

				// 몬스터 섹션이면 바로 실행
				if (_currentSection.Type == SectionType.Monster && _currentSection.monsterSpawned == false)
				{
					Debug.Log("몬스터섹션이 닿았고 아직 스폰이 안되었을 때 실행!");
					_currentSection.RunArrivalMonster(ShipMover.Instance.gameObject);
				}
				if (EventManager.Instance.CurrentSection.Type == SectionType.Event)
				{
					sectionUnder.monsterSpawned = true;
					Debug.Log("닿았을때 이벤트몬스터 스폰!");
				}
			}


			if ((transform.position - _targetPos).sqrMagnitude < 0.001f)
			{
				_isMoving = false;
				if (_targetSection != null)
				{
					_targetSection.RunArrivalEvent();
				}
			}
		}
	}


}
