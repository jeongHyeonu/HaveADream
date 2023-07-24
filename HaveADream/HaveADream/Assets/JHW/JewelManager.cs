using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelManager : Singleton<JewelManager>
{
    [SerializeField] GameObject BlueJewelPool;
    [SerializeField] GameObject RedJewelPool;

    //
    int max_blueJewel = 0; //�ִ� �Ķ����� ��
    int max_redJewel = 0; //�ִ� �������� ��

    int blueJewelCnt = 0; // ������ �Ķ����� ī��Ʈ
    int redJewelCnt = 0; // ������ �������� ī��Ʈ

    int boss_distance = 0; // �������� �Ÿ�

    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    // ������ ������ ���������κ��� ���� �� �ҷ�����
    public void StageInfo_jewel_getData()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage();

        boss_distance = (int)StageDataManager.Instance.GetStageInfo(key)["boss_distance"];
        max_blueJewel = (int)StageDataManager.Instance.GetStageInfo(key)["number_bluejewel"];
        max_redJewel = (int)StageDataManager.Instance.GetStageInfo(key)["number_redjewel"];
        blueJewelCnt = 0; // �Ķ����� ī��Ʈ 0���� �ʱ�ȭ
        redJewelCnt = 0; // �������� ī��Ʈ 0���� �ʱ�ȭ

        StartCoroutine(BlueJewel_Randominstantiate());
        StartCoroutine(RedJewel_Randominstantiate());
    }

    private void OnDisable()
    {
        //StopCoroutine(BlueJewel_Randominstantiate());
    }

    IEnumerator BlueJewel_Randominstantiate()
    {
        // �Ķ����� ī��Ʈ ���� ��
        // ���� �Ķ����� �ִ� ��ȯ���� �ʰ��� ���� X
        if (max_blueJewel <= blueJewelCnt++)
        {
            StopCoroutine(BlueJewel_Randominstantiate());
            yield return null;
        }
        else
        {
            // 2~3�� �ڿ� ���� ���� ����
            float randomSpawnDelay = Random.Range(2.5f, 5f);
            yield return new WaitForSeconds(randomSpawnDelay);

            // ���� ���� ��ġ ����
            float height = 2 * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;

            float playerDistance = player.transform.position.x - this.transform.position.x;
            float randomY = Random.Range(-height / 4, height / 4);
            Vector2 vec2 = new Vector2(width + playerDistance, randomY);
            //if (OcclusionManager.Instance.IsNearObjectOnObstacle(vec2)) // ���� ��ֹ��� ����� �Ÿ��� ������
            //{
            //    // Y�� ��ġ �ٽ� �����ؼ� �ٽ� ����
            //    randomY = Random.Range(-height / 4, height / 4);
            //    vec2 = new Vector2(vec2.x, randomY);
            //};

            // ������Ʈ Ǯ��, ��Ȱ��ȭ�� ���� ã�Ƽ� active �� ���� �� ��ġ����
            GameObject blueJewel;
            for (int i = 0; i < BlueJewelPool.transform.childCount; i++)
            {
                blueJewel = BlueJewelPool.transform.GetChild(i).gameObject;
                if (blueJewel.activeSelf != true)
                {
                    blueJewel.SetActive(true);
                    blueJewel.transform.localPosition = vec2;
                    break;
                }
            }

            // ����Լ��� �ݺ�����
            StartCoroutine(BlueJewel_Randominstantiate());
        }
    }

    IEnumerator RedJewel_Randominstantiate()
    {
        // �������� ī��Ʈ ���� ��
        // ���� �Ķ����� �ִ� ��ȯ���� �ʰ��� ���� X
        if (max_redJewel <= redJewelCnt++)
        {
            StopCoroutine(RedJewel_Randominstantiate());
            yield return null;
        }
        else
        {
            // 10~20�� �ڿ� ���� ���� ����
            float randomSpawnDelay = Random.Range(5f, 6f);
            yield return new WaitForSeconds(randomSpawnDelay);

            // ���� ���� ��ġ ����
            float height = 2 * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;

            float playerDistance = player.transform.position.x - this.transform.position.x;
            float randomY = Random.Range(-height / 4, height / 4);
            Vector2 vec2 = new Vector2(width + playerDistance, randomY);
            //if (OcclusionManager.Instance.IsNearObjectOnObstacle(vec2)) // ���� ��ֹ��� ����� �Ÿ��� ������ �ٽû���
            //{
            //    vec2 = new Vector2(vec2.x + 10, vec2.y);
            //};

            // ������Ʈ Ǯ��, ��Ȱ��ȭ�� ���� ã�Ƽ� active �� ���� �� ��ġ����
            GameObject redJewel;
            for (int i = 0; i < BlueJewelPool.transform.childCount; i++)
            {
                redJewel = RedJewelPool.transform.GetChild(i).gameObject;
                if (redJewel.activeSelf != true)
                {
                    redJewel.SetActive(true);
                    redJewel.transform.localPosition = vec2;
                    break;
                }
            }

            // ����Լ��� �ݺ�����
            StartCoroutine(RedJewel_Randominstantiate());
        }
    }
}
