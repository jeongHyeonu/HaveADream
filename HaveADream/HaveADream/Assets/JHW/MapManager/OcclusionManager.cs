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

    public GameObject currentObstaclePreset_1; // ���� �ʿ� ������ ��ֹ� ������
    int presetCnt=0; // ������ ���� Ƚ��
    int presetCnt_1_inCam = 0; // ȭ�� ���� ǥ�õ� ������(101~136) ��

    private int floorSquareCount = 13; // �ٴڿ� ���� �簢�� Ÿ�� ��
    private float floorSize = 2f; // Ÿ�� �簢�� ũ��

    int preset_1_Range = 35; // �����ų ������1 ����
    int preset_3_Range = 8; // �����ų ������3 ����

    public int epiNum; // ������ ������ ���Ǽҵ� ��ȣ
    public int stageNum; // ������ ������ �������� ��ȣ

    Queue<GameObject> mapPresetQueue1 = new Queue<GameObject>();// ����� �� ������ ���
    Queue<GameObject> mapPresetQueue3 = new Queue<GameObject>();// ����� �� ������ ���

    private void OnEnable()
    {
        // �����ų ������ ����
        string userCurrentStage = UserDataManager.Instance.GetUserData_userCurrentStage();
        string[] userCurrentStages = userCurrentStage.Split('-');
        epiNum = int.Parse(userCurrentStages[0]); // ������ ������ ���Ǽҵ� ��ȣ
        stageNum = int.Parse(userCurrentStages[1]); // ������ ������ �������� ��ȣ

        // �� �������� ù��° �������� �ݵ�� �����ų ��! (������ ���� ��ֹ� ���� ����)
        currentObstaclePreset_1 = MapPresets_1.transform.GetChild(0).gameObject;
        currentObstaclePreset_1.gameObject.SetActive(true);
        presetCnt = 1;// ������ ���� ī��Ʈ

        // �� ������1 102���� �ִ���� �ʱ�ȭ
        for (int i = 1; i < MapPresets_1.transform.childCount-1; i++)
            MapPresets_1.transform.GetChild(i).gameObject.SetActive(false);

        // �� ������1�� �⺻���� �ִ´�
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapPresetStartPoint")) // ������ ����
        {
            // ���� �������� �����ߴٸ� ��ֹ� �������� ���ÿ�
            if (DistanceManager.Instance.isBossArrived) return;

            // �� ������ 101~136
            currentObstaclePreset_1 = GetRandomMapPreset_1();
            currentObstaclePreset_1.transform.position = new Vector2(MapPresetSpawnPoint.transform.position.x, MapPresetSpawnPoint.transform.position.y);
            currentObstaclePreset_1.SetActive(true);

            // ť�� ������ ��ֹ� ���
            mapPresetQueue1.Enqueue(currentObstaclePreset_1);

            // �� ������ 301~309// ���������� ���� 301~309 ��ֹ� ��ȯ x
            if (stageNum >= 10)
            {
                GameObject targetObject_3 = GetRandomMapPreset_3();
                targetObject_3.transform.position = MapPresetSpawnPoint.transform.position;
                targetObject_3.SetActive(true);

                // ť�� ������ ��ֹ� ���
                mapPresetQueue3.Enqueue(targetObject_3);
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapPresetEndPoint")) // ������ ����
        {
            // ť�� ��ֹ� ������ ���� X
            if (mapPresetQueue1.Count == 0) return;
            if (mapPresetQueue3.Count == 0) return;

            // ť�� ������ ��ֹ� ����
            if (collision.transform.parent.name.Contains("Map1")) mapPresetQueue1.Dequeue().SetActive(false);
            if (collision.transform.parent.name.Contains("Map3")) mapPresetQueue3.Dequeue().SetActive(false);

            // �� �������� ù��° �������� �ݵ�� �����ų ��! (������ ���� ��ֹ� ���� ����)
            if (presetCnt++ == 0) return;
            collision.transform.parent.gameObject.SetActive(false);

            // ȭ�� �� ������ �� ����
            presetCnt_1_inCam--;
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("currency")) // ��ȭ�� ������ �ܼ��� ��Ȱ��ȭ
        {
            collision.gameObject.SetActive(false);
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
        currentObstaclePreset_1 = MapPresets_1.transform.GetChild(randInt).gameObject;
        return currentObstaclePreset_1;
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

    // ��ֹ��� ����� �Ÿ��� �ִٸ� true, �׷��� �ʴٸ� false ����
    // ��ֹ��� ����� ������Ʈ���� �˻�, �̶� �Ű������� �Ѿ���� ������Ʈ�� ��ġ(����)��
    public bool IsNearObjectOnObstacle(Vector2 _checkPos)
    {
        // �� ����
        int endPoint = currentObstaclePreset_1.transform.childCount;

        // ����� �Ÿ����� üũ�ϴ� ���ذ�
        float crit = 3f;

        // ��ֹ� ������ ������Ʈ�� �˻�
        // ������, ���� ����
        for (int i=1; i < endPoint-1; i++)
        {
            float playerMove = GameObject.Find("Player").transform.position.x - GameObject.Find("Map").transform.position.x;
            Vector2 _obstaclePos = new Vector2(currentObstaclePreset_1.transform.GetChild(i).gameObject.transform.position.x + playerMove, currentObstaclePreset_1.transform.GetChild(i).gameObject.transform.position.y);
            float distance = Vector2.Distance(_obstaclePos, _checkPos);
            Debug.Log(distance);
            if (crit > distance) return true;
        }

        // ����� �Ÿ��� ���� �ʴٸ� false ����
        return false;
    }
}
