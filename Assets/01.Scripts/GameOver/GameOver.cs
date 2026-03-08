using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
	public static GameOver instance;
	[SerializeField] private GameObject Panel_Inven;
	[SerializeField] private GameObject Panel_Ending;
	private Button title;
	public event Action OnOver;
	private void Awake()
	{
		instance = this;
		title = Panel_Ending.GetComponentInChildren<Button>();
		title.onClick.AddListener(LoadScene);
	}
	
	public void WhenGameOver()
	{
		Panel_Inven.SetActive(true);
		Panel_Ending.SetActive(true);
		InventoryManager.Instance.gridInventoryUI.RefreshUI();
		OnOver?.Invoke();
	}
	public void Start()
	{
		Panel_Ending.SetActive(false);
	}
	public void LoadScene()
	{
		SceneManager.LoadScene("TitleScene");
	}
}
