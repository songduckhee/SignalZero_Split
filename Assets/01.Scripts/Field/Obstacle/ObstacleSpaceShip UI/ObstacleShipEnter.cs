using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleShipEnter : MonoBehaviour
{
    
    public ObstacleEventType eventType;
    public bool IsClear = false;


	private void OnTriggerEnter(Collider other)
	{
        // obstacleShipDialouge가 자기 타입의 이벤트 로그 텍스트를 띄워주도록 해야하고 코루틴 실행 끝에 버튼이 나오고,
        // 버튼은 실행 끝에서 EventBase Text로 바뀌어어야하고
        // 버튼을 눌렀을 시 다시 텍스트 코루틴이 진행되고
        // 진행 끝에 텍스트 패널이 꺼져야함
        
		bool isLaunched = FighterManager.Instance.isLaunched;

		if (other.gameObject.CompareTag("Player") && isLaunched == true && IsClear == false)
        {
			EventManager.Instance.ShipDialogue.SetInfo(eventType);
			EventManager.Instance.ShipDialogue.AddListener();
            EventManager.Instance.ShipDialogue.InitPopUp(this);
            
		}
	}
		
	private void OnTriggerExit(Collider other)
	{
        bool isLaunched = FighterManager.Instance.isLaunched;

		if (other.gameObject.CompareTag("Player") && isLaunched == true)
        {
     
				EventManager.Instance.ShipDialogue.StopCoroutine();
	
			

			//이벤트 매니저에 있는 shipAnimator와 ShipUI를 Close상태로
			EventManager.Instance.CloseObstacleShipUI();
			//교신버튼 리셋
			EventManager.Instance.DeletePlayerRemoveListener();
		}
	}


    

    



}
