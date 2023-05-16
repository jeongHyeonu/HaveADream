using TMPro;
using UnityEngine;

public class DistanceManager : Singleton<DistanceManager>
{
    [SerializeField] public GameObject DistanceUI;
    [SerializeField] TextMeshProUGUI distance_text;
    [SerializeField] float distance = 0f; // 유저 이동거리
    [SerializeField] float bossDistance = 0f; // 보스까지 거리

    public bool isGamePlaying = false;
    public bool isBossArrived = false; // 보스까지 도착시 true

    public float GetBossDistance()
    {
        return bossDistance;
    }

    private void OnEnable()
    {
        DistanceUI_ON();

    }
    private void OnDisable()
    {
        DistanceUI_OFF();
    }
    public void DistanceUI_ON()
    {
        DistanceUI.SetActive(true);

        // 초기 이동거리 0으로 세팅
        distance = 0;

        // 보스까지 거리 계산
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        bossDistance = (int)StageDataManager.Instance.GetStageInfo(key)["boss_distance"];
        GameObject.Find("Result2starZone").transform.position = new Vector3(70+bossDistance/5,0);

        isGamePlaying = true;
    }
    public void DistanceUI_OFF()
    {
        DistanceUI.SetActive(false);
        isBossArrived = false;
        isGamePlaying = false;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGamePlaying == false || isBossArrived == true) return; // 게임 실행중이면 실행X / 또는 보스까지 도착했을시 실행 X

        distance += MapMove.Instance.mapSpeed * Time.deltaTime * 4.15f;

        if (bossDistance <= distance)// 보스까지 도달시
        {
            if (isBossArrived) return; // 한번만 실행

            isBossArrived = true;
            distance = bossDistance;
            distance_text.SetText(bossDistance.ToString() + " m");
        }

        distance_text.SetText(Mathf.Round(distance).ToString() + " m");
    }
}
