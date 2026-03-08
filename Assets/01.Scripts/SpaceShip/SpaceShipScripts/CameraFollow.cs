using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // 우주선
    public Vector3 offset = new Vector3(0, 0, 0);

    [Header("Follow Settings")]
    public float positionSmooth = 5f;
    public float rotationSmooth = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // 위치 추적
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            positionSmooth * Time.deltaTime
        );

        // 회전 추적 (우주선 방향 기준)
        Quaternion targetRot = Quaternion.LookRotation(
            target.forward,
            Vector3.up
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSmooth * Time.deltaTime
        );
    }
}
