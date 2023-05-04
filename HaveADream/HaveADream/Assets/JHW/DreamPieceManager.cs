using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamPieceManager : Singleton<DreamPieceManager>
{
    [SerializeField] GameObject DreamPiecePool;

    int max_dreampiece_count = 0; // �ִ� ������ ��
    int dreamPiecePower = 0; // ������ �ϳ��� ���ݷ� ��
    int reqDreamPieceCount = 0; // ������ �ʿ� ����

    int dreamPieceCnt = 0; // ������ ������ ī��Ʈ

    int boss_distance = 0; // �������� �Ÿ�

    // ������ ������ ���������κ��� ������ �� �ҷ�����
    public void StageInfo_dreamPiece_getData()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage();

        max_dreampiece_count = (int)StageDataManager.Instance.GetStageInfo(key)["number_dreampieces"];
        dreamPiecePower = 100; //(int)StageDataManager.Instance.GetStageInfo(key)["dreampiece_power"];
        reqDreamPieceCount = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];
        dreamPieceCnt = 0; // ������ ī��Ʈ 0���� �ʱ�ȭ

        StartCoroutine(DreamPiece_Randominstantiate());
    }

    private void OnDisable()
    {
        //StopCoroutine(BlueJewel_Randominstantiate());
    }

    IEnumerator DreamPiece_Randominstantiate()
    {
    // ������ ī��Ʈ ���� ��
    // ���� ������ �ִ� ��ȯ���� �ʰ��� ���� X
    if (max_dreampiece_count <= dreamPieceCnt++)
        {
            StopCoroutine(DreamPiece_Randominstantiate());
            yield return null;
        }
        else
        {
            // 0.5~1�� �ڿ� ���� ������ ����
            float randomSpawnDelay = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(randomSpawnDelay);

            // ���� ���� ��ġ ����
            float height = 2 * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;

            float playerDistance = GameObject.Find("Player").transform.position.x - this.transform.position.x;
            float randomY = Random.Range(-height / 4, height / 4);
            Vector2 vec2 = new Vector2(width + playerDistance, randomY);
            //if (OcclusionManager.Instance.IsNearObjectOnObstacle(vec2)) // ���� ��ֹ��� ����� �Ÿ��� ������
            //{
            //    // Y�� ��ġ �ٽ� �����ؼ� �ٽ� ����
            //    randomY = Random.Range(-height / 4, height / 4);
            //    vec2 = new Vector2(width + playerDistance, randomY);
            //};

            // ������Ʈ Ǯ��, ��Ȱ��ȭ�� ���� ã�Ƽ� active �� ���� �� ��ġ����
            GameObject dreamPiece;
            for (int i = 0; i < DreamPiecePool.transform.childCount; i++)
            {
                dreamPiece = DreamPiecePool.transform.GetChild(i).gameObject;
                if (dreamPiece.activeSelf != true)
                {
                    dreamPiece.SetActive(true);
                    dreamPiece.transform.localPosition = vec2;
                    break;
                }
            }

            // ����Լ��� �ݺ�����
            StartCoroutine(DreamPiece_Randominstantiate());
        }
    }

}
