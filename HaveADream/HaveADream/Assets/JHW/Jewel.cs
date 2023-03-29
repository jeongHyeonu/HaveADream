using UnityEngine;

public class Jewel : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float speed = 5.0f;
    public enum Jewel_type
    {
        blue,
        red,
    }

    [SerializeField] Jewel_type jewelType;

    //자석
    private float magnetStrength = 5f;
    private float distanceStretch = 7.5f;
    private int magnetDirection = 1;
    private bool looseMagnet = true;

    private Transform trans;
    private Rigidbody2D rb;
    private Transform magnetTrans;
    private bool magetinZone;


    private void SetActiveFalseJewel()
    {
        this.gameObject.SetActive(false);
    }
    void Awake()
    {
        trans = this.transform;
        rb = trans.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (magetinZone)
        {
            Vector2 directionToMagnet = magnetTrans.position - trans.position;
            float distance = Vector2.Distance(magnetTrans.position, trans.position);
            float magnetDistanceStr = (distanceStretch / distance) * magnetStrength;
            rb.AddForce(magnetDistanceStr * (directionToMagnet * magnetDirection), ForceMode2D.Force);
        }
    }

    private void OnEnable()
    {
        // 활성화 시 15초 뒤 비활성화
        Invoke("SetActiveFalseJewel", 15f);
    }

    private void OnDisable()
    {
        // 보석 먹어서 disable되면 위에 15초뒤 비활성화되는거 해제
        CancelInvoke("SetActiveFalseJewel");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            if (this.jewelType == Jewel_type.blue)
            {
                SkillManager.Instance.GetBlueJewel();
                this.gameObject.SetActive(false);
            }
            if (this.jewelType == Jewel_type.red)
            {
                SkillManager.Instance.GetRedJewel();
                this.gameObject.SetActive(false);
            }

        }
        if (collision.gameObject.tag.CompareTo("MagneticField") == 0)
        {
            magnetTrans = collision.transform;
            magetinZone = true;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MagneticField" && looseMagnet)
        {
            magetinZone = false;
        }
    }
}
