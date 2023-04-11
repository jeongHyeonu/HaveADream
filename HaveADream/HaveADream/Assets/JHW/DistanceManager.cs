using TMPro;
using UnityEngine;

public class DistanceManager : Singleton<DistanceManager>
{
    [SerializeField] public GameObject DistanceUI;
    [SerializeField] TextMeshProUGUI distance_text;
    [SerializeField] float distance = 0f; // ���� �̵��Ÿ�
    [SerializeField] float bossDistance = 0f; // �������� �Ÿ�

    public bool isGamePlaying = false;
    public bool isBossArrived = false; // �������� ������ true

    public float GetBossDistance()
    {
        return bossDistance;
    }

    public void DistanceUI_ON()
    {
        DistanceUI.SetActive(true);

        // �ʱ� �̵��Ÿ� 0���� ����
        distance = 0;

        // �������� �Ÿ� ���
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
        bossDistance = (int)StageDataManager.Instance.GetStageInfo(key)["boss_distance"];

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
        if (isGamePlaying == false || isBossArrived == true) return; // ���� �������̸� ����X / �Ǵ� �������� ���������� ���� X

        distance += MapMove.Instance.mapSpeed * Time.deltaTime * 4f;
        if (bossDistance <= distance)// �������� ���޽�
        {
            isBossArrived = true;
            distance = bossDistance;
            distance_text.SetText(bossDistance.ToString() + " m");
        }

        distance_text.SetText(Mathf.Round(distance).ToString() + " m");
    }
}
