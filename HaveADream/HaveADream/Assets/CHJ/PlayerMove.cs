using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : Singleton<PlayerMove>
{
    //���ǵ� ����
    [SerializeField] float speed = 1;
    [SerializeField] GameObject HpBar;
    [SerializeField] Image HpBarFilled;

    bool isTouching = false;
    bool isSkillBtn = false; // ������ ��ų ��ư �������� �˻翩��

    public bool isShield = false;
    public bool isInvisible = false;


    Rigidbody2D rb;
    SpriteRenderer sr;

    private SceneManager sm = null;


    [SerializeField] float smoothTime; // 보간 시간

    private Vector3 targetPosition; // 목표 위치
    private Vector3 velocity = Vector3.zero; // 보간용 변수

    public GameObject MagneticField;
    [SerializeField] float magnetForce = 10f;

    [SerializeField] GameObject Player;

    [SerializeField] GameObject DP;
    [SerializeField] GameObject BJW;
    //[SerializeField] GameObject RJW;




    //스킬1 : 스피드 상승
    public void ChangeSpeed(float speed = 1)
    {
        this.speed = speed;
    }

    //스킬2 : 안 보이게 되는 무적 부스터
    public void ChangeInvisible()
    {
        gameObject.layer = 21;
        //Debug.Log("21로 전환되었나?");
        sr.color = new Color(0.7f, 0.4f, 0.4f, 1f);
        MapMove.Instance.mapSpeed = 10f;
        isInvisible = true;

    }


    //스킬3 : 체력 회복
    public void ChangeHealth()
    {
        DataManager.Instance.HealthCurrent += 1.0f;
        HpBarFilled.fillAmount += 0.1f;
    }

    //스킬4 : 자석
    public void Magnet()
    {
        //MagneticField.layer = 21;
        MagneticField.SetActive(true);
    }
    //스킬5 : 방어막 (구현 완료, 건드리지 말 것)
    public void Shield()
    {
        gameObject.layer = 21;
        //실드 작동 확인
        sr.color = new Color(0.3f, 0.4f, 0.7f, 1f);
        isShield = true;
    }

    public void setIsSkillBtn(bool flag)
    {
        isSkillBtn = flag;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //씬 시작 시 Hp바 초기화
        HpBarFilled.fillAmount = 1.0f;
    }

    void Start()
    {
        // �̱��� �޾ƿ���
        sm = SceneManager.Instance;
        targetPosition = transform.position + Vector3.right * 2f; // 목표 위치 설정

    }

    void Update()
    {
        // 주석이 다 이상해졌는데 
        if (Input.GetMouseButton(0) && !isSkillBtn) isTouching = true;
        else isTouching = false;
    }
    private void FixedUpdate()
    {
        if (isTouching == false)
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 19f * speed);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 19f * speed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            OnDamaged();

            if (HpBarFilled.fillAmount <= 0.0f)
            {
                sm.Scene_Change_Result();
            }
            if (isInvisible)
            {
                HpBarFilled.fillAmount += 0.25f;
                DataManager.Instance.HealthCurrent += 2.5f;

            }
            if (isShield)
            {
                HpBarFilled.fillAmount += 0.25f;
                DataManager.Instance.HealthCurrent += 2.5f;
                isShield = false;
            }


        }
    }
    void OnDamaged()
    {
        //layer 바꿔서 충돌하지 않도록 임시로 바꿈
        gameObject.layer = 21;
        //체력바 감소
        HpBarFilled.fillAmount -= 0.25f;
        //데이터상 감소
        DataManager.Instance.HealthCurrent -= 2.5f;


        sr.color = new Color(1, 1, 1, 0.4f);
        Invoke("OffDamaged", 2f);
    }

    void OffDamaged()
    {
        gameObject.layer = 20;
        sr.color = new Color(1, 1, 1, 1);
    }
}
