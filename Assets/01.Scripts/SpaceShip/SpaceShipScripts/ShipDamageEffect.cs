using UnityEngine;
using System.Collections;

public class ShipDamageEffect : MonoBehaviour
{
    [Header("Particle")]
    public ParticleSystem hitEffect;

    [Header("Flash Effect")]
    public Renderer shipRenderer;
    public Color flashColor = Color.white;
    public float flashTime = 0.1f;

    public bool isShipDestroy;

    [Header("Camera Shake")]
    public Transform cameraTransform;
    public float shakePower = 0.2f;
    public float shakeTime = 0.15f;

    Color originColor;
    bool isFlashing = false;

    void Start()
    {
        // 기본 머티리얼 컬러 저장
        if (shipRenderer != null)
            originColor = shipRenderer.material.color;

        cameraTransform = Camera.main.transform;

        // 테스트용
        //Invoke(nameof(TestDamage), 2f);
    }

    public void PlayDamage()
    {
        if (isShipDestroy != true)
        {
			Debug.Log("SHIP DAMAGE!");

			//파티클
			if (hitEffect != null)
				hitEffect.Play();

			//플래시
			if (!isFlashing)
				StartCoroutine(Flash());
                SoundManager.Instance.PlaySFX(SFXType.Spaceship_Hit);

			//카메라 흔들림
			if (cameraTransform != null)
				StartCoroutine(CameraShake());
		}    
    }


    public void stopDamageCoroutines()
    {
		StopAllCoroutines();
	}

    IEnumerator Flash()
    {
        isFlashing = true;

		shipRenderer.material.EnableKeyword("_EMISSION");
		shipRenderer.material.SetColor("_EmissionColor", Color.red);
		shipRenderer.material.color = Color.red;

		yield return new WaitForSeconds(flashTime);
        shipRenderer.material.DisableKeyword("_EMISSION");
		shipRenderer.material.color = originColor;
        isFlashing = false;
    }

    IEnumerator CameraShake()
    {
        Vector3 originPos = cameraTransform.localPosition;
        float t = 0f;

        while (t < shakeTime)
        {
            t += Time.deltaTime;
            cameraTransform.localPosition =
                originPos +
                Random.insideUnitSphere * shakePower;
            yield return null;
        }

        cameraTransform.localPosition = originPos;
    }

    //void TestDamage()
    //{
    //    PlayDamage();
    //}
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        PlayDamage();
    //    }
    //}
}
