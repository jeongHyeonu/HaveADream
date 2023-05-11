using UnityEngine;
using UnityEngine.UI;

public class DreamPieceUI : MonoBehaviour
{

    [SerializeField] Image DpBar;
    [SerializeField] float GoalScore;
    [SerializeField] int DpValue;
    public float DpScore;
    [SerializeField] RectTransform dpUI;
    [SerializeField] RectTransform startPos;
    [SerializeField] RectTransform targetPos;
    //public RectTransform rectTransform;


    [SerializeField] float distance = 0f; // ���� �̵��Ÿ�
    [SerializeField] float bossDistance = 0f; // �������� �Ÿ�
    [SerializeField] float moveSpeed;


    void Awake()
    {
        DpBar.GetComponent<Image>();
        //DataManager.Instance.DreamPieceScore = DpScore;
        DpScore = 0.0f;
    }
    private void OnEnable()
    {
        //dpUI= this.GetComponent<RectTransform>();
        /*startPos = dpUI; // ���� ��ġ�� UI ����� ���� ��ġ�� ����
        targetPos = dpUI; // ��ǥ ��ġ�� ���� ��ġ�� ����*/
        distance = 0;
        //dpUI.anchoredPosition = Vector2.zero;

        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
        GoalScore = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];

        // �������� �Ÿ� ���
        string key2 = UserDataManager.Instance.GetUserData_userCurrentStage(); // ������ ������ �������� key
        bossDistance = (int)StageDataManager.Instance.GetStageInfo(key2)["boss_distance"];

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

            /*distance += MapMove.Instance.mapSpeed * Time.deltaTime * 4.09f;
            moveSpeed = distance / bossDistance;

            dpUI.anchoredPosition = Vector2.Lerp(startPos.anchoredPosition, targetPos.anchoredPosition, moveSpeed);


            //Debug.Log(dpUI.anchoredPosition);

            if (bossDistance <= distance)// �������� ���޽�
            {

                distance = bossDistance;


            }*/


        }
    }
}
