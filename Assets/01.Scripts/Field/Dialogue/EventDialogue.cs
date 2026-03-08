using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ObstacleShipEventBase;


public class EventDialogue : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI dialogueTMP;
	[SerializeField] private EventType eventType;
	[SerializeField] private EventDialogueSO eventDialogueSO;
	[SerializeField] private EventBase ExecuteEvent;

	[SerializeField] private GameObject Buttons;
	[SerializeField] private GameObject AcceptObject;
	[SerializeField] private Button AcceptBtn;
	[SerializeField] private TextMeshProUGUI AcceptText;
	[SerializeField] private GameObject DetectObject;
	[SerializeField] private Button DetectBtn;
	[SerializeField] private TextMeshProUGUI DetectText;
	[SerializeField] private GameObject Option1Object;
	[SerializeField] private Button Option1Btn;
	[SerializeField] private TextMeshProUGUI Option1Text;
	[SerializeField] private GameObject Option2Object;
	[SerializeField] private Button Option2Btn;
	[SerializeField] private TextMeshProUGUI Option2Text;

	bool buttonOpened = false;
	public bool ShopClose = false;

	public event Action reward = null;
	public int? rewardIndex = null;

	[TextArea]
	[SerializeField] private List<string> Lines = new List<string>();

	private int index = 0;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		dialogueTMP = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		if (Lines == null || Lines.Count == 0)
		{
			Debug.LogError("Dialogue list is empty");
			return;
		}
		if (index < 0 || index >= Lines.Count)
		{
			Debug.LogError($"Dialogue index out of range: {index} / {Lines.Count}");
			return;
		}
		dialogueTMP.text = Lines[index];
		ButtonClose();
	}

	public void init()
	{
		Buttons = this.transform.Find("Buttons")?.gameObject;
		AcceptObject = Buttons.transform.Find("Accept")?.gameObject;
		DetectObject = Buttons.transform.Find("Reject")?.gameObject;
		Option1Object = Buttons.transform.Find("Option1")?.gameObject;
		Option2Object = Buttons.transform.Find("Option2")?.gameObject;
		AcceptBtn = AcceptObject.GetComponentInChildren<Button>();
		DetectBtn = DetectObject.GetComponentInChildren<Button>();
		Option1Btn = Option1Object.GetComponentInChildren<Button>();
		Option2Btn = Option2Object.GetComponentInChildren<Button>();
		AcceptText = AcceptObject.GetComponentInChildren<TextMeshProUGUI>();
		DetectText = DetectObject.GetComponentInChildren<TextMeshProUGUI>();
		Option1Text = Option1Object.GetComponentInChildren<TextMeshProUGUI>();
		Option2Text = Option2Object.GetComponentInChildren<TextMeshProUGUI>();
	}


	public void ChangeSetting(EventType _eventType, List<string> lines, EventDialogueSO _eventDialogueSO, EventBase _eventBase)
	{
		eventType = _eventType;
		Lines = lines;
		eventDialogueSO = _eventDialogueSO;
		ExecuteEvent = _eventBase;
		buttonOpened = false;
	}


	public void NextLine()
	{

		if (Lines == null || Lines.Count == 0) 
		{
			EventManager.Instance.CloseEventUI();
			return;
		} 

		index++;

		if (index == Lines.Count)
		{
			index = 0;
			if (buttonOpened == false)
			{
				SetBtn();
				SetBtnName();
				ButtonOpen();
				DialogueException();
				buttonOpened = true;
			}
			if (buttonOpened && EventManager.Instance.TargetSection.clearEvent)
			{
				EventManager.Instance.CloseEventUI();
			}
			if (eventType == EventType.blackMarket && buttonOpened == true && ShopClose == true)
			{
				EventManager.Instance.CloseEventUI();
				ShopClose = false;
			}
		}
		if (index >= Lines.Count)
		{
			index = 0;
		}
		ApplyDialogue();
	}

	public void LivingDialogue()
	{
		index = 0;
		ApplyDialogue();
		ButtonClose();
	}

	public void ApplyDialogue()
	{
		if(!(index >= Lines.Count || index < 0))
		{
			dialogueTMP.text = Lines[index];
		}
		else
		{
			transform.parent.gameObject.SetActive(false);
		}
		//dialogueTMP.text = Lines[index];
		Canvas.ForceUpdateCanvases();
	}
	public void SetBtn()
	{
		ButtonClear();
		if (eventDialogueSO.ButtonCount == 2)
		{
			AcceptBtn.onClick.AddListener(() => ExecuteEvent.Accepted());
			DetectBtn.onClick.AddListener(() => ExecuteEvent.Detected());
		}
		else if (eventDialogueSO.ButtonCount == 3)
		{
			AcceptBtn.onClick.AddListener(() => ExecuteEvent.Accepted());
			DetectBtn.onClick.AddListener(() => ExecuteEvent.Detected());
			Option1Btn.onClick.AddListener(() => ExecuteEvent.OtherOption());
		}
		else if (eventDialogueSO.ButtonCount == 4)
		{
			AcceptBtn.onClick.AddListener(() => ExecuteEvent.Accepted());
			DetectBtn.onClick.AddListener(() => ExecuteEvent.Detected());
			Option1Btn.onClick.AddListener(() => ExecuteEvent.OtherOption());
			Option2Btn.onClick.AddListener(() => ExecuteEvent.OtherOption2());
		}

	}
	public void SetBtnName()
	{

		if (eventDialogueSO.ButtonCount == 2)
		{
			AcceptText.text = ExecuteEvent.AcceptBtnName;
			DetectText.text = ExecuteEvent.DetectBtnName;
		}
		else if (eventDialogueSO.ButtonCount == 3)
		{
			AcceptText.text = ExecuteEvent.AcceptBtnName;
			DetectText.text = ExecuteEvent.DetectBtnName;
			Option1Text.text = ExecuteEvent.Option1BtnName;
		}
		else if (eventDialogueSO.ButtonCount == 4)
		{
			AcceptText.text = ExecuteEvent.AcceptBtnName;
			DetectText.text = ExecuteEvent.DetectBtnName;
			Option1Text.text = ExecuteEvent.Option1BtnName;
			Option2Text.text = ExecuteEvent.Option2BtnName;
		}
		List<TextMeshProUGUI> TMPList = new List<TextMeshProUGUI> { AcceptText, DetectText, Option1Text, Option2Text };
		foreach (TextMeshProUGUI TMP in TMPList)
		{
			if(TMP.text != null)
			{
				if (TMP.text.Length > 9)
				{
					TMP.fontSize = 30;
				}
				else
				{
					TMP.fontSize = 40.2f;
				}
			}
			else
			{
				continue;
			}
			
		}

	}
	public void SetBtnName(string accept = null, string detect = null, string option1 = null, string option2 = null)
	{
		AcceptText.text = accept;
		DetectText.text = detect;
		Option1Text.text = option1;
		Option2Text.text = option2;
		List<TextMeshProUGUI> TMPList = new List<TextMeshProUGUI> { AcceptText, DetectText, Option1Text, Option2Text };
		foreach (TextMeshProUGUI TMP in TMPList)
		{
			if( TMP.text != null)
			{
				if (TMP.text.Length > 9)
				{
					TMP.fontSize = 30;
				}
				else
				{
					TMP.fontSize = 40.2f;
				}
			}
		}
	}
	public void SetBtn(Action accept, Action detect, Action option1 = null, Action option2 = null)
	{
		ButtonClear();
		AcceptBtn.onClick.AddListener(() => accept?.Invoke());
		DetectBtn.onClick.AddListener(() => detect?.Invoke());
		Option1Btn.onClick.AddListener(() => option1?.Invoke());
		Option2Btn.onClick.AddListener(() => option2?.Invoke());
	}

	public void ButtonOpen()
	{
		if (eventDialogueSO.ButtonCount == 2)
			{
				AcceptObject.SetActive(true);
				DetectObject.SetActive(true);
			}
			else if (eventDialogueSO.ButtonCount == 3)
			{
				AcceptObject.SetActive(true);
				DetectObject.SetActive(true);
				Option1Object.SetActive(true);
			}
			else if (eventDialogueSO.ButtonCount == 4)
			{
				AcceptObject.SetActive(true);
				DetectObject.SetActive(true);
				Option1Object.SetActive(true);
				Option2Object.SetActive(true);

			}
		else if (eventDialogueSO.ButtonCount == 0)
		{
			return;
		}

		if(eventDialogueSO.EventType == EventType.imperialTradeLine)
		{
			if(EventManager.Instance.isAccept == false)
			{
				DetectObject.SetActive(false);
				Option1Object.SetActive(true);
			}
		}
		if(eventDialogueSO.EventType == EventType.ruleAnnouncement)
		{
			if (InventoryManager.Instance.HasTicket() == false)
			{
				AcceptObject.SetActive(false);
			}
		}
		
	}
	public void DialogueException()
	{
		if(eventDialogueSO.ButtonCount == 0)
		{
			ExecuteEvent.Accepted();
		}
	}
	public void ButtonClose()
	{
		AcceptObject.SetActive(false);
		DetectObject.SetActive(false);
		Option1Object.SetActive(false);
		Option2Object.SetActive(false);
	}
	public void ButtonClear()
	{
		AcceptBtn.onClick.RemoveAllListeners();
		DetectBtn.onClick.RemoveAllListeners();
		Option1Btn.onClick.RemoveAllListeners();
		Option2Btn.onClick.RemoveAllListeners();
	}

	public void SetLine(List<string> _line)
	{
		Lines = _line;
		index = 0;
	}
	public void ButtonOpen(bool accept = false , bool detect = false, bool option1 = false, bool option2 = false)
	{
		AcceptObject.SetActive(accept);
		DetectObject.SetActive(detect);
		Option1Object.SetActive(option1);

		Option2Object.SetActive(option2);
	}

}
