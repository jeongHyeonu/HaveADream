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
        // 2~4초 뒤에 보석 랜덤 생성
        float randomSpawnDelay = Random.Range(2f,4f);
        yield return new WaitForSeconds(randomSpawnDelay);

        // 랜덤 생성 위치 지정
        float height = 2 * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        float playerDistance = GameObject.Find("Player").transform.position.x - this.transform.position.x;
        float randomY = Random.Range(-height / 4, height / 4);
        Vector2 vec2 = new Vector2(width + playerDistance, randomY);

        // 오브젝트 풀링, 비활성화된 보석 찾아서 active 로 변경 및 위치조정
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

        // 재귀함수로 반복실행
        StartCoroutine(BlueJewel_Randominstantiate());
    }
}
