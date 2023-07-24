using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelManager : Singleton<JewelManager>
{
    [SerializeField] GameObject BlueJewelPool;
    [SerializeField] GameObject RedJewelPool;

    //
    int max_blueJewel = 0; //최대 파란보석 수
    int max_redJewel = 0; //최대 빨간보석 수

    int blueJewelCnt = 0; // 스폰된 파란보석 카운트
    int redJewelCnt = 0; // 스폰된 빨간보석 카운트

    int boss_distance = 0; // 보스까지 거리

    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    // 유저가 선택한 스테이지로부터 보석 수 불러오기
    public void StageInfo_jewel_getData()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage();

        boss_distance = (int)StageDataManager.Instance.GetStageInfo(key)["boss_distance"];
        max_blueJewel = (int)StageDataManager.Instance.GetStageInfo(key)["number_bluejewel"];
        max_redJewel = (int)StageDataManager.Instance.GetStageInfo(key)["number_redjewel"];
        blueJewelCnt = 0; // 파란보석 카운트 0으로 초기화
        redJewelCnt = 0; // 빨간보석 카운트 0으로 초기화

        StartCoroutine(BlueJewel_Randominstantiate());
        StartCoroutine(RedJewel_Randominstantiate());
    }

    private void OnDisable()
    {
        //StopCoroutine(BlueJewel_Randominstantiate());
    }

    IEnumerator BlueJewel_Randominstantiate()
    {
        // 파란보석 카운트 증가 및
        // 만약 파란보석 최대 소환개수 초과시 생성 X
        if (max_blueJewel <= blueJewelCnt++)
        {
            StopCoroutine(BlueJewel_Randominstantiate());
            yield return null;
        }
        else
        {
            // 2~3초 뒤에 보석 랜덤 생성
            float randomSpawnDelay = Random.Range(2.5f, 5f);
            yield return new WaitForSeconds(randomSpawnDelay);

            // 랜덤 생성 위치 지정
            float height = 2 * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;

            float playerDistance = player.transform.position.x - this.transform.position.x;
            float randomY = Random.Range(-height / 4, height / 4);
            Vector2 vec2 = new Vector2(width + playerDistance, randomY);
            //if (OcclusionManager.Instance.IsNearObjectOnObstacle(vec2)) // 만약 장애물과 가까운 거리에 생성시
            //{
            //    // Y축 위치 다시 조정해서 다시 생성
            //    randomY = Random.Range(-height / 4, height / 4);
            //    vec2 = new Vector2(vec2.x, randomY);
            //};

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

    IEnumerator RedJewel_Randominstantiate()
    {
        // 빨간보석 카운트 증가 및
        // 만약 파란보석 최대 소환개수 초과시 생성 X
        if (max_redJewel <= redJewelCnt++)
        {
            StopCoroutine(RedJewel_Randominstantiate());
            yield return null;
        }
        else
        {
            // 10~20초 뒤에 보석 랜덤 생성
            float randomSpawnDelay = Random.Range(5f, 6f);
            yield return new WaitForSeconds(randomSpawnDelay);

            // 랜덤 생성 위치 지정
            float height = 2 * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;

            float playerDistance = player.transform.position.x - this.transform.position.x;
            float randomY = Random.Range(-height / 4, height / 4);
            Vector2 vec2 = new Vector2(width + playerDistance, randomY);
            //if (OcclusionManager.Instance.IsNearObjectOnObstacle(vec2)) // 만약 장애물과 가까운 거리에 생성시 다시생성
            //{
            //    vec2 = new Vector2(vec2.x + 10, vec2.y);
            //};

            // 오브젝트 풀링, 비활성화된 보석 찾아서 active 로 변경 및 위치조정
            GameObject redJewel;
            for (int i = 0; i < BlueJewelPool.transform.childCount; i++)
            {
                redJewel = RedJewelPool.transform.GetChild(i).gameObject;
                if (redJewel.activeSelf != true)
                {
                    redJewel.SetActive(true);
                    redJewel.transform.localPosition = vec2;
                    break;
                }
            }

            // 재귀함수로 반복실행
            StartCoroutine(RedJewel_Randominstantiate());
        }
    }
}
