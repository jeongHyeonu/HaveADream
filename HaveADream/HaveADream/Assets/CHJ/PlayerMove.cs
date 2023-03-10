using UnityEngine;

public class PlayerMove : Singleton<PlayerMove>
{
    //스피드 조절
    [SerializeField] float speed = 1;
    bool isTouching = false;
    bool isSkillBtn = false; // 유저가 스킬 버튼 눌렀는지 검사여부
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
        // 화면을 클릭하고 유저가 스킬 버튼을 누르지 않았다면 비행
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
    //무적 기능, 플리커
    void OnDamaged()
    {
        //무적 기능
        gameObject.layer = 21;
        //색상 변경
        sr.color = new Color(1, 1, 1, 0.4f);

        Invoke("OffDamaged", 1);

    }

    //무적 기능 풀기
    void OffDamaged()
    {
        gameObject.layer = 20;
        sr.color = new Color(1, 1, 1, 1);
    }
}
