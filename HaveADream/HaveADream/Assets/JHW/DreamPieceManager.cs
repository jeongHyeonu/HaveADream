using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamPieceManager : Singleton<DreamPieceManager>
{
    [SerializeField] GameObject DreamPiecePool;

    int max_dreampiece_count = 0; // 최대 꿈조각 수
    int dreamPiecePower = 0; // 꿈조각 하나당 공격력 수
    int reqDreamPieceCount = 0; // 꿈조각 필요 개수

    int dreamPieceCnt = 0; // 등장한 꿈조각 카운트

    int boss_distance = 0; // 보스까지 거리

    // 유저가 선택한 스테이지로부터 꿈조각 수 불러오기
    public void StageInfo_dreamPiece_getData()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage();

        max_dreampiece_count = (int)StageDataManager.Instance.GetStageInfo(key)["number_dreampieces"];
        dreamPiecePower = 100; //(int)StageDataManager.Instance.GetStageInfo(key)["dreampiece_power"];
        reqDreamPieceCount = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];
        dreamPieceCnt = 0; // 꿈조각 카운트 0으로 초기화

        StartCoroutine(DreamPiece_Randominstantiate());
    }

    private void OnDisable()
    {
        //StopCoroutine(BlueJewel_Randominstantiate());
    }

    IEnumerator DreamPiece_Randominstantiate()
    {
    // 꿈조각 카운트 증가 및
    // 만약 꿈조각 최대 소환개수 초과시 생성 X
    if (max_dreampiece_count <= dreamPieceCnt++)
        {
            StopCoroutine(DreamPiece_Randominstantiate());
            yield return null;
        }
        else
        {
            // 0.5~1초 뒤에 보석 꿈조각 생성
            float randomSpawnDelay = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(randomSpawnDelay);

            // 랜덤 생성 위치 지정
            float height = 2 * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;

            float playerDistance = GameObject.Find("Player").transform.position.x - this.transform.position.x;
            float randomY = Random.Range(-height / 4, height / 4);
            Vector2 vec2 = new Vector2(width + playerDistance, randomY);
            //if (OcclusionManager.Instance.IsNearObjectOnObstacle(vec2)) // 만약 장애물과 가까운 거리에 생성시
            //{
            //    // Y축 위치 다시 조정해서 다시 생성
            //    randomY = Random.Range(-height / 4, height / 4);
            //    vec2 = new Vector2(width + playerDistance, randomY);
            //};

            // 오브젝트 풀링, 비활성화된 보석 찾아서 active 로 변경 및 위치조정
            GameObject dreamPiece;
            for (int i = 0; i < DreamPiecePool.transform.childCount; i++)
            {
                dreamPiece = DreamPiecePool.transform.GetChild(i).gameObject;
                if (dreamPiece.activeSelf != true)
                {
                    dreamPiece.SetActive(true);
                    dreamPiece.transform.localPosition = vec2;
                    break;
                }
            }

            // 재귀함수로 반복실행
            StartCoroutine(DreamPiece_Randominstantiate());
        }
    }

}
