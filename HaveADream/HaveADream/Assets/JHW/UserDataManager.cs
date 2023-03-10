using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class UserData
{
    public int maxHeart = 30;
    public int heart = 20;
    public int gold = 100;

    public List<Episode1Data> epi1Data;
    public List<Episode2Data> epi2Data;
    public List<Episode3Data> epi3Data;
}

[Serializable]
[SerializeField]
public class Episode1Data
{
    bool isClearStage; // 클리어했는지 여부
    int stageNumber; // 스테이지 넘버
    int star; // 유저가 달성한 별 개수
}

[Serializable]
[SerializeField]
public class Episode2Data
{
    bool isClear; // 클리어했는지 여부
    int stageNumber; // 스테이지 넘버
    int star; // 별 개수
}

[Serializable]
[SerializeField]
public class Episode3Data
{
    bool isClear; // 클리어했는지 여부
    int stageNumber; // 스테이지 넘버
    int star; // 별 개수
}

public class UserDataManager : Singleton<UserDataManager>
{
    [SerializeField] private int maxHeart;
    [SerializeField] private int heart;
    [SerializeField] private int gold;

    [SerializeField] private List<Episode1Data> epi1Data;
    [SerializeField] private List<Episode2Data> epi2Data;
    [SerializeField] private List<Episode3Data> epi3Data;

    string path;

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





    private void Start()
    {
        // 데이터 불러오기
        path = Path.Combine(Application.dataPath, "Datas/userData.json");
        LoadData();

        // 데이터 불러오고 상단 Heart UI 변경
        UIGroupManager.Instance.ChangeHeartUI();
    }

    public void LoadData()
    {
        UserData userData = new UserData();

        if (!File.Exists(path))
        {
            SaveData();
        } 
        else
        {
            string loadJson = File.ReadAllText(path);
            userData = JsonUtility.FromJson<UserData>(loadJson);

            UserDataManager.Instance.maxHeart = userData.maxHeart;
            UserDataManager.Instance.heart = userData.heart;
            UserDataManager.Instance.gold = userData.gold;
        }
    }

    public void SaveData()
    {
        UserData userData = new UserData();

        userData.maxHeart = UserDataManager.Instance.maxHeart;
        userData.heart = UserDataManager.Instance.heart;
        userData.gold = UserDataManager.Instance.gold;

        userData.epi1Data = UserDataManager.Instance.epi1Data;
        userData.epi2Data = UserDataManager.Instance.epi2Data;
        userData.epi3Data = UserDataManager.Instance.epi3Data;

        string json = JsonUtility.ToJson(userData, true);

        File.WriteAllText(path, json);
    }
}

