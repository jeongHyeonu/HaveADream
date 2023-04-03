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

    private int floorSquareCount = 13; // 바닥에 놓인 사각형 타일 수
    private float floorSize = 2f; // 타일 사각형 크기

    int preset_1_Range = 35; // 등장시킬 프리셋1 범위
    int preset_3_Range = 8; // 등장시킬 프리셋3 범위

    public int epiNum; // 유저가 선택한 에피소드 번호
    public int stageNum; // 유저가 선택한 스테이지 번호

    private void OnEnable()
    {
        // 등장시킬 프리셋 결정
        string userCurrentStage = UserDataManager.Instance.GetUserData_userCurrentStage();
        string[] userCurrentStages = userCurrentStage.Split('-');
        epiNum = int.Parse(userCurrentStages[0]); // 유저가 선택한 에피소드 번호
        stageNum = int.Parse(userCurrentStages[1]); // 유저가 선택한 스테이지 번호
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapFloor"))
        {
            GameObject targetObject = collision.gameObject.transform.parent.GetChild(floorSquareCount).gameObject;
            collision.transform.position = new Vector3(targetObject.transform.position.x+ floorSize, collision.transform.position.y);
            collision.transform.SetAsLastSibling();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapPresetStartPoint"))
        {
            // 만약 보스까지 도달했다면 장애물 생성하지 마시오
            if (DistanceManager.Instance.isBossArrived) return;

            // 맵 프리셋 101~136
            GameObject targetObject_1 = GetRandomMapPreset_1();
            targetObject_1.transform.position = MapPresetSpawnPoint.transform.position;
            targetObject_1.SetActive(true);


            // 맵 프리셋 301~309
            if (stageNum < 10) return; // 스테이지에 따라 301~309 장애물 소환 x
            GameObject targetObject_3 = GetRandomMapPreset_3();
            targetObject_3.transform.position = MapPresetSpawnPoint.transform.position;
            targetObject_3.SetActive(true);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapPresetEndPoint"))
        {
            collision.transform.parent.gameObject.SetActive(false);

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
        return MapPresets_1.transform.GetChild(randInt).gameObject;
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
}
