using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public enum Npc
{
    None,
    cape,
    adaptor,
    centinel,
    cape2,
    cape3
}
public class ShopDialogue : MonoBehaviour
{

    [SerializeField] GameObject panel_ShopNPC;
    [SerializeField] TextMeshProUGUI dialogue;
    [SerializeField] Button dialogueButton;

    [SerializeField] GameObject cape;
    [SerializeField] GameObject adaptor;
    [SerializeField] GameObject centinel;

    [SerializeField] List<ShopSO> ShopSOs;
    Dictionary<Npc , ShopSO> shopDict = new Dictionary<Npc, ShopSO>();

	bool isOpen;
    bool isGambling;
    bool isRepair;
    bool isBuyScrap;

    int index = 0;
    string currentNpc;
	[SerializeField] Animator? NpcAnimator;
    [SerializeField] Animator ImageAnimater;
    List<string>Lines;
    [SerializeField] Npc currentNpcEnum;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
       
	}

    public void Init()
    {
		BuildDict();
		AddListener();
		CloseShop();
	}

    public void BuildDict()
    {
        foreach (var shop in ShopSOs)
        {
            shopDict.Add(shop.Npc,shop);
        }
    }

    public void OpenShop(Npc npc)
    {
        panel_ShopNPC.SetActive(true);
		ChangePortrait(npc);
		ChangeLine();
		ApplyDialogue();
		OpenUiPanel();
	}
    public void CloseShop()
    {
        panel_ShopNPC.SetActive(false);
		isOpen = false;
        currentNpcEnum = Npc.cape;
		ChangetoBool("");
		CloseUiPanel();
	}

    public void OpenNpcPanel()
    {
		panel_ShopNPC.SetActive(true);
		ChangetoBool("");
		ApplyDialogue();
	}

    public void CloseNpcPanel(string boolname)
    {
		panel_ShopNPC.SetActive(false);
		ChangetoBool(boolname);
		ApplyDialogue();
	}

    public void AddListener()
    {
		dialogueButton.onClick.AddListener(ApplyDialogue);
        Button capeButton = cape.GetComponent<Button>();
        capeButton.onClick.AddListener(ApplyDialogue);
        Button adaptorButton = adaptor.GetComponent<Button>();
        adaptorButton.onClick.AddListener(ApplyDialogue);
	}
    public void ChangePortrait(Npc npc)
    {
        if(npc == Npc.cape)
        {
            currentNpc = "cape";
            currentNpcEnum = Npc.cape;
            NpcAnimator = cape.GetComponent<Animator>();
            cape?.SetActive(true);
            adaptor?.SetActive(false);
            centinel?.SetActive(false);
        }
        else if (npc == Npc.adaptor)
        {
            currentNpc = "adaptor";
            currentNpcEnum = Npc.adaptor;
            NpcAnimator = adaptor.GetComponent<Animator>();
            cape?.SetActive(false);
            adaptor?.SetActive(true);
            centinel?.SetActive(false);
        }
        else if (npc == Npc.centinel)
        {
            currentNpc = "centinel";
            currentNpcEnum = Npc.centinel;
            NpcAnimator = centinel.GetComponent<Animator>();
            cape?.SetActive(false);
            adaptor?.SetActive(false);
            centinel?.SetActive(true);
        }
        else if (npc == Npc.cape2)
        {
			currentNpc = "cape2";
			currentNpcEnum = Npc.cape2;
			NpcAnimator = cape.GetComponent<Animator>();
			cape?.SetActive(true);
			adaptor?.SetActive(false);
			centinel?.SetActive(false);
		}
        else if (npc == Npc.cape3)
        {
			currentNpc = "cape3";
			currentNpcEnum = Npc.cape3;
			NpcAnimator = cape.GetComponent<Animator>();
			cape?.SetActive(true);
			adaptor?.SetActive(false);
			centinel?.SetActive(false);
		}
    }

    public void ChangeLine()
    {
         //인덱스 초기화
        if(isOpen == false)
        {
			Lines = shopDict[currentNpcEnum].firstshops;
			index = 0;
		}
        else if (isOpen == true)
        {
            Lines = shopDict[currentNpcEnum].dialogue;
            index = 0;
		}
        else if (isRepair == true)
        {
            Lines = shopDict[(currentNpcEnum)].repair;
			index = 0;
		}
        else if (isGambling == true)
        {
            Lines = shopDict[currentNpcEnum].gambling;
			index = 0;
		}
        else if (isBuyScrap == true)
        {
            Lines = shopDict[currentNpcEnum].buyScrap;
			index = 0;
		}
       
    }
    public void ApplyDialogue()
    {
		if (Lines == null)
        {
            Lines = shopDict[currentNpcEnum].dialogue;
        }
        dialogue.text = Lines[index];
        index++;
        if(NpcAnimator != null)
        {
			NpcAnimator?.SetTrigger("IsTalk");
			
		}
        if(index >= Lines.Count)
        {
            if(isOpen == false || isGambling == true || isRepair == true || isBuyScrap == true)
            {
				Check();
			}
            index = 0;
        }
    }
    public void Check()
    {
		if (isOpen == false && index >= shopDict[currentNpcEnum].firstshops.Count)
		{
            isOpen = true;
			ChangeLine();
		}
        if (isGambling == true || isRepair == true || isBuyScrap == true)
        {
			ChangetoBool("");

		}
	}
    public void ChangetoBool(string boolname)
    {
		isGambling = boolname == "isGambling";
		isRepair = boolname == "isRepair";
		isBuyScrap = boolname == "isBuyScrap";
		ChangeLine();
	}

    

    public void OpenUiPanel()
    {
        if (EventManager.Instance.UI_Shop != null)
        {
			Transform UI_Shop = EventManager.Instance.UI_Shop.transform;
			Transform Close = UI_Shop.Find("Close");
			Button Closebutton = Close.GetComponent<Button>();

            if (!FieldManager.Instance.isBlueSpot)
            {
                Closebutton.onClick.AddListener(CloseShop);
                Closebutton.onClick.AddListener(EventManager.Instance.ReAddlistender);
            }
            

			Button UpgradeButton = UI_Shop.Find("Buttons").Find("UpgradeButton").GetComponent<Button>();
			Button InventoryButton = UI_Shop.Find("Buttons").Find("InventoryButton").GetComponent<Button>();
			UpgradeButton.onClick.AddListener(() => CloseNpcPanel(""));
			InventoryButton.onClick.AddListener(() => CloseNpcPanel(""));

			Button GatchaBtn = UI_Shop.Find("Buttons").Find("SideButtons").Find("Gatcha").GetComponent<Button>();
			Button Recover = UI_Shop.Find("Buttons").Find("SideButtons").Find("Recover").GetComponent<Button>();
			GatchaBtn.onClick.AddListener(() => CloseNpcPanel("isGambling"));
			Recover.onClick.AddListener(() => CloseNpcPanel("isRepair"));

			//사라지게
			Button ChoiceButton0 = UI_Shop.Find("Gatcha").Find("ShopChoice0").Find("ChoiceButton").GetComponent<Button>();
			Button ChoiceButton1 = UI_Shop.Find("Gatcha").Find("ShopChoice1").Find("ChoiceButton").GetComponent<Button>();
			Button ChoiceButton2 = UI_Shop.Find("Gatcha").Find("ShopChoice2").Find("ChoiceButton").GetComponent<Button>();

			ChoiceButton0.onClick.AddListener(OpenNpcPanel);
			ChoiceButton1.onClick.AddListener(OpenNpcPanel);
			ChoiceButton2.onClick.AddListener(OpenNpcPanel);

			Button Close1 = UI_Shop.Find("Recovered").Find("Close").GetComponent<Button>();
			Button Close2 = UI_Shop.Find("NotRecovered").Find("Close").GetComponent<Button>();

			Close1.onClick.AddListener(OpenNpcPanel);
			Close2.onClick.AddListener(OpenNpcPanel);

            Transform Panel_Inven = EventManager.Instance.Panel_Inven.transform;
            Transform Panel_Upgrade = EventManager.Instance.Panel_Upgrade.transform;

			Button Inven_ButtonClose = Panel_Inven.Find("Button_Close").GetComponent<Button>();
			Button Up_ButtonClose = Panel_Upgrade.Find("CloseButton").GetComponent<Button>();

			Inven_ButtonClose.onClick.AddListener(OpenNpcPanel);
			Up_ButtonClose.onClick.AddListener(OpenNpcPanel);
			//나타나게


			EventManager.Instance.UI_Shop.SetActive(true);
		}
    }
    public void CloseUiPanel()
    {
		if (EventManager.Instance.UI_Shop != null)
		{
			Transform UI_Shop  = EventManager.Instance.UI_Shop.transform;
			Transform Close = UI_Shop.Find("Close");
			Button Closebutton = Close.GetComponent<Button>();
			Closebutton.onClick.RemoveListener(CloseShop);
			Closebutton.onClick.RemoveListener(EventManager.Instance.ReAddlistender);

			Button UpgradeButton = UI_Shop.Find("Buttons").Find("UpgradeButton").GetComponent<Button>();
            Button InventoryButton = UI_Shop.Find("Buttons").Find("InventoryButton").GetComponent<Button>();
            UpgradeButton.onClick.RemoveListener(() => CloseNpcPanel(""));
            InventoryButton.onClick.RemoveListener(() => CloseNpcPanel(""));

            Button GatchaBtn = UI_Shop.Find("Buttons").Find("SideButtons").Find("Gatcha").GetComponent <Button>();
            Button Recover = UI_Shop.Find("Buttons").Find("SideButtons").Find("Recover").GetComponent<Button>();
            GatchaBtn.onClick.RemoveListener(() =>CloseNpcPanel("isGambling"));
            Recover.onClick.RemoveListener(() => CloseNpcPanel("isRepair"));

            //사라지게
            Button ChoiceButton0 = UI_Shop.Find("Gatcha").Find("ShopChoice0").Find("ChoiceButton").GetComponent<Button>();
			Button ChoiceButton1 = UI_Shop.Find("Gatcha").Find("ShopChoice1").Find("ChoiceButton").GetComponent<Button>();
			Button ChoiceButton2 = UI_Shop.Find("Gatcha").Find("ShopChoice2").Find("ChoiceButton").GetComponent<Button>();

            ChoiceButton0.onClick.RemoveListener(OpenNpcPanel);
            ChoiceButton1.onClick.RemoveListener(OpenNpcPanel);
			ChoiceButton2.onClick.RemoveListener(OpenNpcPanel);

            Button Close1 = UI_Shop.Find("Recovered").Find("Close").GetComponent<Button>();
            Button Close2 = UI_Shop.Find("NotRecovered").Find("Close").GetComponent<Button>();

            Close1.onClick.RemoveListener(OpenNpcPanel);
            Close2.onClick.RemoveListener(OpenNpcPanel);

			Transform Panel_Inven = EventManager.Instance.Panel_Inven.transform;
			Transform Panel_Upgrade = EventManager.Instance.Panel_Upgrade.transform;

			Button Inven_ButtonClose = Panel_Inven. Find("Button_Close").GetComponent<Button>();
            Button Up_ButtonClose = Panel_Upgrade. Find("CloseButton").GetComponent<Button>();

            Inven_ButtonClose.onClick.RemoveListener(OpenNpcPanel);
            Up_ButtonClose.onClick.RemoveListener(OpenNpcPanel);
            //나타나게

			EventManager.Instance.UI_Shop.SetActive(false);
            EventManager.Instance.contactShip.gameObject.SetActive(true);
		}
	}

}
