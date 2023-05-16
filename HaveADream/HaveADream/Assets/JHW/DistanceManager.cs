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

        // �ʱ� �̵��Ÿ� 0���� ����
        distance = 0;

        // �������� �Ÿ� ���
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
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
        if (isGamePlaying == false || isBossArrived == true) return; // ���� �������̸� ����X / �Ǵ� �������� ���������� ���� X

        distance += MapMove.Instance.mapSpeed * Time.deltaTime * 4.15f;

        if (bossDistance <= distance)// �������� ���޽�
        {
            if (isBossArrived) return; // �ѹ��� ����

            isBossArrived = true;
            distance = bossDistance;
            distance_text.SetText(bossDistance.ToString() + " m");
        }

        distance_text.SetText(Mathf.Round(distance).ToString() + " m");
    }
}
