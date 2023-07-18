//using UnityEditor.SceneManagement;
using JHW;
using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] GameObject resultWindow;
    [SerializeField] GameObject getAnimalWindow1;
    [SerializeField] GameObject getAnimalWindow2;

    private SceneManager sm = null;

    [SerializeField] int currentEpisodeNumber;
    [SerializeField] int currentStageNumber;



    void Start()
    {
        // �̱���
        sm = SceneManager.Instance;

    }
    //Ȩ���� ����
    public void ReturnHomeBtn_OnClick()
    {
        sm.Scene_Change_Home();

        // ����
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);
    }
    //�ٽ��ϱ�
    public void ReplayBtn_OnClick()
    {
        resultWindow.SetActive(false);

        sm.Scene_Change_GamePlay();

        // ����ʱ�ȭ
        BackgroundManager.Instance.GenerateBackground(JHW.StageSelectManager.Instance.curEpiNum);

        int currentStageMusic = int.Parse(UserDataManager.Instance.GetUserData_userCurrentStage().Split("-")[1]);


        // ����
        // ���� ���
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

    //������
    public void ReturnStageBtn_OnClick()
    {
        sm.Scene_Change_StageSelect();


        // ����
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.StageSelect_BGM);
    }


    // ���â ������ ���������� ����
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
        // �������� ����
        // ������ ������ �������� ����
        string key = UserDataManager.Instance.GetUserData_userCurrentStage();

        currentStageNumber = (int)StageDataManager.Instance.GetStageInfo(key)["episode"];
        currentEpisodeNumber = (int)StageDataManager.Instance.GetStageInfo(key)["stage"];


        if (currentEpisodeNumber == 1)
        {
            if (currentStageNumber == 13)
            {
                if (DataManager.Instance.ResultStars == 3)
                {
                    getAnimalWindow1.SetActive(true);
                }

            }

        }
        if (currentEpisodeNumber == 2)
        {
            if (currentStageNumber == 18)
            {
                if (DataManager.Instance.ResultStars == 3)
                {
                    getAnimalWindow2.SetActive(true);
                }

            }

        }


    }

    // ��� ����
    public void ResultSave()
    {
        string epiData = UserDataManager.Instance.GetUserData_userCurrentStage();

        int epiNum = int.Parse(epiData.Split("-")[0]);
        int stageNum = int.Parse(epiData.Split("-")[1]);

        int userGainStar = DataManager.Instance.ResultStars; // ������ ȹ���� �� ����
        switch (epiNum)
        {

            case 1:
                Episode1Data data1 = UserDataManager.Instance.GetUserData_userEpi1Data()[stageNum - 1]; // ���� ������
                if (data1.star < userGainStar) // ������ ����
                {
                    UserDataManager.Instance.setUserData_epi1Data(epiData, true, userGainStar);
                }
                break;
            case 2:
                Episode2Data data2 = UserDataManager.Instance.GetUserData_userEpi2Data()[stageNum - 1]; // ���� ������
                if (data2.star < userGainStar) // ������ ����
                {
                    UserDataManager.Instance.setUserData_epi2Data(epiData, true, userGainStar);
                }
                break;
            case 3:
                Episode3Data data3 = UserDataManager.Instance.GetUserData_userEpi3Data()[stageNum - 1]; // ���� ������
                if (data3.star < userGainStar) // ������ ����
                {
                    UserDataManager.Instance.setUserData_epi3Data(epiData, true, userGainStar);
                }
                break;
        }
        UserDataManager.Instance.SaveData(null); // ������ ����
    }
}
