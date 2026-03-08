using UnityEngine;

public class DestructibleObstacle : MonoBehaviour, IDamageable
{
	public int index;
	public bool isUnDestroyed;
	public bool isDestroy;

	public void Die()
	{
		throw new System.NotImplementedException();
	}

	public void GetDamage(float value)
	{
		if (isUnDestroyed)
			return;
		Debug.Log("장애물파괴!");
		FieldManager.Instance.obstacleManager.RemoveObstacleFromList(gameObject);
		GameObject effect = Instantiate(FieldManager.Instance.obstacleManager.explosionEffect,
	transform.position,
	Quaternion.identity);
		Destroyed();

	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created

	public void OnCollisionEnter(Collision collision)
	{
		if (isUnDestroyed)
			return;
		Debug.Log("장애물파괴!");
		Vector3 hitPos = collision.contacts[0].point;
		GameObject effect = Instantiate(FieldManager.Instance.obstacleManager.explosionEffect,
		hitPos,
		Quaternion.identity);
		effect.transform.localScale = Vector3.one * 5f;
		Destroyed();
	}

	public void Destroyed()
	{
		isDestroy = true;
		this.gameObject.SetActive(false);
	}
}
