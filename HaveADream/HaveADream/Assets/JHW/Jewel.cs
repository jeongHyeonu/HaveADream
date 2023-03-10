using UnityEngine;

public class Jewel : MonoBehaviour
{
    public enum Jewel_type
    {
        blue,
        red,
    }

    [SerializeField] Jewel_type jewelType;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            if (this.jewelType == Jewel_type.blue) { SkillManager.Instance.GetBlueJewel(); }
            if (this.jewelType == Jewel_type.red) { SkillManager.Instance.GetRedJewel(); }
        }
        Destroy(this);
    }
}
