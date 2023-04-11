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

    [SerializeField] GameObject ResultWindow;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
    }
    private void OnEnable()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        projectile = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];
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
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;


        if (DataManager.Instance.bossAttackScore == projectile)
        {
            //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            //Destroy(explosionPrefab);
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            gameObject.SetActive(false);

            Invoke("SetResultWindow", 2f);
            //sm.Scene_Change_Result();
        }
    }
    public void SetResultWindow()
    {
        ResultWindow.SetActive(true);
    }
}
