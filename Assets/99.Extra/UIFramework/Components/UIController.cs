using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public UiManagerNew uIManager;
    [SerializeField] private GameObject shipView;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Destroy old UI");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        uIManager = GetComponent<UiManagerNew>();
    }

    public void OpenShipUI()
    {
        uIManager.OpenOnlyUI(shipView);
    }


    public void openInvenUI()
    {
        //uIManager.ShowPopup(invenView);
        InventoryManager.Instance.OpenInvenUI();
    }

    public void LaunchFighter()
    {
        FighterManager.Instance.LaunchWithWeapon();
    }
}
