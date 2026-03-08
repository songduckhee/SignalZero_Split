using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SignalRingWave : MonoBehaviour
{
    [Header("기본 링")]
    public int segments = 128;
    public float baseRadius = 1.5f;

    [Header("색상")]
    public Color idleColor = new Color(0f, 1f, 0.4f);
    public Color dangerColor = new Color(1f, 0.25f, 0.1f);

    [Header("웨이브(좌클릭)")]
    public float idleWaveAmp = 0.00f;      // 평소는 0에 가깝게
    public float clickWaveAmp = 0.25f;     // 좌클릭 웨이브 강도
    public float waveFrequency = 2.5f;     // 둘레에서 굴곡 수(대충 느낌)
    public float noiseSpeed = 2.0f;        // 시간에 따른 변화 속도(라디오 느낌)

    [Header("우클릭(위험) - 거리 기반")]
    public float dangerMinAmp = 0.15f;     // 멀 때 최소 웨이브
    public float dangerMaxAmp = 0.55f;     // 가까울 때 최대 웨이브
    public float dangerNearDistance = 1.0f; // 이 거리 이하면 최대
    public float dangerFarDistance = 8.0f;  // 이 거리 이상이면 최소

    [Header("옵션: 방향성 강조(원하면 켜기)")]
    public bool directionalBoost = true;
    public float directionBoostPower = 2.0f;   // 높을수록 한쪽만 튐

    LineRenderer lr;
    Vector3[] points;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.loop = true;
        lr.useWorldSpace = false;

        points = new Vector3[segments];
        lr.positionCount = segments;
    }

    void Update()
    {
        bool leftHeld = Input.GetMouseButton(0);
        bool rightHeld = Input.GetMouseButton(1);

        float amp = idleWaveAmp;
        Color col = idleColor;

        // 우클릭(위험)이 좌클릭보다 우선
        if (rightHeld)
        {
            col = dangerColor;

            // 커서와 링 중심 거리로 웨이브 강도 결정
            Vector3 mouseWorld = GetMouseWorld();
            float dist = Vector3.Distance(transform.position, mouseWorld);

            float t = Mathf.InverseLerp(dangerFarDistance, dangerNearDistance, dist);
            // dist가 near면 t=1, far면 t=0
            amp = Mathf.Lerp(dangerMinAmp, dangerMaxAmp, t);
        }
        else if (leftHeld)
        {
            col = idleColor;
            amp = clickWaveAmp;
        }
        else
        {
            col = idleColor;
            amp = idleWaveAmp;
        }

        lr.startColor = col;
        lr.endColor = col;

        DrawRing(amp, rightHeld);
    }

    void DrawRing(float amp, bool isDanger)
    {
        // 커서 방향 강조용 벡터(우클릭일 때만 쓰는 게 보통 자연스러움)
        Vector2 dir = Vector2.zero;
        if (isDanger && directionalBoost)
        {
            Vector3 mouseWorld = GetMouseWorld();
            Vector3 d = (mouseWorld - transform.position);
            dir = new Vector2(d.x, d.y).normalized;
        }

        float time = Time.time * noiseSpeed;

        for (int i = 0; i < segments; i++)
        {
            float a = (i / (float)segments) * Mathf.PI * 2f;
            Vector2 unit = new Vector2(Mathf.Cos(a), Mathf.Sin(a));

            // “라디오 잡음” 느낌: PerlinNoise 두 축 + 약간의 사인 섞기
            float n = Mathf.PerlinNoise(unit.x * waveFrequency + time, unit.y * waveFrequency + 0.37f + time);
            n = (n - 0.5f) * 2f; // -1 ~ 1

            float sine = Mathf.Sin(a * waveFrequency + time) * 0.35f;

            float wave = (n * 0.65f + sine * 0.35f);

            // 우클릭일 때 커서 방향 쪽을 더 크게 튀게(원하면)
            if (isDanger && directionalBoost)
            {
                float dot = Mathf.Clamp01(Vector2.Dot(unit, dir)); // 커서 방향과 같은 쪽 1
                dot = Mathf.Pow(dot, directionBoostPower);
                wave *= Mathf.Lerp(0.4f, 1.2f, dot); // 반대쪽은 약하게
            }

            float r = baseRadius + wave * amp;

            points[i] = new Vector3(unit.x * r, unit.y * r, 0f);
        }

        lr.SetPositions(points);
    }

    Vector3 GetMouseWorld()
    {
        // 2D 탑뷰 기준: Z를 오브젝트와 동일 평면으로 맞춤
        Camera cam = Camera.main;
        Vector3 m = Input.mousePosition;

        float z = cam.WorldToScreenPoint(transform.position).z;
        m.z = z;

        return cam.ScreenToWorldPoint(m);
    }
}
