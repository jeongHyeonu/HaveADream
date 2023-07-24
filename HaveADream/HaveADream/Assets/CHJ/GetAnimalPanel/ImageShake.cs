using UnityEngine;

public class ImageShake : MonoBehaviour
{
    [SerializeField]
    float shakeDuration = 0.5f; // ��鸲 ���� �ð�
    [SerializeField]
    float shakeStrength = 10f; // ��鸲 ����

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private float elapsedTime;

    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    private void Start()
    {
        originalPosition = rectTransform.position; // �ʱ� ��ġ ����
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
            rectTransform.position = originalPosition; // ��鸲 ȿ�� ���� �� �ʱ� ��ġ�� ����
            elapsedTime = 0f; // elapsedTime �ʱ�ȭ
        }
    }
    
}