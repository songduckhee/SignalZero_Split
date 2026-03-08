using UnityEngine;

public class StableSkyboxRotation : MonoBehaviour
{
	[SerializeField] private Transform player;
	Vector3 prevPos;
	Vector3 skyRot;
	[SerializeField] private float rotateSpeed;

	private void Awake()
	{	
		player = GameObject.FindWithTag("Player").transform;
		prevPos = player.position;
	}

	private void Update()
	{
		//skyRot = RenderSettings.skybox.GetFloat("_Rotation");
		Vector3 currPos = player.position;
		Vector3 subtract = currPos - prevPos;
		float x = subtract.x *Time.deltaTime * rotateSpeed;
		float z = subtract.z * Time.deltaTime * rotateSpeed;
		Quaternion q = Quaternion.Euler(new Vector3(-z, x, 0));
		transform.rotation *= q;
		prevPos = currPos;
		//float delta = Mathf.DeltaAngle(prevY, currY);
		//skyRot += delta + rotateSpeed * Time.deltaTime;
		//RenderSettings.skybox.SetFloat("_Rotation",skyRot);

		//prevY = currY;
	}
}




