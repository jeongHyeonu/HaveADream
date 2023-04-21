using UnityEngine;

public class Jewel : MonoBehaviour
{
    public GameObject targetj;

    public enum Jewel_type
    {
        blue,
        red,
    }

    [SerializeField] Jewel_type jewelType;

    //�ڼ�
    private float magnetStrength = 5f;
    private float distanceStretch = 10f;
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

    void FixedUpdate()
    {
        if (magetinZone)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetj.transform.position, 0.35f);
            /*Debug.Log("ȣ��");
            Vector2 directionToMagnet = magnetTrans.position - trans.position;
            float distance = Vector2.Distance(magnetTrans.position, trans.position);
            float magnetDistanceStr = (distanceStretch / distance) * magnetStrength;
            rb.AddForce(magnetDistanceStr * (directionToMagnet * magnetDirection), ForceMode2D.Force);*/
        }
    }

    private void OnEnable()
    {
        // Ȱ��ȭ �� 15�� �� ��Ȱ��ȭ
        Invoke("SetActiveFalseJewel", 15f);
        targetj = null;
        magetinZone = false;
    }

    private void OnDisable()
    {
        // ���� �Ծ disable�Ǹ� ���� 15�ʵ� ��Ȱ��ȭ�Ǵ°� ����
        CancelInvoke("SetActiveFalseJewel");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("MagneticField") == 0)
        {
            magnetTrans = collision.transform;
            magetinZone = true;

        }

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

            // ȿ���� �� ����Ʈ
            EffectManager.Instance.PlayVFX(EffectManager.VFX_list.SPARK1, this.gameObject);
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.JEWEL_GET);
        }



    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MagneticField") && looseMagnet)
        {
            magetinZone = false;
        }
    }
}
