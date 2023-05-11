using UnityEngine;
using UnityEngine.UI;

public class DistanceBarManager : Singleton<DistanceBarManager>
{
    [SerializeField] public GameObject DistanceBarUI;
    [SerializeField] Image distanceBar;
    [SerializeField] float distance = 0f; // ���� �̵��Ÿ�
    [SerializeField] float bossDistance = 0f; // �������� �Ÿ�

    private bool isGamePlaying = false;
    private bool isBossArrived = false; // �������� ������ true


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
        // �ʱ� �̵��Ÿ� 0���� ����
        distance = 0;
        distanceBar.fillAmount = 0.0f;

        // �������� �Ÿ� ���
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
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


        if (bossDistance <= distance)// �������� ���޽�
        {
            isBossArrived = true;
            distance = bossDistance;
            distanceBar.fillAmount = 1.0f;
        }


        distanceBar.fillAmount = distance / bossDistance;
    }


}