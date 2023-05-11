using UnityEngine;
using UnityEngine.UI;

public class DistanceBarManager : Singleton<DistanceBarManager>
{
    [SerializeField] public GameObject DistanceBarUI;
    [SerializeField] Image distanceBar;
    [SerializeField] float distance = 0f; // 유저 이동거리
    [SerializeField] float bossDistance = 0f; // 보스까지 거리

    private bool isGamePlaying = false;
    private bool isBossArrived = false; // 보스까지 도착시 true


    public float GetBossDistance()
    {
        return bossDistance;
    }



    private void OnEnable()
    {
        DistanceBarUI_ON();

    }

    private void OnDisable()
    {
        DistanceBarUI_OFF();
    }


    public void DistanceBarUI_ON()
    {

        DistanceBarUI.SetActive(true);
        distanceBar.GetComponent<Image>();
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
        if (isBossArrived == true) return;

        distance += MapMove.Instance.mapSpeed * Time.deltaTime * 4.5f;


        if (bossDistance <= distance)// 보스까지 도달시
        {
            isBossArrived = true;
            distance = bossDistance;
            distanceBar.fillAmount = 1.0f;
        }


        distanceBar.fillAmount = distance / bossDistance;
    }


}