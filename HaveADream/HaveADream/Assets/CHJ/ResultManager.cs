using System.Collections;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneManager sm = null;
    [SerializeField] GameObject textBossWarning; //���� ���� �ؽ�Ʈ
    [SerializeField] GameObject boss;   //���� ���
    [SerializeField] float bossDistance = 0f; // �������� �Ÿ�

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
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
        bossDistance = (int)StageDataManager.Instance.GetStageInfo(key)["boss_distance"];

        gameObject.transform.position = new Vector2(bossDistance, 0f);
    }


    void Start()
    {
        // �̱���
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
        textBossWarning.SetActive(true);    //���� ���� ����
        yield return new WaitForSeconds(0.5f);  //0.5�� ���
        textBossWarning.SetActive(false);   //���� �����
        boss.SetActive(true);               //���� ����
        boss.GetComponent<Boss>().ChangeState(BossState.MoveToAppearPoint);

    }



}
