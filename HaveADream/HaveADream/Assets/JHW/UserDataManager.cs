using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using PlayFab;
using PlayFab.ClientModels;

[System.Serializable]
public class UserData
{
    public string userID = "";
    public string userName = "";
    public int maxHeart = 30;
    public int heart = 20;
    public int gold = 100;

    public List<Episode1Data> epi1Data;
    public List<Episode2Data> epi2Data;
    public List<Episode3Data> epi3Data;
}

[System.Serializable]
public class Episode1Data
{
    public string mapName;
    public bool isClearStage; // 클리어했는지 여부
    public int star; // 유저가 달성한 별 개수
}

[System.Serializable]
public class Episode2Data
{
    public string mapName;
    public bool isClearStage; // 클리어했는지 여부
    public int star; // 유저가 달성한 별 개수
}

[System.Serializable]
public class Episode3Data
{
    public string mapName;
    public bool isClearStage; // 클리어했는지 여부
    public int star; // 유저가 달성한 별 개수
}

public class UserDataManager : Singleton<UserDataManager>
{
    [SerializeField] private string userID;
    [SerializeField] private string userName;
    [SerializeField] private int maxHeart;
    [SerializeField] private int heart;
    [SerializeField] private int gold;

    [SerializeField] private List<Episode1Data> epi1Data;
    [SerializeField] private List<Episode2Data> epi2Data;
    [SerializeField] private List<Episode3Data> epi3Data;

    public string path;

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
    public void SetUserData_maxHeart(int maxHeart)
    {
        this.maxHeart = maxHeart;
    }

    public void SetUserData_heart(int heart)
    {
        this.heart += heart;
    }
    public void SetUserData_userID(String ID)
    {
        this.userID = ID;
    }

    void Awake()
    {
        // 데이터 불러오거나 저정하는 경로 설정 (단, 불러오는건 서버에서 받아올것임. 로컬에선 불러오지말것)
        path = Path.Combine(Application.dataPath, "Datas/userData.json");


        // 플레이어 데이터 불러오고 로컬파일에 저장
        LoadData();

    }

    void OnApplicationQuit()
    {
        // 데이터 저장
        path = Path.Combine(Application.dataPath, "Datas/userData.json");
        SaveData(null);
    }

    public void LoadData()
    {
        UserData userData = new UserData();


        // 만약 플레이어 데이터 존재하지 않으면 새로 만든다
        if (!File.Exists(path))
        {
            userID = "";
            PlayFabLogin.Instance.SetPlayerData();
            //SaveData(null);
        }
        else // 플레이어 데이터 존재시 ID만 읽고 서버에서 데이터 불러온다
        {
            string loadJson = File.ReadAllText(path);
            userData = JsonUtility.FromJson<UserData>(loadJson);

            UserDataManager.Instance.userID = userData.userID;
            PlayFabLogin.Instance.SetPlayerData();
        }
    }

    public void LoadDataOnComplete(UserData Data) // 위에서 (서버에 있는 유저정보) json 읽어오고 다시 userdatamanager에 저장
    {
        UserDataManager.Instance.userName = Data.userName;
        UserDataManager.Instance.maxHeart = Data.maxHeart;
        UserDataManager.Instance.heart = Data.heart;
        UserDataManager.Instance.gold = Data.gold;

        UserDataManager.Instance.epi1Data = Data.epi1Data;
        UserDataManager.Instance.epi2Data = Data.epi2Data;
        UserDataManager.Instance.epi3Data = Data.epi3Data;

        // 데이터 불러오고 상단 Heart UI 변경
        UIGroupManager.Instance.ChangeHeartUI();
    }

    public void SaveData(UserData userData) // userData를 null 로 줄 경우 UserData의 데이터를 새로 json으로 저장, null 아니면 지정된 값으로 데이터 저장
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

            userData.epi1Data = UserDataManager.Instance.epi1Data;
            userData.epi2Data = UserDataManager.Instance.epi2Data;
            userData.epi3Data = UserDataManager.Instance.epi3Data;
        }



        // 로컬파일 데이터저장
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(path, json);



        // playfab 서버로 데이터저장
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

