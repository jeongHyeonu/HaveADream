using UnityEngine;



public class DreamPiece : MonoBehaviour
{

    [SerializeField] GameObject target;

    private float magnetStrength = 5f;
    private float distanceStretch = 7.5f;
    private int magnetDirection = 1;
    private bool looseMagnet = true;

    //자석
    private Transform trans;
    private Rigidbody2D rb;
    private Transform magnetTrans;
    private bool magetinZone;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            //꿈조각 카운트
            DataManager.Instance.DreamPieceScore += 1;

            //화면에서 끄기
            gameObject.SetActive(false);

            // 사운드 및 이펙트
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.GOLD_GET);
            EffectManager.Instance.PlayVFX(EffectManager.VFX_list.FLASH1, this.gameObject);

        }
        if (collision.gameObject.tag.CompareTo("MagneticField") == 0)
        {
            //자석 켜기
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
