using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

partial class PlayFabLogin : Singleton<PlayFabLogin>
{
    [SerializeField] private string userID = "";
    public bool isLogined = false;

    private void Start()
    {
        isLogined = false;
    }
}

#region �޼���
partial class PlayFabLogin {
    public void SetPlayerData()
    {
        userID = UserDataManager.Instance.GetUserData_userID();

        // ���� ���� ���̵� ������ ���� ����, ���̵� ������ �α���
        if (userID == "") CreateGuestId(); // �Խ�Ʈ ���̵� ����
        else LoginUserID(); // �α���
    }

    void SetUserData()
    {
        // ���� ���Ͽ� �ִ� userData.JSON �� playpref ������ ����
        string path = Path.Combine(Application.dataPath, "Datas/userData.json");
        
        string loadJson = File.ReadAllText(path);
        UserData userData = JsonUtility.FromJson<UserData>(loadJson);

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

    public UserData GetUserData(string myPlayFabId)
    {
        UserData content = null;

        // playpref ������ �ִ� ������ �ҷ����� (json ������ ���� ��������� �ð����̰ɷ���)
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result => {
            //Debug.Log("Got user data:");
            //if (result.Data == null || !result.Data.ContainsKey("Ancestor")) Debug.Log("No Ancestor");
            //else Debug.Log("Ancestor: " + result.Data["Ancestor"].Value);
            foreach (var eachData in result.Data)
            {
                string key = eachData.Key;

                if (eachData.Key.Contains("userInfo"))
                {
                    content = JsonUtility.FromJson<UserData>(eachData.Value.Value);
                    UserDataManager.Instance.LoadDataOnComplete(content);
                    //UserDataManager.Instance.SaveData(content);
                }
            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });

        return content;
    }

    private string GetRandomPassword(int _totLen) //������ 16�ڸ� id ����
    {
        string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var chars = Enumerable.Range(0, _totLen)
            .Select(x => input[UnityEngine.Random.Range(0, input.Length)]);
        return new string(chars.ToArray());
    }


    private void CreateGuestId() //����� ���̵� ���� ��� ���� �Խ�Ʈ ���̵� ����
    {
        userID = GetRandomPassword(16);
        UserDataManager.Instance.SetUserData_userID(userID);

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = userID,
            CreateAccount = true
        }, result =>
        {
            OnLoginSuccess(result);
        }, error =>
        {
            Debug.LogError("Login Fail - Guest");
        });
    }

    private void LoginUserID() // ����� ���̵��� �α���
    {


        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = userID,
            CreateAccount = true
        }, result =>
        {
            OnLoginSuccess(result);
        }, error =>
        {
            Debug.LogError("Login Fail - Guest");
        });
    }

    private void OnLoginSuccess(LoginResult result)
    {
        GetUserData(result.PlayFabId);
        //SetUserData(); // playfab���� ������ ����
    }
    private void DisplayPlayfabError(PlayFabError error) => Debug.LogError("error : " + error.GenerateErrorReport());

    public void SetUserData(Dictionary<string, string> data)
    {
        var request = new UpdateUserDataRequest() { Data = data, Permission = UserDataPermission.Public };
        try
        {
            PlayFabClientAPI.UpdateUserData(request, (result) =>
            {
                Debug.Log("Update Player Data!");

            }, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

}
#endregion


#region LeaderBoard
partial class PlayFabLogin
{
    public void SendLeaderboad(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlatformScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult res)
    {
        Debug.Log("�������� ���� ����!");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("���� : " +error.ErrorMessage);
    }

    public void GetLeaderboad()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult res)
    {
        foreach(var item in res.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
}
#endregion


#region CloudScript
partial class PlayFabLogin
{
    Dictionary<string, string> datas;

    public void ReadTitleNews()
    {
        PlayFabClientAPI.GetTitleNews(new GetTitleNewsRequest(), result => {
            BroadcastManager.Instance.OpenBroadcastUI();
            if (BroadcastManager.Instance.BroadcastUI.transform.GetChild(0).GetChild(0).GetChild(0).childCount > 0) return;
            for(int i = 0; i < result.News.Count; i++)
            {
                BroadcastManager.Instance.CreateBroadcastItem(result.News[i]);
            }
        }, error => Debug.LogError(error.GenerateErrorReport()));

        //var req = new ExecuteCloudScriptRequest
        //{
        //    FunctionName = "broadcast"
        //};
        //PlayFabClientAPI.ExecuteCloudScript(req, OnExecuteSuccess, OnError);
    }

    void OnExecuteSuccess(ExecuteCloudScriptResult res)
    {
        Debug.Log(res.FunctionResult);
        Dictionary<string,string> s = (Dictionary<string, string>)res.FunctionResult;
        BroadcastManager.Instance.broadCast = s;
    }
}
#endregion

# region virtual currency
partial class PlayFabLogin
{


    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    public void AddHeart(int Amount = 1) // ��Ʈ ���� ����
    {
        var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "HT", Amount = Amount };
        PlayFabClientAPI.AddUserVirtualCurrency(request, (res) => { }, OnError);
    }

    public void SubtractHeart(int Amount = 1) // ��Ʈ ���� ����
    {
        var request = new SubtractUserVirtualCurrencyRequest() { VirtualCurrency = "HT", Amount = Amount };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, (res)=> { },OnError);
    }

    void OnGetUserInventorySuccess(GetUserInventoryResult res)
    {
        int heart = res.VirtualCurrency["HT"];
        UserDataManager.Instance.SetUserData_heart(heart);
        HeartManager.Instance.secondsLeftToRefresh = res.VirtualCurrencyRechargeTimes["HT"].SecondsToRecharge;
        if (heart != 0) { HeartManager.Instance.isHeartZero = false; HeartManager.Instance.inUIheartText.text = heart.ToString() + "/" + UserDataManager.Instance.GetUserData_maxHeart(); HeartManager.Instance.topUIheartText.text = heart.ToString() + "/" + UserDataManager.Instance.GetUserData_maxHeart(); }
        else { HeartManager.Instance.isHeartZero = true; }
    }
}
#endregion
