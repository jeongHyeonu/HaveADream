using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : Singleton<PlayerMove>
{
    //���ǵ� ���� 주석 왜이래
    [SerializeField] float speed = 1;
    [SerializeField] GameObject HpBar;
    [SerializeField] Image HpBarFilled;

    bool isTouching = false;
    bool isSkillBtn = false; // ������ ��ų ��ư �������� �˻翩��
    Rigidbody2D rb;
    SpriteRenderer sr;

    private SceneManager sm = null;

    public void ChangeSpeed(float speed = 1)
    {
        this.speed = speed;
    }

    public void setIsSkillBtn(bool flag)
    {
        isSkillBtn = flag;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //씬 시작 시 Hp바 초기화 되도록
        HpBarFilled.fillAmount = 1.0f;
    }

    void Start()
    {
        // �̱��� �޾ƿ���
        sm = SceneManager.Instance;


    }

    void Update()
    {
        // 주석이 다 이상해졌는데 ȭ���� Ŭ���ϰ� ������ ��ų ��ư�� ������ �ʾҴٸ� ����
        if (Input.GetMouseButton(0) && !isSkillBtn) isTouching = true;
        else isTouching = false;
    }
    private void FixedUpdate()
    {
        if (isTouching == false)
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 20f * speed);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 20f * speed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            OnDamaged();
            //체력바 감소
            HpBarFilled.fillAmount -= 0.1f;
            if (HpBarFilled.fillAmount == 0.0f)
            {
                sm.Scene_Change_Result();
            }
        }
    }
    void OnDamaged()
    {
        //layer 바꿔서 충돌하지 않도록 임시로 바꿈
        gameObject.layer = 21;
        sr.color = new Color(1, 1, 1, 0.4f);
        Invoke("OffDamaged", 1);
    }

    void OffDamaged()
    {
        gameObject.layer = 20;
        sr.color = new Color(1, 1, 1, 1);
    }
}
