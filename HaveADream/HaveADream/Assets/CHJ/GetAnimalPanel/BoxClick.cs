using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class BoxClick : MonoBehaviour
{
    [SerializeField]
    GameObject boxImage; // ���� �̹��� ������Ʈ
    [SerializeField]
    GameObject getAnimal; // GetAnimal ������Ʈ

    // ��鸲�� ���ӽð�
    [SerializeField] float shakeDuration = 1.5f;

    // ��鸲�� ����
    [SerializeField] float shakeStrength = 0.5f;

    // ��鸲 Ƚ��
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

        boxImage.SetActive(false); // ���� ��Ȱ��ȭ
        getAnimal.SetActive(true); // GetAnimal Ȱ��ȭ

        stopwatch.Stop();
    }
}