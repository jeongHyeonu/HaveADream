using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionManager : Singleton<OcclusionManager>
{
    [SerializeField] GameObject MapPresets_1;
    [SerializeField] GameObject MapPresets_2;
    [SerializeField] GameObject MapPresets_3;
    [SerializeField] GameObject MapPresets_4;
    [SerializeField] GameObject MapPresetSpawnPoint;

    public GameObject currentObstaclePreset_1; // 현재 맵에 등장한 장애물 프리셋
    int presetCnt=0; // 프리셋 등장 횟수
    int presetCnt_1_inCam = 0; // 화면 내에 표시된 프리셋(101~136) 수

    private int floorSquareCount = 13; // 바닥에 놓인 사각형 타일 수
    private float floorSize = 2f; // 타일 사각형 크기

    int preset_1_Range = 35; // 등장시킬 프리셋1 범위
    int preset_3_Range = 8; // 등장시킬 프리셋3 범위

    public int epiNum; // 유저가 선택한 에피소드 번호
    public int stageNum; // 유저가 선택한 스테이지 번호

    Queue<GameObject> mapPresetQueue1 = new Queue<GameObject>();// 생산된 맵 프리셋 담기
    Queue<GameObject> mapPresetQueue3 = new Queue<GameObject>();// 생산된 맵 프리셋 담기

    private void OnEnable()
    {
        // 등장시킬 프리셋 결정
        string userCurrentStage = UserDataManager.Instance.GetUserData_userCurrentStage();
        string[] userCurrentStages = userCurrentStage.Split('-');
        epiNum = int.Parse(userCurrentStages[0]); // 유저가 선택한 에피소드 번호
        stageNum = int.Parse(userCurrentStages[1]); // 유저가 선택한 스테이지 번호

        // 맵 프리셋의 첫번째 프리셋은 반드시 등장시킬 것! (없으면 다음 장애물 생성 못함)
        currentObstaclePreset_1 = MapPresets_1.transform.GetChild(0).gameObject;
        currentObstaclePreset_1.gameObject.SetActive(true);
        presetCnt = 1;// 프리셋 등장 카운트

        // 맵 프리셋1 102부터 최대까지 초기화
        for (int i = 1; i < MapPresets_1.transform.childCount-1; i++)
            MapPresets_1.transform.GetChild(i).gameObject.SetActive(false);

        // 맵 프리셋1은 기본으로 넣는다
        mapPresetQueue1.Enqueue(MapPresets_1.transform.GetChild(0).gameObject);
        MapPresets_1.transform.GetChild(0).position = Vector2.zero;
    }

    private void OnDisable()
    {
        for (int i = 0; i < mapPresetQueue1.Count; i++) mapPresetQueue1.Dequeue().SetActive(false);
        for (int i = 0; i < mapPresetQueue3.Count; i++) mapPresetQueue3.Dequeue().SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapFloor"))
        {
            GameObject targetObject = collision.gameObject.transform.parent.GetChild(floorSquareCount).gameObject;
            collision.transform.position = new Vector3(targetObject.transform.position.x, collision.transform.position.y);
            collision.transform.SetAsLastSibling();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapPresetStartPoint")) // 프리셋 생성
        {
            // 만약 보스까지 도달했다면 장애물 생성하지 마시오
            if (DistanceManager.Instance.isBossArrived) return;

            // 맵 프리셋 101~136
            currentObstaclePreset_1 = GetRandomMapPreset_1();
            currentObstaclePreset_1.transform.position = new Vector2(MapPresetSpawnPoint.transform.position.x, MapPresetSpawnPoint.transform.position.y);
            currentObstaclePreset_1.SetActive(true);

            // 큐에 생성된 장애물 담기
            mapPresetQueue1.Enqueue(currentObstaclePreset_1);

            // 맵 프리셋 301~309// 스테이지에 따라 301~309 장애물 소환 x
            if (stageNum >= 10)
            {
                GameObject targetObject_3 = GetRandomMapPreset_3();
                targetObject_3.transform.position = MapPresetSpawnPoint.transform.position;
                targetObject_3.SetActive(true);

                // 큐에 생성된 장애물 담기
                mapPresetQueue3.Enqueue(targetObject_3);
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapPresetEndPoint")) // 프리셋 제거
        {
            // 큐에 장애물 없으면 실행 X
            if (mapPresetQueue1.Count == 0) return;
            if (mapPresetQueue3.Count == 0) return;

            // 큐에 생성된 장애물 빼기
            if (collision.transform.parent.name.Contains("Map1")) mapPresetQueue1.Dequeue().SetActive(false);
            if (collision.transform.parent.name.Contains("Map3")) mapPresetQueue3.Dequeue().SetActive(false);

            // 맵 프리셋의 첫번째 프리셋은 반드시 등장시킬 것! (없으면 다음 장애물 생성 못함)
            if (presetCnt++ == 0) return;
            collision.transform.parent.gameObject.SetActive(false);

            // 화면 내 프리셋 수 감소
            presetCnt_1_inCam--;
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("currency")) // 재화와 닿을시 단순히 비활성화
        {
            collision.gameObject.SetActive(false);
        }
    }

    private GameObject GetRandomMapPreset_1()
    {
        if (stageNum <= 10) preset_1_Range = 35;
        else preset_1_Range = 11;

        int randInt = Random.Range(0, preset_1_Range);
        while (MapPresets_1.transform.GetChild(randInt).gameObject.activeSelf == true) // 중복제거
        {
            randInt= Random.Range(0, preset_1_Range);
        }
        currentObstaclePreset_1 = MapPresets_1.transform.GetChild(randInt).gameObject;
        return currentObstaclePreset_1;
    }

    private GameObject GetRandomMapPreset_3()
    {
        if (stageNum >= 10) preset_3_Range = 8;

        int randInt = Random.Range(0, preset_3_Range);
        while (MapPresets_3.transform.GetChild(randInt).gameObject.activeSelf == true) // 중복제거
        {
            randInt = Random.Range(0, preset_3_Range);
        }
        return MapPresets_3.transform.GetChild(randInt).gameObject;
    }

    // 장애물과 가까운 거리에 있다면 true, 그렇지 않다면 false 리턴
    // 장애물과 가까운 오브젝트인지 검사, 이때 매개변수는 넘어오는 오브젝트의 위치(벡터)값
    public bool IsNearObjectOnObstacle(Vector2 _checkPos)
    {
        // 맵 끝점
        int endPoint = currentObstaclePreset_1.transform.childCount;

        // 가까운 거리인지 체크하는 기준값
        float crit = 3f;

        // 장애물 프리셋 오브젝트들 검사
        // 시작점, 끝점 제외
        for (int i=1; i < endPoint-1; i++)
        {
            float playerMove = GameObject.Find("Player").transform.position.x - GameObject.Find("Map").transform.position.x;
            Vector2 _obstaclePos = new Vector2(currentObstaclePreset_1.transform.GetChild(i).gameObject.transform.position.x + playerMove, currentObstaclePreset_1.transform.GetChild(i).gameObject.transform.position.y);
            float distance = Vector2.Distance(_obstaclePos, _checkPos);
            Debug.Log(distance);
            if (crit > distance) return true;
        }

        // 가까운 거리에 있지 않다면 false 리턴
        return false;
    }
}
