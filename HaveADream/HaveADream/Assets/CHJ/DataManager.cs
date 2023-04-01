using UnityEngine;

public class DataManager : MonoBehaviour
{
    static DataManager instance;


    public static DataManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
    }

    //실제 꿈조각 모은 양 저장
    public float DreamPieceScore = 0;
    public float HealthCurrent = 0.0f;
    public int ResultStars = 0;
    public int bossAttackScore = 0;



}
