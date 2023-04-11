using System.Collections;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneManager sm = null;
    [SerializeField] GameObject textBossWarning; //보스 등장 텍스트
    [SerializeField] GameObject boss;   //보스 경고



    private void Awake()
    {
        textBossWarning.SetActive(false);
        boss.SetActive(false);
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
