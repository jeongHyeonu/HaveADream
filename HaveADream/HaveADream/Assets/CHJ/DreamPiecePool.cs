using System.Collections;
using UnityEngine;


/*[System.Serializable]
//오브젝트 풀링 클래스
public class ObjectInfo
{
    public GameObject DpPrefab;
    public int count;
    public Transform tfPoolParent;
}*/

public class DreamPiecePool : MonoBehaviour
{
    [SerializeField] GameObject DPPool;

    private void OnEnable()
    {
        StartCoroutine(DP_Randominstantiate());
    }

    IEnumerator DP_Randominstantiate()
    {
        float randomSpawnDelay = Random.Range(2f, 4f);
        yield return new WaitForSeconds(randomSpawnDelay);

        // 랜덤 생성 위치 지정
        // Block (Cbstacle) 충돌 일어나지 않게 추가 구현
        float height = 2 * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        float playerDistance = GameObject.Find("Player").transform.position.x - this.transform.position.x;
        float randomY = Random.Range(-height / 4, height / 4);
        Vector2 vec2 = new Vector2(width + playerDistance, randomY);

        // 오브젝트 풀링
        GameObject DP;
        for (int i = 0; i < DPPool.transform.childCount; i++)
        {
            DP = DPPool.transform.GetChild(i).gameObject;
            if (DP.activeSelf != true)
            {
                DP.SetActive(true);
                DP.transform.localPosition = vec2;
                break;
            }
        }

        // 재귀함수로 반복실행
        StartCoroutine(DP_Randominstantiate());
    }

}
