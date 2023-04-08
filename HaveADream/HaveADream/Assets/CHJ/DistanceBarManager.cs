using UnityEngine;
using UnityEngine.UI;

public class DistanceBarManager : Singleton<DistanceBarManager>
{
    [SerializeField] public GameObject DistanceBarUI;
    [SerializeField] Image distanceBar;
    [SerializeField] float distance = 0f; // ���� �̵��Ÿ�
    [SerializeField] float bossDistance = 0f; // �������� �Ÿ�

    public bool isGamePlaying = false;
    public bool isBossArrived = false; // �������� ������ true

    private void OnEnable()
    {
        distance = 0;
    }
    private void Start()
    {
        distanceBar.GetComponent<Image>();
    }
    public void DistanceBarUI_ON()
    {
        DistanceBarUI.SetActive(true);

        // �ʱ� �̵��Ÿ� 0���� ����
        distance = 0;

        // �������� �Ÿ� ���
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
        bossDistance = (int)StageDataManager.Instance.GetStageInfo(key)["boss_distance"];

        isGamePlaying = true;
    }
    public void DistanceBarUI_OFF()
    {
        DistanceBarUI.SetActive(false);
        isGamePlaying = false;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (isBossArrived == true) return; // ���� �������̸� ����X / �Ǵ� �������� ���������� ���� X

        distance += MapMove.Instance.mapSpeed * Time.deltaTime * 4f;


        if (bossDistance <= distance)// �������� ���޽�
        {
            isBossArrived = true;
            distance = bossDistance;
            distanceBar.fillAmount = 1.0f;
        }


        distanceBar.fillAmount = distance / bossDistance;
    }
    
}
