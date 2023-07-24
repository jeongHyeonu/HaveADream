using UnityEngine;

public class ImageShake : MonoBehaviour
{
    [SerializeField]
    float shakeDuration = 0.5f; // 흔들림 지속 시간
    [SerializeField]
    float shakeStrength = 10f; // 흔들림 강도

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private float elapsedTime;

    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    private void Start()
    {
        originalPosition = rectTransform.position; // 초기 위치 저장
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < shakeDuration)
        {
            float t = elapsedTime / shakeDuration;
            float strength = Mathf.Lerp(shakeStrength, 0f, t);

            Vector3 randomOffset = Random.insideUnitCircle * strength;
            rectTransform.position = originalPosition + randomOffset;
        }
        else
        {
            rectTransform.position = originalPosition; // 흔들림 효과 종료 후 초기 위치로 복원
            elapsedTime = 0f; // elapsedTime 초기화
        }
    }
    
}