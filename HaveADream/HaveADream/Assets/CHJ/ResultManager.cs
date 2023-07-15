using System.Collections;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneManager sm = null;
    [SerializeField] GameObject textBossWarning; //보스 등장 텍스트
    [SerializeField] GameObject boss;   //보스 경고
    [SerializeField] float bossDistance = 0f; // 보스까지 거리

    private bool hasStartedSpawnBoss = false;

    private void OnEnable()
    {

        textBossWarning.SetActive(false);
        boss.SetActive(false);
        GetStageData();
        hasStartedSpawnBoss = false;
    }

    private void DisEnable()
    {
        hasStartedSpawnBoss = false;
    }
    private void GetStageData()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        bossDistance = (int)StageDataManager.Instance.GetStageInfo(key)["boss_distance"];

        gameObject.transform.position = new Vector2(bossDistance, 0f);
    }


    void Start()
    {
        // 싱글톤
        sm = SceneManager.Instance;

    }

    void FixedUpdate()
    {
        if (!hasStartedSpawnBoss && DistanceManager.Instance.isBossArrived == true && 
            DistanceManager.Instance.isGamePlaying == true)
        {
            StartCoroutine("SpawnBoss");
            hasStartedSpawnBoss = true;
        }
    }

    // Update is called once per frame
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            StartCoroutine("SpawnBoss");

        }

    }*/


    private IEnumerator SpawnBoss()
    {
        textBossWarning.SetActive(true);    //보스 등장 문구
        yield return new WaitForSeconds(0.5f);  //0.5초 대기
        textBossWarning.SetActive(false);   //문구 사라짐
        boss.SetActive(true);               //보스 등장
        boss.GetComponent<Boss>().ChangeState(BossState.MoveToAppearPoint);

    }



}
