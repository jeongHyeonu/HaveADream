using System.Collections;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneManager sm = null;
    [SerializeField] GameObject textBossWarning; //���� ���� �ؽ�Ʈ
    [SerializeField] GameObject boss;   //���� ���
    [SerializeField] float bossDistance = 0f; // �������� �Ÿ�


    private void OnEnable()
    {
        textBossWarning.SetActive(false);
        boss.SetActive(false);
        GetStageData();

    }

    private void GetStageData()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
        bossDistance = (int)StageDataManager.Instance.GetStageInfo(key)["stage"];

        gameObject.transform.position = new Vector2(gameObject.transform.position.x + (bossDistance * 1.15f), 0f);
    }


    void Start()
    {
        // �̱���
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
        textBossWarning.SetActive(true);    //���� ���� ����
        yield return new WaitForSeconds(1.0f);  //1�� ���
        textBossWarning.SetActive(false);   //���� �����
        boss.SetActive(true);               //���� ����
        boss.GetComponent<Boss>().ChangeState(BossState.MoveToAppearPoint);

    }



}
