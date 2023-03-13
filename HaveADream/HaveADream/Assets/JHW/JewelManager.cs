using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelManager : MonoBehaviour
{
    [SerializeField] GameObject BlueJewelPool;
    [SerializeField] GameObject RedJewelPool;

    private void OnEnable()
    {
        StartCoroutine(BlueJewel_Randominstantiate());
    }

    IEnumerator BlueJewel_Randominstantiate()
    {
        // 2~4�� �ڿ� ���� ���� ����
        float randomSpawnDelay = Random.Range(2f,4f);
        yield return new WaitForSeconds(randomSpawnDelay);

        // ���� ���� ��ġ ����
        float height = 2 * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        float playerDistance = GameObject.Find("Player").transform.position.x - this.transform.position.x;
        float randomY = Random.Range(-height / 4, height / 4);
        Vector2 vec2 = new Vector2(width + playerDistance, randomY);

        // ������Ʈ Ǯ��, ��Ȱ��ȭ�� ���� ã�Ƽ� active �� ���� �� ��ġ����
        GameObject blueJewel;
        for (int i = 0; i < BlueJewelPool.transform.childCount; i++)
        {
            blueJewel = BlueJewelPool.transform.GetChild(i).gameObject;
            if (blueJewel.activeSelf != true)
            {
                blueJewel.SetActive(true);
                blueJewel.transform.localPosition = vec2;
                break;
            }
        }

        // ����Լ��� �ݺ�����
        StartCoroutine(BlueJewel_Randominstantiate());
    }
}
