using UnityEngine;

public class MiniMapToggle : MonoBehaviour
{
    public GameObject miniMapRoot;

    void Start()
    {
       
    }

    public void Toggle()
    {
        miniMapRoot.SetActive(!miniMapRoot.activeSelf);
        Debug.Log("MiniMap Toggle : " + miniMapRoot.activeSelf);
    }
}
