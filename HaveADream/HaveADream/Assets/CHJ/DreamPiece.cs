using UnityEngine;



public class DreamPiece : MonoBehaviour
{

    public GameObject target;

    private float magnetStrength = 5f;
    private float distanceStretch = 10f;
    private int magnetDirection = 1;
    private bool looseMagnet = true;

    //�ڼ�
    private Transform trans;
    private Rigidbody2D rb;
    private Transform magnetTrans;
    private bool magetinZone;

    void Awake()
    {


    }
    private void OnEnable()
    {
        target = null;
        magetinZone = false;
    }
    void FixedUpdate()
    {
        //rb = target.GetComponent<Rigidbody2D>();
        if (magetinZone)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 0.1f);
            /*Debug.Log("ȣ��");
            Vector2 directionToMagnet = magnetTrans.position - trans.position;
            float distance = Vector2.Distance(magnetTrans.position, trans.position);
            float magnetDistanceStr = (distanceStretch / distance) * magnetStrength;
            rb.AddForce(magnetDistanceStr * (directionToMagnet * magnetDirection), ForceMode2D.Force);*/
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("MagneticField") == 0)
        {
            Debug.Log("ȣ��");
            //�ڼ� �ѱ�
            magnetTrans = collision.transform;
            magetinZone = true;

        }

        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            //������ ī��Ʈ
            DataManager.Instance.DreamPieceScore += 1;

            //ȭ�鿡�� ����
            gameObject.SetActive(false);

            // ���� �� ����Ʈ
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.GOLD_GET);
            EffectManager.Instance.PlayVFX(EffectManager.VFX_list.FLASH1, this.gameObject);

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
