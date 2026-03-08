using UnityEngine;

public class ShipDamageReceiver : MonoBehaviour, IDamageable
{
	public float maxHP = 3000;
	float lastHP;
	[SerializeField] float currentHP = 3000;
	public float RecoverSpeed = 3f;

	public float MaxAlertTime = 30f;
	[SerializeField] float alertTimer = 30f;
	public bool Startalert;
	public bool isRecover;
	public bool ShipBodyDestroyed;

	public bool Godmode = false;
	public bool dead = false;

	public bool isShipDestroy;

	public Collider shipCollider;

	public GameObject ShipBody;

	public ShipDamageEffect damageEffect;   // 이미 만든 데미지 연출 연결


	public ShipStatUI statUI;

	void Start()
	{
		Init();
	}


	public void Init()
	{
		currentHP = maxHP;
		alertTimer = MaxAlertTime;
		Startalert = false;
		isRecover = false;
		ShipBodyDestroyed = false;
	}



	private void Update()
	{
		if (statUI != null)
		{
			statUI.UpdateHealthUI(currentHP,maxHP);
			if (isShipDestroy != true)
			{
				RecoverHealth();
				AlertCount();
			}
			if (currentHP <= 10)
			{
				if(isShipDestroy == false)
				{
					statUI.ShowVignette();
				}
				else
				{
					statUI.HideVignette();
				}	
			}
			else
			{
				statUI.HideVignette();
			}
			if (currentHP <= 0)
			{
				if(dead == true)
					return;
				OnShipDestroyed();

			}
		}
	}


	private void RecoverHealth()
	{
		if (currentHP >= 100)
		{
			return;
		}
		if (isRecover == true)
		{
			currentHP += Time.deltaTime * RecoverSpeed;
		}
	}
	public void GetHealth(float health)
	{
		currentHP += health;
		if(currentHP > maxHP)
		{
			currentHP = maxHP;
		}
	}
	public void DangerAlert()
	{
		if (statUI != null)
		{
			if (currentHP < 52 && currentHP > 48)
			{
				statUI.ShowAlertImage();
			}
			else if (currentHP < 32 && currentHP > 28)
			{
				statUI.ShowAlertImage();
			}
			else if (currentHP < 12 && currentHP > 8)
			{

				statUI.ShowAlertImage();
			}
		}
	}

	public void AlertCount()
	{
		if (Startalert)
		{
			alertTimer -= Time.deltaTime;
		}
		if (alertTimer <= 0.1 || alertTimer <= 0)
		{
			alertTimer = MaxAlertTime;
			Startalert = false;
		}
	}

	/// <summary>
	/// 외부에서 데미지를 줄 때 호출!
	/// </summary>
	public void ApplyDamage(float damage)
	{
		if (isShipDestroy != true)
		{
			if (currentHP <= 0)
				return;
			lastHP = currentHP;
			currentHP -= damage;

			if (damageEffect != null)
				damageEffect.PlayDamage();

			Debug.Log($"Ship Hit! HP : {currentHP}");

			if (currentHP <= 0)
			{
				isShipDestroy = true;
				currentHP = 0;
				OnShipDestroyed();
			}
		}
	}
	public void IfDead()
	{
		if(dead == false)
		{
			OnShipDestroyed();
		}
	}

	public void ApplyDamagePercent(int damage)
	{
		float damageToDeal = maxHP * damage/100;
		float curHp = currentHP;
		if(curHp -  damageToDeal <= 0)
		{
			currentHP = 0;
		}
		else
		{
			currentHP -= (float)damageToDeal;
		}	
	}
	public void ApplyRecoveryPercent(int health)
	{
		float RecoverHealth = maxHP * health / 100;
		float curHp = currentHP;
		if(curHp +  RecoverHealth >= maxHP)
		{
			currentHP = maxHP;
		}
		else
		{
			currentHP += (float)RecoverHealth;
		}		
	}

	void OnShipDestroyed()
	{
		dead = true;
		Debug.Log("SHIP DESTROYED!");
		ShipBodyDestroyed = true;
		damageEffect.isShipDestroy = true;
		statUI.HideVignette();
		damageEffect.stopDamageCoroutines();
		ShipMover.Instance.ShipDestroy();
		GameOver.instance.WhenGameOver();

		//폭발, 게임오버 UI, 상태변경 등 연결
	}

	public void GetDamage(float value)
	{
		if (Startalert != true)
		{
			Startalert = true;
			statUI?.ShowAlertImage();
		}
		if (currentHP <= 0)
		{
			shipCollider.enabled = false;
		}
		DangerAlert();

		if(Godmode == false)
		{
			ApplyDamage(value);
		}
		else
		{
			return;
		}
	}

	public void Die()
	{
		OnShipDestroyed();
	}

	
}
