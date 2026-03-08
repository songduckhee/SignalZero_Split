using UnityEngine;

public class ShipStat : MonoBehaviour
{
    float MaxHealth = 100;
    float CurrentHealth;
	float HealthRecoverSpeed = 3;
	float shieldRecoverSpeed = 3;
    float MaxShield = 100;
    float CurrentShield;
    


    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void init()
    {
        CurrentHealth = MaxHealth;
        CurrentShield = MaxShield;
    }

    public void ShieldDamage(float damage)
    {
        if(CurrentShield > 0f)
        {
			CurrentShield -= damage;
		}
    }

    public void HealthDamage(float damage)
    {
        if(CurrentHealth > 0f)
        {
			CurrentHealth -= damage;
		}
    }






}
