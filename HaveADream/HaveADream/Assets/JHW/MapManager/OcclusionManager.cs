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

    private int floorSquareCount = 13; // �ٴڿ� ���� �簢�� Ÿ�� ��
    private float floorSize = 2f; // Ÿ�� �簢�� ũ��

    int preset_1_Range = 35; // �����ų ������1 ����
    int preset_3_Range = 8; // �����ų ������3 ����

    public int epiNum; // ������ ������ ���Ǽҵ� ��ȣ
    public int stageNum; // ������ ������ �������� ��ȣ

    private void OnEnable()
    {
        // �����ų ������ ����
        string userCurrentStage = UserDataManager.Instance.GetUserData_userCurrentStage();
        string[] userCurrentStages = userCurrentStage.Split('-');
        epiNum = int.Parse(userCurrentStages[0]); // ������ ������ ���Ǽҵ� ��ȣ
        stageNum = int.Parse(userCurrentStages[1]); // ������ ������ �������� ��ȣ
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
            // ���� �������� �����ߴٸ� ��ֹ� �������� ���ÿ�
            if (DistanceManager.Instance.isBossArrived) return;

            // �� ������ 101~136
            GameObject targetObject_1 = GetRandomMapPreset_1();
            targetObject_1.transform.position = MapPresetSpawnPoint.transform.position;
            targetObject_1.SetActive(true);


            // �� ������ 301~309
            if (stageNum < 10) return; // ���������� ���� 301~309 ��ֹ� ��ȯ x
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
        while (MapPresets_1.transform.GetChild(randInt).gameObject.activeSelf == true) // �ߺ�����
        {
            randInt= Random.Range(0, preset_1_Range);
        }
        return MapPresets_1.transform.GetChild(randInt).gameObject;
    }

    private GameObject GetRandomMapPreset_3()
    {
        if (stageNum >= 10) preset_3_Range = 8;

        int randInt = Random.Range(0, preset_3_Range);
        while (MapPresets_3.transform.GetChild(randInt).gameObject.activeSelf == true) // �ߺ�����
        {
            randInt = Random.Range(0, preset_3_Range);
        }
        return MapPresets_3.transform.GetChild(randInt).gameObject;
    }
}
