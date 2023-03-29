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


    private void SetActiveFalseJewel()
    {
        this.gameObject.SetActive(false);
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

            // 효과음 및 이펙트
            EffectManager.Instance.PlayVFX(EffectManager.VFX_list.SPARK1, this.gameObject);
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.JEWEL_GET);
        }
        if (collision.gameObject.tag.CompareTo("MagneticField") == 0)
        {
            //매끄럽게 움직이게 하기
            Vector3 pos = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed * 1.5f);
            transform.position = pos;

        }
    }
}
