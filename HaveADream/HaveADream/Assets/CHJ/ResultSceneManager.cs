//using UnityEditor.SceneManagement;
using JHW;
using System.Collections;
using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] GameObject resultWindow;
    [SerializeField] GameObject getAnimalWindow1;
    [SerializeField] GameObject getAnimalWindow2;

    private SceneManager sm = null;


    void Start()
    {
        // 싱글톤
        sm = SceneManager.Instance;

    }
    //홈으로 가기
    public void ReturnHomeBtn_OnClick()
    {
        sm.Scene_Change_Home();

        // 사운드
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);
    }
    //다시하기
    public void ReplayBtn_OnClick()
    {
        resultWindow.SetActive(false);

        sm.Scene_Change_GamePlay();

        // 배경초기화
        BackgroundManager.Instance.GenerateBackground(JHW.StageSelectManager.Instance.curEpiNum);

        int currentStageMusic = int.Parse(UserDataManager.Instance.GetUserData_userCurrentStage().Split("-")[1]);


        // 사운드
        // 음악 재생
        switch (currentStageMusic)
        {
            case 1:
            case 2:
            case 5:
            case 6:
            case 9:
            case 10:
            case 13:
                SoundManager.Instance.PlayBGM(SoundManager.BGM_list.GamePlayBGM_1);
                break;
            case 3:
            case 4:
            case 7:
            case 8:
            case 11:
            case 12:
                SoundManager.Instance.PlayBGM(SoundManager.BGM_list.GamePlayBGM_2);
                break;
        }
    }

    //나가기
    public void ReturnStageBtn_OnClick()
    {
        sm.Scene_Change_StageSelect();


        // 사운드
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.StageSelect_BGM);
    }


    // 결과창 켜질때 유저데이터 저장
    private void OnEnable()
    {
        Time.timeScale = 0;
        resultWindow.SetActive(true);
        GetAnimalPanel();
    }
    private void OnDisable()
    {
        ResultSave();
        Time.timeScale = 1;
    }

    private void GetAnimalPanel()
    {
        string epiData = UserDataManager.Instance.GetUserData_userCurrentStage();

        int epiNum = int.Parse(epiData.Split("-")[0]);
        int stageNum = int.Parse(epiData.Split("-")[1]);


        if (epiNum == 1)
        {
            if (stageNum == 13)
            {
                if (DataManager.Instance.ResultStars == 3)
                {
                    resultWindow.SetActive(false);
                    getAnimalWindow1.SetActive(true);
                    StartCoroutine(StartFadeIn(getAnimalWindow1));
                }

            }

        }
        if (epiNum == 2)
        {
            if (stageNum == 18)
            {
                if (DataManager.Instance.ResultStars == 3)
                {
                    resultWindow.SetActive(false);
                    getAnimalWindow2.SetActive(true);
                    StartCoroutine(StartFadeIn(getAnimalWindow2));
                }

            }

        }


    }


    private IEnumerator FadeIn(GameObject targetWindow)
    {
        CanvasGroup cg = targetWindow.GetComponent<CanvasGroup>();
        if (cg == null)
            yield break;

        float startAlpha = 0.0f; // Completely transparent at the start
        float endAlpha = 1.0f;
        float duration = 2.0f; // Increase duration to make fade in slower
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration; // goes from 0 to 1
                                                       // Update alpha
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);
            yield return null;
        }

        cg.alpha = endAlpha; // ensure it's set to the end alpha
    }

    private IEnumerator StartFadeIn(GameObject targetWindow)
    {
        //targetWindow.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FadeIn(targetWindow));
    }



    // 결과 저장
    public void ResultSave()
    {
        string epiData = UserDataManager.Instance.GetUserData_userCurrentStage();

        int epiNum = int.Parse(epiData.Split("-")[0]);
        int stageNum = int.Parse(epiData.Split("-")[1]);

        int userGainStar = DataManager.Instance.ResultStars; // 유저가 획득한 별 개수
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
