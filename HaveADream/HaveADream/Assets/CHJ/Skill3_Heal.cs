using UnityEngine;

public class Skill3_Heal : MonoBehaviour
{
    static Skill3_Heal instance;

    public static Skill3_Heal Instance
    {
        get
        {
            return instance;
        }
    }


    void Awake()
    {
        instance = this;
    }

}
