using UnityEngine;
using UnityEngine.UI;

public class SignalRingController : MonoBehaviour
{
    [Header("참조")]
    public Image ringImage;              // 링 이미지 (없으면 자동으로 GetComponent)

    [Header("색상")]
    public Color idleColor = new Color(0f, 1f, 0.4f);   // 평소 색 (초록)
    public Color dangerColor = new Color(1f, 0.3f, 0f); // 위협 색 (붉은색)

    [Header("웨이브 설정 (좌클릭)")]
    public float waveScaleAmount = 0.12f;   // 얼마나 크게/작게 흔들릴지 (0.05~0.2 사이에서 조절)
    public float waveSpeed = 5f;            // 웨이브 속도

    [Header("위험 신호 설정 (우클릭)")]
    public float dangerDuration = 0.4f;     // 위협 연출 총 시간
    public float dangerMaxScale = 1.3f;     // 최대 확대 배율
    public float dangerMaxOffset = 40f;     // 화면 픽셀 기준으로 얼마나 튈지

    RectTransform rect;
    Vector3 idlePosition;                   // 기본 위치
    float dangerTimer = 0f;
    Vector3 dangerDir;                      // 마우스 방향

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (ringImage == null)
            ringImage = GetComponent<Image>();

        idlePosition = rect.position;       // 시작 위치를 기본 위치로 사용
    }

    void Update()
    {
        // 1) 우클릭 입력 체크: Danger 연출 시작
        if (Input.GetMouseButtonDown(1))
        {
            dangerTimer = dangerDuration;

            // Canvas가 Screen Space - Overlay 일 때는 RectTransform.position이 화면 좌표라 이걸로 충분
            Vector3 mouse = Input.mousePosition;
            dangerDir = (mouse - rect.position).normalized;
        }

        // 2) Danger 상태일 때는 이쪽만 실행, Wave 무시
        if (dangerTimer > 0f)
        {
            PlayDangerAnimation();
            return;
        }

        // 3) Danger 아니면 기본 위치/색상으로 복구
        rect.position = idlePosition;
        ringImage.color = idleColor;

        // 4) 좌클릭 눌린 동안 Wave 상태
        if (Input.GetMouseButton(0))
        {
            PlayWaveAnimation();
        }
        else
        {
            // 클릭 안 하는 동안은 서서히 원래 크기로 복귀
            rect.localScale = Vector3.Lerp(rect.localScale, Vector3.one, Time.deltaTime * 10f);
        }
    }

    void PlayWaveAnimation()
    {
        // PerlinNoise로 부드러운 랜덤값 만들어서 0~1
        float noise = Mathf.PerlinNoise(Time.time * waveSpeed, 0f);
        // -1 ~ 1 범위로 바꾼 다음, waveScaleAmount 만큼 증폭
        float offset = (noise - 0.5f) * 2f * waveScaleAmount;
        float scale = 1f + offset;

        rect.localScale = new Vector3(scale, scale, 1f);
        // 색은 평소대로 유지
    }

    void PlayDangerAnimation()
    {
        dangerTimer -= Time.deltaTime;
        float t = Mathf.Clamp01(1f - (dangerTimer / dangerDuration)); // 0 → 1

        // 0→1→0 곡선 (튀어올랐다가 돌아오는 느낌)
        float curve = Mathf.Sin(t * Mathf.PI);

        // 크기
        float scale = Mathf.Lerp(1f, dangerMaxScale, curve);
        rect.localScale = Vector3.one * scale;

        // 위치: 기본 위치에서 마우스 방향으로 튀었다가 돌아옴
        rect.position = idlePosition + dangerDir * dangerMaxOffset * curve;

        // 색: 초록 → 빨강 → 초록
        ringImage.color = Color.Lerp(idleColor, dangerColor, curve);
    }
}
