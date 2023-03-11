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
        // Ȱ��ȭ �� 15�� �� ��Ȱ��ȭ
        Invoke("SetActiveFalseJewel", 15f);
    }

    private void OnDisable()
    {
        // ���� �Ծ disable�Ǹ� ���� 15�ʵ� ��Ȱ��ȭ�Ǵ°� ����
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
