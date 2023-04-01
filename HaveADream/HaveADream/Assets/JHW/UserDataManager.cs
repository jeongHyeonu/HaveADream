using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

[System.Serializable]
public class UserData
{
    public string userID = "";
    public string userName = "";
    public int maxHeart = 30;
    public int heart = 20;
    public int gold = 100;
    public string userCurrentStage = "1-1";

    public List<Episode1Data> epi1Data;
    public List<Episode2Data> epi2Data;
    public List<Episode3Data> epi3Data;
}

[System.Serializable]
public class Episode1Data
{
    public string mapName;
    public bool isClearStage; // Ŭ�����ߴ��� ����
    public int star; // ������ �޼��� �� ����
}

[System.Serializable]
public class Episode2Data
{
    public string mapName;
    public bool isClearStage; // Ŭ�����ߴ��� ����
    public int star; // ������ �޼��� �� ����
}

[System.Serializable]
public class Episode3Data
{
    public string mapName;
    public bool isClearStage; // Ŭ�����ߴ��� ����
    public int star; // ������ �޼��� �� ����
}

partial class UserDataManager : Singleton<UserDataManager>
{
    [SerializeField] private string userID;
    [SerializeField] private string userName;
    [SerializeField] private int maxHeart;
    [SerializeField] private int heart;
    [SerializeField] private int gold;
    [SerializeField] private string userCurrentStage;

    [SerializeField] private List<Episode1Data> epi1Data;
    [SerializeField] private List<Episode2Data> epi2Data;
    [SerializeField] private List<Episode3Data> epi3Data;

    public string path;

    #region Getter/Setter
    public string GetUserData_userID()
    {
        return this.userID;
    }

    public int GetUserData_maxHeart()
    {
        return this.maxHeart;
    }

    public int GetUserData_heart()
    {
        return this.heart;
    }
    public string GetUserData_userCurrentStage()
    {
        return this.userCurrentStage;
    }

    public List<Episode1Data> GetUserData_userEpi1Data()
    {
        return this.epi1Data;
    }
    public List<Episode2Data> GetUserData_userEpi2Data()
    {
        return this.epi2Data;
    }
    public List<Episode3Data> GetUserData_userEpi3Data()
    {
        return this.epi3Data;
    }


    public void SetUserData_maxHeart(int maxHeart)
    {
        this.maxHeart = maxHeart;
    }

    public void SetUserData_heart(int heart)
    {
        this.heart = heart;
    }
    public void SetUserData_userID(String ID)
    {
        this.userID = ID;
    }
    public void setUserData_userCurrentStage(string stageInfo)
    {
        this.userCurrentStage = stageInfo;
    }
    public void setUserData_epi1Data(string _stageName,bool _clearFlag,int _userGainStar)
    {
        Episode1Data data = epi1Data.Find(x=>x.mapName == _stageName);
        data.isClearStage = _clearFlag;
        data.star = _userGainStar;
    }
    public void setUserData_epi2Data(string stageInfo)
    {
        this.userCurrentStage = stageInfo;
    }
    public void setUserData_epi3Data(string stageInfo)
    {
        this.userCurrentStage = stageInfo;
    }

    #endregion

    void Awake()
    {
        // ������ �ҷ����ų� �����ϴ� ��� ���� (��, �ҷ����°� �������� �޾ƿð���. ���ÿ��� �ҷ���������)
        path = Path.Combine(Application.dataPath, "Datas/userData.json");


        // �÷��̾� ������ �ҷ����� �������Ͽ� ����
        LoadData();

        // �ε�â Ȱ��ȭ
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    void OnApplicationQuit()
    {
        // ������ ����
        path = Path.Combine(Application.dataPath, "Datas/userData.json");
        SaveData(null);
    }

    public void LoadData()
    {
        UserData userData = new UserData();


        // ���� �÷��̾� ������ �������� ������ ���� �����
        if (!File.Exists(path))
        {
            userID = "";
            PlayFabLogin.Instance.SetPlayerData();
            //SaveData(null);
        }
        else // �÷��̾� ������ ����� ID�� �а� �������� ������ �ҷ��´�
        {
            string loadJson = File.ReadAllText(path);
            userData = JsonUtility.FromJson<UserData>(loadJson);

            UserDataManager.Instance.userID = userData.userID;
            PlayFabLogin.Instance.SetPlayerData();
        }
    }

    public void LoadDataOnComplete(UserData Data) // ������ (������ �ִ� ��������) json �о���� �ٽ� userdatamanager�� ����
    {
        // ������ �ִ� ������ �� �ҷ����� �����ϴ� �Լ�.
        // UserData ���ӿ�����Ʈ �ν��Ͻ��� ���� �����͵��� �ֽ��ϴ�.
        if (Data != null)
        {
            UserDataManager.Instance.userName = Data.userName;
            UserDataManager.Instance.maxHeart = Data.maxHeart;
            UserDataManager.Instance.heart = Data.heart;
            UserDataManager.Instance.gold = Data.gold;
            UserDataManager.Instance.userCurrentStage = Data.userCurrentStage;

            UserDataManager.Instance.epi1Data = Data.epi1Data;
            UserDataManager.Instance.epi2Data = Data.epi2Data;
            UserDataManager.Instance.epi3Data = Data.epi3Data;
        }

        // ������ �ҷ����� ��� Heart UI ����
        UIGroupManager.Instance.ChangeHeartUI();

        // Ȩ ȭ�� �ر��� ���� ����
        HomeManager.Instance.Home_OpenAnials();

        // �ε�â ��Ȱ��ȭ
        this.transform.GetChild(0).gameObject.SetActive(false);

        // �÷��̾� �α��ο���
        PlayFabLogin.Instance.isLogined = true;
    }

    public void SaveData(UserData userData) // userData�� null �� �� ��� UserData�� �����͸� ���� json���� ����, null �ƴϸ� ������ ������ ������ ����
    {
        //if (!File.Exists(path)) maxHeart = heart = 30;
        if (userData == null)
        {
            userData = new UserData();
            userData.userID = UserDataManager.Instance.userID;
            userData.userName = UserDataManager.Instance.userName;
            userData.maxHeart = UserDataManager.Instance.maxHeart;
            userData.heart = UserDataManager.Instance.heart;
            userData.gold = UserDataManager.Instance.gold;
            userData.userCurrentStage = UserDataManager.Instance.userCurrentStage;

            userData.epi1Data = UserDataManager.Instance.epi1Data;
            userData.epi2Data = UserDataManager.Instance.epi2Data;
            userData.epi3Data = UserDataManager.Instance.epi3Data;
        }



        // �������� ����������
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(path, json);



        // playfab ������ ����������
        Dictionary<string, string> datas = new Dictionary<string, string>();
        datas.Add("userInfo", JsonUtility.ToJson(userData));
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = datas,
        },
        result => { }, //Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    
}


partial class UserDataManager
{
    //[SerializeField] private TextMeshProUGUI heartText;
    //[SerializeField] private TextMeshProUGUI heartRechargeTimeText;

    float secondsLeftToRefresh = 1200f;
    // ��Ʈ �� ����


    public void HeartChange()
    {
        PlayFabLogin.Instance.GetVirtualCurrencies();
    }
    
}