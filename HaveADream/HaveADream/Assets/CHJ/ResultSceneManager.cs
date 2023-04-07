using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    private SceneManager sm = null;
    void Start()
    {
        // �̱���
        sm = SceneManager.Instance;

    }
    public void ReturnHomeBtn_OnClick()
    {
        sm.Scene_Change_Home();
    }

    // ���â ������ ���������� ����
    private void OnEnable()
    {
        ResultSave();
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
                    UserDataManager.Instance.setUserData_epi1Data(epiData, true, DataManager.Instance.ResultStars);
                }
                break;
            case 2:
                Episode2Data data2 = UserDataManager.Instance.GetUserData_userEpi2Data()[stageNum - 1]; // ���� ������
                if (data2.star < userGainStar) // ������ ����
                {
                    UserDataManager.Instance.setUserData_epi2Data(epiData, true, DataManager.Instance.ResultStars);
                }
                break;
            case 3:
                Episode3Data data3 = UserDataManager.Instance.GetUserData_userEpi3Data()[stageNum - 1]; // ���� ������
                if (data3.star < userGainStar) // ������ ����
                {
                    UserDataManager.Instance.setUserData_epi3Data(epiData, true, DataManager.Instance.ResultStars);
                }
                break;
        }
        UserDataManager.Instance.SaveData(null); // ������ ����
    }
}
