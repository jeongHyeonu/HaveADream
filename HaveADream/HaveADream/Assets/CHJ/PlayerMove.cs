using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //스피드 조절
    [SerializeField] float speed;
    bool isTouching = false;
    Rigidbody2D rb;
    SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) isTouching = true;
        else isTouching = false;
    }
    private void FixedUpdate()
    {
        if (isTouching == false)
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 20f);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 20f);
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
