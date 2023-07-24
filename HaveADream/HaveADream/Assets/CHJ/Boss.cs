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
    private Boss boss;

    private SpriteRenderer renderer;

    //[SerializeField] Transform returnTransform;

    [SerializeField] GameObject hudDamageText;
    [SerializeField] Transform hudPos;

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] int projectile;
    [SerializeField] GameObject ResultWindow;

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
        boss = GetComponent<Boss>();
        renderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {

        gameObject.SetActive(true);
        renderer.enabled = true;
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        projectile = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];

        // 보스 이미지
        int userCurEpi = int.Parse(UserDataManager.Instance.GetUserData_userCurrentStage().Split("-")[0]); // 유저 현재 에피소드
        this.GetComponent<SpriteRenderer>().sprite = StageDataManager.Instance.BindBossImg[userCurEpi - 1];
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


            //총알수=피격수 시 호출
            if (DataManager.Instance.bossAttackScore == projectile)
            {

                boss.OnDie();
                renderer.enabled = false; //렌더러 비활성화
                Time.timeScale = 0.5f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;

                Invoke("SetResultWindow", 1.25f);
                DistanceManager.Instance.DistanceUI_OFF();
                //SetResultWindow();
                //sm.Scene_Change_Result();
            }

        }
    }
    public void TakeDamage(int damage)
    {
        // 데미지 텍스트 출력
        GameObject hudText = Instantiate(hudDamageText);
        Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
        hudText.transform.position = hudPos.position + randomOffset;


    }
    public void OnDie()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    public void SetResultWindow()
    {
        ResultWindow.SetActive(true);
    }
}
