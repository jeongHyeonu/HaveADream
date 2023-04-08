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


    // 보스 처치 시 슬로우 효과 걸리게
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
        // 싱글톤
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
        //코루틴 실행 시 1회 호출
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
        //데미지 텍스트 출력
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;

        //총알수=피격수 시 호출
        if (DataManager.Instance.bossAttackScore == projectile)
        {
            //보스 슬로우 연출 추가하기

            gameObject.SetActive(false);
            sm.Scene_Change_Result();
            //sm.Scene_Change_Result();
        }
    }
}
