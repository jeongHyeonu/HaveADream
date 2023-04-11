using UnityEngine;
using UnityEngine.UI;

public class DistanceBarManager : Singleton<DistanceBarManager>
{
    [SerializeField] public GameObject DistanceBarUI;
    [SerializeField] Image distanceBar;
    [SerializeField] float distance = 0f; // 유저 이동거리
    [SerializeField] float bossDistance = 0f; // 보스까지 거리

    public bool isGamePlaying = false;
    public bool isBossArrived = false; // 보스까지 도착시 true

    public float GetBossDistance()
    {
        return bossDistance;
    }
    private void Start()
    {
        distanceBar.GetComponent<Image>();
    }

    public void DistanceBarUI_ON()
    {
        DistanceBarUI.SetActive(true);

        // 초기 이동거리 0으로 세팅
        distance = 0;
        distanceBar.fillAmount = 0.0f;

        // 보스까지 거리 계산
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        bossDistance = (int)StageDataManager.Instance.GetStageInfo(key)["boss_distance"];

        isGamePlaying = true;
    }
    public void DistanceBarUI_OFF()
    {
        DistanceBarUI.SetActive(false);
        isBossArrived = false;
        isGamePlaying = false;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (isBossArrived == true) return; // 게임 실행중이면 실행X / 또는 보스까지 도착했을시 실행 X

        distance += MapMove.Instance.mapSpeed * Time.deltaTime * 4f;


        if (bossDistance <= distance)// 보스까지 도달시
        {
            isBossArrived = true;
            distance = bossDistance;
            distanceBar.fillAmount = 1.0f;
        }


        distanceBar.fillAmount = distance / bossDistance;
    }
}
