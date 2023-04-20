using UnityEngine;
using UnityEngine.UI;

public class DreamPieceUI : MonoBehaviour
{

    [SerializeField] Image DpBar;
    [SerializeField] float GoalScore;
    [SerializeField] int DpValue;
    public float DpScore;
    [SerializeField] Vector2 target;

    [SerializeField] float distance = 0f; // ���� �̵��Ÿ�
    [SerializeField] float bossDistance = 0f; // �������� �Ÿ�

    void Awake()
    {
        DpBar.GetComponent<Image>();
        //DataManager.Instance.DreamPieceScore = DpScore;
        DpScore = 0.0f;
    }
    private void OnEnable()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
        GoalScore = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];

        // �������� �Ÿ� ���
        string key2 = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
        bossDistance = (int)StageDataManager.Instance.GetStageInfo(key2)["boss_distance"];

        target = new Vector2(-85.0f, 0f);
    }
    private void OnDisable()
    {
        DpBar.fillAmount = 0.0f;
        DpScore = 0.0f;
    }

    void FixedUpdate()
    {
        if (DpBar != null)
        {

            DpBar.fillAmount = DataManager.Instance.DreamPieceScore / GoalScore;
            distance += MapMove.Instance.mapSpeed * Time.deltaTime * 0.0075f;
            Vector2 temp = new Vector2(0.5f, 0f);
            gameObject.transform.Translate(temp);

        }
    }
}
