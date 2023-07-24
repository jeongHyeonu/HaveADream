using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class BoxClick : MonoBehaviour
{
    [SerializeField]
    GameObject boxImage; // 상자 이미지 오브젝트
    [SerializeField]
    GameObject getAnimal; // GetAnimal 오브젝트

    // 흔들림의 지속시간
    [SerializeField] float shakeDuration = 1.5f;

    // 흔들림의 강도
    [SerializeField] float shakeStrength = 0.5f;

    // 흔들림 횟수
    [SerializeField] int shakeVibrato = 5;

    private void Start()
    {
        SkillManager.Instance.UI_Off();
    }

    public void OnBoxClick()
    {
        StartCoroutine(ShakeBox());
        
    }

    public void OnPopUpClick()
    {

    }

    private IEnumerator ShakeBox()
    {
        RectTransform rect = boxImage.GetComponent<RectTransform>();
        Vector3 originalPosition = rect.anchoredPosition;

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        while (stopwatch.Elapsed.TotalSeconds < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeStrength;
            float y = Random.Range(-1f, 1f) * shakeStrength;

            rect.anchoredPosition = new Vector3(x, y, originalPosition.z);

            yield return null;
        }

        rect.anchoredPosition = originalPosition;

        boxImage.SetActive(false); // 상자 비활성화
        getAnimal.SetActive(true); // GetAnimal 활성화

        stopwatch.Stop();
    }
}