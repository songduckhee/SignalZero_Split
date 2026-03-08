using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BlueSpot : MonoBehaviour
{
	public GameObject Panel_BlueSpot;
	public GameObject Juwon_BlueSpot;
	public GameObject BluespotBackground;
	private Animator animator;
	public Button NextSector;
	public event Action inBlueSpot;
	public event Action outBlueSpot;

	private Camera mainCamera;
	public EndingTemp endingTemp;



	private void Start()
	{
		CloseBlueSpotUI();
		mainCamera = Camera.main;
	}


	public void OpenBlueSpotUI()
	{
		Panel_BlueSpot.SetActive(true);
		Juwon_BlueSpot.SetActive(true);
		if (FieldManager.Instance.SectorCount == 1)
		{
			EventManager.Instance.ShopDialogue.OpenShop(Npc.adaptor);
		}
		else if (FieldManager.Instance.SectorCount >= 2)
		{
			EventManager.Instance.ShopDialogue.OpenShop(Npc.centinel);
		}
		BluespotBackground.SetActive(true);
		inBlueSpot?.Invoke();
		EventManager.Instance.contactShip.gameObject.SetActive(false);
		mainCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
		if(FieldManager.Instance.SectorCount == 1 || FieldManager.Instance.SectorCount == 2)
		{
			NextSector.onClick.AddListener(FieldManager.Instance.ResetSector);
		}
		else
		{
			NextSector.onClick.AddListener(endingTemp.LoadEnding);
		}
		
	}
	public void CloseBlueSpotUI()
	{
		Panel_BlueSpot.SetActive(false);
		Juwon_BlueSpot.SetActive(false);
		BluespotBackground.SetActive(false);
		outBlueSpot?.Invoke();
		if(mainCamera != null)
		{
			mainCamera.cullingMask = -1;
		}
		NextSector.onClick.RemoveAllListeners();
	}

}
