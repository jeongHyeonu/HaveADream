using System.Collections;
using UnityEngine;

public enum BossState { MoveToAppearPoint = 0, }
public class Boss : MonoBehaviour
{
    private SceneManager sm = null;

    [SerializeField]
    private float bossAppearPoint = 0f;
    private BossState bossState = BossState.MoveToAppearPoint;
    private Movement2D movement2D;

    //[SerializeField] Transform returnTransform;

    [SerializeField] GameObject hudDamageText;
    [SerializeField] Transform hudPos;

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] int projectile;


    // ���� óġ �� ���ο� ȿ�� �ɸ���
    private void slowBoss_ON()
    {
        Time.timeScale = 0.5f;
    }
    private void slowBoss_OFF()
    {
        Time.timeScale = 1f;
    }



    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
    }
    void Start()
    {
        // �̱���
        sm = SceneManager.Instance;
    }
    private void OnDisable()
    {
        Vector2 temp = new Vector2(13.0f, 0);
        gameObject.SetActive(false);
        gameObject.transform.Translate(temp);
    }

    public void ChangeState(BossState newState)
    {
        StopCoroutine(bossState.ToString());
        bossState = newState;
        StartCoroutine(bossState.ToString());
    }

    public IEnumerator MoveToAppearPoint()
    {
        //�ڷ�ƾ ���� �� 1ȸ ȣ��
        movement2D.MoveTo(Vector3.left);
        while (true)
        {
            if (transform.position.x <= bossAppearPoint)
            {
                movement2D.MoveTo(Vector3.zero);
            }
            yield return null;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("BossProjectile") == 0)
        {
            DataManager.Instance.bossAttackScore++;
            TakeDamage(-50);
        }
    }
    public void TakeDamage(int damage)
    {
        //������ �ؽ�Ʈ ���
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;

        //�Ѿ˼�=�ǰݼ� �� ȣ��
        if (DataManager.Instance.bossAttackScore == projectile)
        {
            //���� ���ο� ���� �߰��ϱ�

            gameObject.SetActive(false);
            sm.Scene_Change_Result();
            //sm.Scene_Change_Result();
        }
    }
}
