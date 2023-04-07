using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WingManager : Singleton<WingManager>
{
    [SerializeField] GameObject WingPool;

    int max_wingCnt = 0; // �ִ� ���� ��

    int wingCnt = 0; // ������ ���� �� ī��Ʈ

    int boss_distance = 0; // �������� �Ÿ�

    // ������ ������ ���������κ��� ���� �� �ҷ�����
    public void StageInfo_wing_getData()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage();

        max_wingCnt = (int)StageDataManager.Instance.GetStageInfo(key)["number_wings"];
        wingCnt = 0; // ������ ���� �� ī��Ʈ 0���� �ʱ�ȭ

        StartCoroutine(Wing_Randominstantiate());
    }

    private void OnDisable()
    {
        //StopCoroutine(BlueJewel_Randominstantiate());
    }

    IEnumerator Wing_Randominstantiate()
    {
        // ���� �� ī��Ʈ ���� ��
        // ���� ���� �� �ִ� ��ȯ���� �ʰ��� ���� X
        if (max_wingCnt <= wingCnt++)
        {
            StopCoroutine(Wing_Randominstantiate());
            yield return null;
        }
        else
        {
            // 1~3�� �ڿ� ���� ����
            float randomSpawnDelay = Random.Range(1f, 3f);
            yield return new WaitForSeconds(randomSpawnDelay);

            // ���� ���� ��ġ ����
            float height = 2 * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;

            float playerDistance = GameObject.Find("Player").transform.position.x - this.transform.position.x;
            float randomY = Random.Range(-height / 4, height / 4);
            Vector2 vec2 = new Vector2(width + playerDistance, randomY);
            if (OcclusionManager.Instance.IsNearObjectOnObstacle(vec2)) // ���� ��ֹ��� ����� �Ÿ��� ������
            {
                while (OcclusionManager.Instance.IsNearObjectOnObstacle(vec2) == false)
                    vec2 = new Vector2(vec2.x + 10, vec2.y);
            };

            // ������Ʈ Ǯ��, ��Ȱ��ȭ�� ���� ã�Ƽ� active �� ���� �� ��ġ����
            GameObject wingObj;
            for (int i = 0; i < WingPool.transform.childCount; i++)
            {
                wingObj = WingPool.transform.GetChild(i).gameObject;
                if (wingObj.activeSelf != true)
                {
                    wingObj.SetActive(true);
                    wingObj.transform.localPosition = vec2;
                    break;
                }
            }

            // ����Լ��� �ݺ�����
            StartCoroutine(Wing_Randominstantiate());
        }
    }
}
