using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    private Section section;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        section = GetComponentInParent<Section>();
	}

	// Update is called once per frame
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && FighterManager.Instance.isLaunched != false)
		{
			section.RunArrivalMonster(other.gameObject);
		}
	}
}
