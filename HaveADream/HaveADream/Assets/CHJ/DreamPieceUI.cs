using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DreamPieceUI : MonoBehaviour
{
    [SerializeField] float GoalScore;
    [SerializeField] int DpValue;
    public float DpScore;

    [SerializeField] float moveSpeed;

    [SerializeField] TextMeshProUGUI scoreText; // UI Text 컴포넌트를 참조할 변수

    private bool reachedGoal = false; // 목표 도달 여부를 나타내는 변수



    private void OnEnable()
    {
        DpScore = 0;
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        GoalScore = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];
    }

    private void OnDisable()
    {
        /* DpScore = 0;
         GoalScore = 0;
         scoreText.text = ""; */
        reachedGoal = false;
    }

    void FixedUpdate()
    {
        if (!reachedGoal)
        {
            DpScore = DataManager.Instance.DreamPieceScore;
            scoreText.text = DpScore.ToString("F0") + " / " + GoalScore.ToString("F0");

            if (DpScore >= GoalScore)
            {
                DpScore = GoalScore; // DpScore를 GoalScore로 설정하여 더 이상 증가하지 않도록 함
                reachedGoal = true;
            }
        }
    }
}