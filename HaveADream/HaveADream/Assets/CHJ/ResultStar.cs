using UnityEngine;

public class ResultStar : Singleton<ResultStar>
{

    [SerializeField] GameObject Star1;
    [SerializeField] GameObject Star2;
    [SerializeField] GameObject Star3;

    [SerializeField] int DPScore; // 보스까지 거리

    //스테이지 데이터 받아와야 함

    private void OnEnable()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        DPScore = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];
    }
    private void OnDisable()
    {
        //DPScore = 0;
    }
    void Update()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        DPScore = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];
        //gameObject.SetActive(true);
        CheckStar();
        //별 0개인 경우
    }
    void CheckStar()
    {
        if (DataManager.Instance.ResultStars == 0)
        {
            //Debug.Log("별1");
            Star1.gameObject.SetActive(false);
            Star2.gameObject.SetActive(false);
            Star3.gameObject.SetActive(false);

        }
        else if (DataManager.Instance.ResultStars == 1)
        {
            Star1.SetActive(true);
            Star2.SetActive(false);
            Star3.SetActive(false);
        }
        else if (DataManager.Instance.DreamPieceScore < DPScore && DataManager.Instance.ResultStars == 2)
        {
            Star1.gameObject.SetActive(true);
            Star2.gameObject.SetActive(true);
            Star3.gameObject.SetActive(false);
        }
        else if (DataManager.Instance.DreamPieceScore >= DPScore && DataManager.Instance.ResultStars == 2)
        {
            Star1.gameObject.SetActive(true);
            Star2.gameObject.SetActive(true);
            Star3.gameObject.SetActive(true);
        }
    }

}
