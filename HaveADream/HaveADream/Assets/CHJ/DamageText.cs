using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    
    Color alpha;
    //[SerializeField] private GameObject damageText;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 0.5f;

        Invoke("DestroyObject", destroyTime);
    }

    void Enable()
    {
        // ���� ���� ���͸� �����Ͽ� �ؽ�Ʈ ��ġ�� ���մϴ�
        Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0) + randomOffset); // �ؽ�Ʈ ��ġ
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // �ؽ�Ʈ ��ġ

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // �ؽ�Ʈ ���İ�
        
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
