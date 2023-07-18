using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DreamPieceUI : MonoBehaviour
{
    [SerializeField] float GoalScore;
    [SerializeField] int DpValue;
    public float DpScore;

    [SerializeField] float moveSpeed;

    [SerializeField] TextMeshProUGUI scoreText; // UI Text ������Ʈ�� ������ ����

    private bool reachedGoal = false; // ��ǥ ���� ���θ� ��Ÿ���� ����



    private void OnEnable()
    {
        DpScore = 0;
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
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
                DpScore = GoalScore; // DpScore�� GoalScore�� �����Ͽ� �� �̻� �������� �ʵ��� ��
                reachedGoal = true;
            }
        }
    }
}