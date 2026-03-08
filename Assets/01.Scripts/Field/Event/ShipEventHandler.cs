using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipEventHandler
{
	private Vector3 _targetPos;
	private Section _targetSection;
	private Queue<Section> unspawnedSection;
	private int spawnMonCount = 3;


	public ShipEventHandler()
	{
		unspawnedSection = new Queue<Section>();
	}

	public void StartEventHandler(int x, int y, Transform transform) // StartEventHandler(x,y,this.transform)
	{
		Section section = FieldManager.Instance.sections[x, y];
		if (section != null)
		{
			EventManager.Instance.TargetSection = section;
			EventManager.Instance.currentEventSection = section;
			_targetSection = section;
			_targetPos = section.transform.position;
			// 높이 고정
		}
		else
		{
			Debug.Log("eventmanager targetSection is null!");
		}
			Vector3 shootingPos = transform.position + new Vector3(0f, -2f, 0f);
		Vector3 targetPos = _targetPos;
		Vector3 direction = (targetPos - shootingPos).normalized;
		float distance = Vector3.Distance(shootingPos, targetPos);
		Debug.DrawRay(shootingPos, direction * distance, Color.blue, 20.0f);
		RaycastHit[] hitByRaySection = Physics.RaycastAll(shootingPos, direction, distance);
		Debug.Log("Raycast test start");
		for (int i = 0; i < hitByRaySection.Length; i++)
		{
			Section inspectedSection = hitByRaySection[i].collider.GetComponent<Section>();
			if (inspectedSection == null)
				continue;
			if (inspectedSection.monsterSpawned == false && inspectedSection.Type == SectionType.Monster)
			{
				unspawnedSection.Enqueue(inspectedSection);
			}
		}
		for (int q = 0; q < spawnMonCount; q++)
		{
			if (unspawnedSection.Count == 0)
				break;
			Section hitSection = unspawnedSection.Dequeue();

			Debug.Log($"경로상에 있는 스폰 대기중인 몬스터섹션{hitSection}{q} 실행!");
			hitSection.RunArrivalMonster(transform.gameObject);
		}
		unspawnedSection.Clear();
	}

	public void MoveEventHandler(Transform transform) // MoveEventHandler(this.transform)
	{
		Section sectionUnder = FieldManager.Instance.GetSectionAtPosition(transform.position);
		if (sectionUnder != null && sectionUnder != EventManager.Instance.CurrentSection)
		{
			EventManager.Instance.ChangeCurrentSection(sectionUnder);
			// 몬스터 섹션이면 바로 실행
			if (EventManager.Instance.CurrentSection.Type == SectionType.Monster && EventManager.Instance.CurrentSection.monsterSpawned == false)
			{
				Debug.Log("몬스터섹션이 닿았고 아직 스폰이 안되었을 때 실행!");
				EventManager.Instance.CurrentSection.RunArrivalMonster(transform.gameObject); // 몬스터스폰함수
			}
			//if (EventManager.Instance.CurrentSection.Type == SectionType.Event && EventManager.Instance.CurrentSection.monsterSpawned == false)
			//{
				
			//	Debug.Log("닿았을때 이벤트몬스터 스폰!");
			//}
		}
	}
	public void arriveEvent()
	{
		if (_targetSection != null) // 런어라이벌이벤트에서 타입에따라 실행되는게 달라서여기서 타입체크를 할필요가없음
		{
			Debug.Log("도착 후이벤트 실행");
			_targetSection.RunArrivalEvent();
		}
	}
}
