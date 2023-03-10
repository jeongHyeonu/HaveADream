using UnityEngine;

public class PlayerMove : Singleton<PlayerMove>
{
    //���ǵ� ����
    [SerializeField] float speed = 1;
    bool isTouching = false;
    bool isSkillBtn = false; // ������ ��ų ��ư �������� �˻翩��
    Rigidbody2D rb;
    SpriteRenderer sr;


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
    }

    void Update()
    {
        // ȭ���� Ŭ���ϰ� ������ ��ų ��ư�� ������ �ʾҴٸ� ����
        if (Input.GetMouseButton(0) && !isSkillBtn) isTouching = true; 
        else isTouching = false;
    }
    private void FixedUpdate()
    {
        if (isTouching == false)
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 20f *speed);
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
        }
    }
    //���� ���, �ø�Ŀ
    void OnDamaged()
    {
        //���� ���
        gameObject.layer = 21;
        //���� ����
        sr.color = new Color(1, 1, 1, 0.4f);

        Invoke("OffDamaged", 1);

    }

    //���� ��� Ǯ��
    void OffDamaged()
    {
        gameObject.layer = 20;
        sr.color = new Color(1, 1, 1, 1);
    }
}
