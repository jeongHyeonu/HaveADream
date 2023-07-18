using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class WingManager : Singleton<WingManager>
{
    [SerializeField] GameObject WingPool;

    int max_wingCnt = 0; // 최대 날개 수

    int wingCnt = 0; // 수집한 날개 수 카운트

    int boss_distance = 0; // 보스까지 거리
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    // 유저가 선택한 스테이지로부터 날개 수 불러오기
    public void StageInfo_wing_getData()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage();

        max_wingCnt = (int)StageDataManager.Instance.GetStageInfo(key)["number_wings"];
        wingCnt = 0; // 등장한 날개 수 카운트 0으로 초기화

        StartCoroutine(Wing_Randominstantiate());
    }

    private void OnDisable()
    {
        //StopCoroutine(BlueJewel_Randominstantiate());
    }

    IEnumerator Wing_Randominstantiate()
    {
        // 날개 수 카운트 증가 및
        // 만약 날개 수 최대 소환개수 초과시 생성 X
        if (max_wingCnt <= wingCnt++)
        {
            StopCoroutine(Wing_Randominstantiate());
            yield return null;
        }
        else
        {
            // 1~3초 뒤에 날개 생성
            float randomSpawnDelay = Random.Range(1f, 3f);
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
            //    vec2 = new Vector2(width + playerDistance, randomY);
            //};

            // 오브젝트 풀링, 비활성화된 날개 찾아서 active 로 변경 및 위치조정
            GameObject wingObj;
            for (int i = 0; i < WingPool.transform.childCount; i++)
            {
                wingObj = WingPool.transform.GetChild(i).gameObject;
                if (wingObj.activeSelf != true)
                {
                    wingObj.SetActive(true);
                    wingObj.transform.localPosition = vec2;
                    break;
                }
            }

            // 재귀함수로 반복실행
            StartCoroutine(Wing_Randominstantiate());
        }
    }
}
