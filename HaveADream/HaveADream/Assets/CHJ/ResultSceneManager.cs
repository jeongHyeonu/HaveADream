using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    private SceneManager sm = null;
    void Start()
    {
        // 싱글톤
        sm = SceneManager.Instance;

    }
    public void ReturnHomeBtn_OnClick()
    {
        sm.Scene_Change_Home();

        // 사운드
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);
    }

    // 결과창 켜질때 유저데이터 저장
    private void OnEnable()
    {
        ResultSave();
    }

    // 결과 저장
    public void ResultSave()
    {
        string epiData = UserDataManager.Instance.GetUserData_userCurrentStage();

        int epiNum = int.Parse(epiData.Split("-")[0]);
        int stageNum = int.Parse(epiData.Split("-")[1]);

        int userGainStar = DataManager.Instance.ResultStars; // 유저가 획득한 별 개수
        userGainStar = 3;// 테스트 및 데이터 연동 확인을 위해 획득한 별 개수 3으로 설정
        switch (epiNum)
        {

            case 1:
                Episode1Data data1 = UserDataManager.Instance.GetUserData_userEpi1Data()[stageNum - 1]; // 유저 데이터
                if (data1.star < userGainStar) // 데이터 저장
                {
                    UserDataManager.Instance.setUserData_epi1Data(epiData, true, userGainStar);
                }
                break;
            case 2:
                Episode2Data data2 = UserDataManager.Instance.GetUserData_userEpi2Data()[stageNum - 1]; // 유저 데이터
                if (data2.star < userGainStar) // 데이터 저장
                {
                    UserDataManager.Instance.setUserData_epi2Data(epiData, true, userGainStar);
                }
                break;
            case 3:
                Episode3Data data3 = UserDataManager.Instance.GetUserData_userEpi3Data()[stageNum - 1]; // 유저 데이터
                if (data3.star < userGainStar) // 데이터 저장
                {
                    UserDataManager.Instance.setUserData_epi3Data(epiData, true, userGainStar);
                }
                break;
        }
        UserDataManager.Instance.SaveData(null); // 데이터 저장
    }
}
