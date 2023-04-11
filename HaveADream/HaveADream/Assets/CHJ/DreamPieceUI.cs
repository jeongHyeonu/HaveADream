using UnityEngine;
using UnityEngine.UI;

public class DreamPieceUI : MonoBehaviour
{

    [SerializeField] Image DpBar;
    [SerializeField] float GoalScore;
    [SerializeField] int DpValue;
    public float DpScore;


    void Awake()
    {
        DpBar.GetComponent<Image>();
        //DataManager.Instance.DreamPieceScore = DpScore;
        DpScore = 0.0f;
    }
    private void OnEnable()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        GoalScore = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];
    }
    private void OnDisable()
    {
        DpBar.fillAmount = 0.0f;
        DpScore = 0.0f;
    }

    void Update()
    {
        if (DpBar != null)
        {

            DpBar.fillAmount = DataManager.Instance.DreamPieceScore / GoalScore;
        }
    }
}
