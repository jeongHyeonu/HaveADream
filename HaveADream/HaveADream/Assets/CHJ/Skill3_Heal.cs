using UnityEngine;

public class Skill3_Heal : MonoBehaviour
{
    static Skill3_Heal instance;
    // Start is called before the first frame update
    public static Skill3_Heal Instance
    {
        get
        {
            return instance;
        }
    }

    // Update is called once per frame
    void Awake()
    {
        instance = this;
    }

}
