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


    [SerializeField] float distance = 0f; // 유저 이동거리
    [SerializeField] float bossDistance = 0f; // 보스까지 거리
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
        /*startPos = dpUI; // 시작 위치는 UI 요소의 현재 위치로 설정
        targetPos = dpUI; // 목표 위치는 시작 위치로 설정*/
        distance = 0;
        //dpUI.anchoredPosition = Vector2.zero;

        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        GoalScore = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];

        // 보스까지 거리 계산
        string key2 = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
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

            if (bossDistance <= distance)// 보스까지 도달시
            {

                distance = bossDistance;


            }*/


        }
    }
}
