using System.Collections;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneManager sm = null;
    [SerializeField] GameObject textBossWarning; //���� ���� �ؽ�Ʈ
    [SerializeField] GameObject boss;   //���� ���



    private void Awake()
    {
        textBossWarning.SetActive(false);
        boss.SetActive(false);
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
