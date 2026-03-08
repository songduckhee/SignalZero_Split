using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ObstacleShipEventBase;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class ShipDialogue : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI ObstacleShipText;
	[SerializeField] ObstacleShipSO obstacleShipSO;
	[SerializeField] List<string> dialogueLine;
	private Coroutine dialogueCoroutine;
	[SerializeField] ObstacleShipEventBase eventBase;
	public ObstacleEventType eventType;
	private ObstacleShipEnter ObstacleShipEnter;

	private int buttonIndex = 0;

	[Header("데이터")]
	[SerializeField] List<ObstacleShipSO> obstacleShipSOList;
	[SerializeField] Dictionary<ObstacleEventType, ObstacleShipSO> obstacleSODict = new Dictionary<ObstacleEventType, ObstacleShipSO>();
	[SerializeField] Dictionary<ObstacleEventType, ObstacleShipEventBase> obstacleBaseDict = new Dictionary<ObstacleEventType, ObstacleShipEventBase>();

	[SerializeField] private GameObject AcceptObject;
	[SerializeField] private Button Accept;
	[SerializeField] private TextMeshProUGUI AcceptText;
	[SerializeField] private GameObject DetectObject;
	[SerializeField] private Button Detect;
	[SerializeField] private TextMeshProUGUI DetectText;
	[SerializeField] private GameObject Option1Object;
	[SerializeField] private Button Option1;
	[SerializeField] private TextMeshProUGUI Option1Text;
	[SerializeField] private GameObject Option2Object;
	[SerializeField] private Button Option2;
	[SerializeField] private TextMeshProUGUI Option2Text;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		//오브젝트가 꺼져있으면 스타트가 시작되지않음
	}

	public void SetLine(List<string> strings)
	{
		buttonIndex = 0;
		dialogueLine = strings;
	}

	public void StopCoroutine()
	{
		if (dialogueCoroutine != null)
		{
			StopCoroutine(dialogueCoroutine);
		}
	}

	IEnumerator StartDialogue()
	{
		dialogueLine = obstacleShipSO.dialogues;
		EventManager.Instance.DerelictShipUI.SetActive(true);
		for (int i = 0; i < dialogueLine.Count; i++)
		{
			EventManager.Instance.DerelictShipUIText.text = dialogueLine[i];

			yield return new WaitForSecondsRealtime(2f);
		}
		ActiveButton(true);
	}
	IEnumerator NextDialogue()
	{
		for (int i = 0; i < dialogueLine.Count; i++)
		{
			EventManager.Instance.DerelictShipUIText.text = dialogueLine[i];

			yield return new WaitForSecondsRealtime(2f);
		}
		EventManager.Instance.CloseObstacleShipUI();
	}

	//버튼을 눌렀을때 나오는것
	public void StartNextDialogue()
	{
		if (dialogueLine[0] == "")
		{
			EventManager.Instance.CloseObstacleShipUI();
			return;
		}
		dialogueCoroutine = StartCoroutine(NextDialogue());
	}
	public void ButtonLine()
	{
		if (dialogueLine == null || dialogueLine.Count == 0) return;
		buttonIndex ++;
		EventManager.Instance.DerelictShipUIText.text = dialogueLine[buttonIndex];
	}

	public void AddListener() // 교신버튼 에드리스너 (교신버튼은 이벤트 매니저에)
	{
		EventManager.Instance.contactPlayer.onClick.AddListener(() => dialogueCoroutine = StartCoroutine(StartDialogue()));
		EventManager.Instance.EnableContactPlayer();
	}
	public void ActiveButton(bool value)
	{
		switch (obstacleShipSO.ButtonCount)
		{
			case 2:
				AcceptObject.SetActive(value);
				DetectObject.SetActive(value);
				break;
			case 3:
				AcceptObject.SetActive(value);
				DetectObject.SetActive(value);
				Option1Object.SetActive(value);
				break;
			case 4:
				AcceptObject.SetActive(value);
				DetectObject.SetActive(value);
				Option1Object.SetActive(value);
				Option2Object.SetActive(value);
				break;
			default:
				break;
		}
		if (eventType == ObstacleEventType.imperialTradeLine)
		{
			if(EventManager.Instance.isAccept == false)
			{
				DetectObject.SetActive(false);
			}
			else if (EventManager.Instance.isAccept == true)
			{
				DetectObject.SetActive(true);
			}
		}

		if (value == true)
		{
			AcceptText.text = eventBase.AcceptBtnName;
			DetectText.text = eventBase.DetectBtnName;
			Option1Text.text = eventBase.Option1BtnName;
			Option2Text.text = eventBase.Option2BtnName;
			List<TextMeshProUGUI> TMPList = new() { AcceptText, DetectText, Option1Text, Option2Text };
			foreach (TextMeshProUGUI TMP in TMPList)
			{
				if (TMP == null)
				{
					Debug.Log("TMP is null!");
				}
				else
				{
					Debug.Log("TMP isn't null!");
				}
				if (TMP.text.Length > 9)
				{
					TMP.fontSize = 30;
				}
				else
				{
					TMP.fontSize = 40.2f;
				}
			}
			Accept.onClick.AddListener(eventBase.Accepted);
			Detect.onClick.AddListener(eventBase.Detected);
			Option1.onClick.AddListener(eventBase.Option1);
			Option2.onClick.AddListener(eventBase.Option2);
		}
		else if (value == false)
		{
			Accept.onClick.RemoveAllListeners();
			Detect.onClick.RemoveAllListeners();
			Option1.onClick.RemoveAllListeners();
			Option2.onClick.RemoveAllListeners();
		}
	}

	public void SetInfo(ObstacleEventType eventtype)
	{
		eventType = eventtype;
		obstacleShipSO = obstacleSODict[eventType];
		eventBase = obstacleBaseDict[eventType];
		dialogueLine = obstacleShipSO.dialogues;
	}



	//데이터 Build
	public void BuildObstacleShipDictionary()
	{
		for (int i = 0; i < obstacleShipSOList.Count; i++)
		{
			obstacleSODict.Add(obstacleShipSOList[i].eventType, obstacleShipSOList[i]);
		}
	}
	public void BuildObstacleBaseDictionary()
	{
		//obstacleBaseDict.Add(ObstacleEventType.blackMarketCape,new BlackMarketCape());
		//obstacleBaseDict.Add(ObstacleEventType.imperialTradeLine,new ImperialTradeLine());
		//obstacleBaseDict.Add(ObstacleEventType.dockedRescueShip,new DockedRescueShip());
		//obstacleBaseDict.Add(ObstacleEventType.rescueSignal,new RescueSignal());
		//obstacleBaseDict.Add(ObstacleEventType.signalRecorder,new SignalRecorder());
	}
	public void SetButtonAtFirst()
	{
		AcceptObject.SetActive(false);
		DetectObject.SetActive(false);
		Option1Object.SetActive(false);
		Option2Object.SetActive(false);
	}
	public void DataBuild()
	{
		BuildObstacleBaseDictionary();
		BuildObstacleShipDictionary();
		SetButtonAtFirst();
	}

	public void InitPopUp(ObstacleShipEnter pop)
	{
		ObstacleShipEnter = pop;
	}
	public void SetClear(bool value)
	{
		ObstacleShipEnter.IsClear = value;
	}
}
