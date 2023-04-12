using System.Collections;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneManager sm = null;
    [SerializeField] GameObject textBossWarning; //보스 등장 텍스트
    [SerializeField] GameObject boss;   //보스 경고
    [SerializeField] float bossDistance = 0f; // 보스까지 거리


    private void OnEnable()
    {
        textBossWarning.SetActive(false);
        boss.SetActive(false);
        GetStageData();

    }

    private void GetStageData()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        bossDistance = (int)StageDataManager.Instance.GetStageInfo(key)["stage"];

        gameObject.transform.position = new Vector2(gameObject.transform.position.x + (bossDistance * 1.15f), 0f);
    }


    void Start()
    {
        // 싱글톤
        sm = SceneManager.Instance;

    }



    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            StartCoroutine("SpawnBoss");

        }

    }

    private IEnumerator SpawnBoss()
    {
        textBossWarning.SetActive(true);    //보스 등장 문구
        yield return new WaitForSeconds(1.0f);  //1초 대기
        textBossWarning.SetActive(false);   //문구 사라짐
        boss.SetActive(true);               //보스 등장
        boss.GetComponent<Boss>().ChangeState(BossState.MoveToAppearPoint);

    }



}
