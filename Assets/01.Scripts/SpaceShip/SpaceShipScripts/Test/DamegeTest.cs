using UnityEngine;

public class DamegeTest : MonoBehaviour
{

    public ShipDamageReceiver ship;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float DamageAmount = 10;



    public void AttackDamege()
    {
        ship.GetDamage(DamageAmount);
    }

}
