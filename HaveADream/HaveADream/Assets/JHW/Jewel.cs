using UnityEngine;

public class Jewel : MonoBehaviour
{
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

        }
    }
}
