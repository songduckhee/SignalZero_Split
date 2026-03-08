using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShipStatUI : MonoBehaviour
{

    [SerializeField] Image HealthUI;
    [SerializeField] Animator AlertAnimator;
    [SerializeField] Animator vignette;

    Coroutine AlertCor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateHealthUI(float health, float maxHealth)
    {
        HealthUI.fillAmount = health/maxHealth;
    }
    public void ShowAlertImage()
    {
       AlertAnimator.SetTrigger("isAlert");
    }

    public void ShowVignette()
    {
        vignette.SetBool("is10%",true);
    }
    public void HideVignette()
    {
        vignette.SetBool("is10%",false);
    }
  

}
