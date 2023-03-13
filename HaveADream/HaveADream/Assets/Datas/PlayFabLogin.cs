using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

partial class PlayFabLogin : Singleton<PlayFabLogin>
{
    [SerializeField] private string userID = "";
}

#region 메서드
partial class PlayFabLogin {
    public void SetPlayerData()
    {
        userID = UserDataManager.Instance.GetUserData_userID();

        // 만약 유저 아이디 없으면 새로 생성, 아이디 있으면 로그인
        if (userID == "") CreateGuestId(); // 게스트 아이디 생성
        else LoginUserID(); // 로그인
    }

    void SetUserData()
    {
        // 로컬 파일에 있는 userData.JSON 을 playpref 서버에 저장
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

        // playpref 서버에 있는 데이터 불러오고 (json 저장은 안함 파일입출력 시간많이걸려서)
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

    private string GetRandomPassword(int _totLen) //랜덤한 16자리 id 생성
    {
        string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var chars = Enumerable.Range(0, _totLen)
            .Select(x => input[UnityEngine.Random.Range(0, input.Length)]);
        return new string(chars.ToArray());
    }


    private void CreateGuestId() //저장된 아이디가 없을 경우 새로 게스트 아이디 생성
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

    private void LoginUserID() // 저장된 아이디대로 로그인
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
        //SetUserData(); // playfab으로 데이터 저장
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


#region JSON 저장/로드
partial class PlayFabLogin
{
    public string entityType; // entityType representing the logged in player
    private readonly Dictionary<string, string> _entityFileJson = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _tempUpdates = new Dictionary<string, string>();
    public string ActiveUploadFileName;
    public string NewFileName;
    // GlobalFileLock provides is a simplistic way to avoid file collisions, specifically designed for this example.
    public int GlobalFileLock = 0;

    void OnSharedFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        GlobalFileLock -= 1;
    }

    //void OnGUI() { }


    void Login()
    {
        var request = new PlayFab.ClientModels.LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLogin, OnSharedFailure);
    }

    void OnLogin(PlayFab.ClientModels.LoginResult result)
    {
        userID = result.EntityToken.Entity.Id;
        entityType = result.EntityToken.Entity.Type;
    }

    void LoadAllFiles()
    {
        if (GlobalFileLock != 0)
            throw new Exception("This example overly restricts file operations for safety. Careful consideration must be made when doing multiple file operations in parallel to avoid conflict.");

        GlobalFileLock += 1; // Start GetFiles
        var request = new PlayFab.DataModels.GetFilesRequest { Entity = new PlayFab.DataModels.EntityKey { Id = userID, Type = entityType } };
        PlayFabDataAPI.GetFiles(request, OnGetFileMeta, OnSharedFailure);
    }

    void OnGetFileMeta(PlayFab.DataModels.GetFilesResponse result)
    {
        Debug.Log("Loading " + result.Metadata.Count + " files");

        _entityFileJson.Clear();
        foreach (var eachFilePair in result.Metadata)
        {
            _entityFileJson.Add(eachFilePair.Key, null);
            GetActualFile(eachFilePair.Value);
        }
        GlobalFileLock -= 1; // Finish GetFiles
    }

    void GetActualFile(PlayFab.DataModels.GetFileMetadata fileData)
    {
        GlobalFileLock += 1; // Start Each SimpleGetCall
        PlayFabHttp.SimpleGetCall(fileData.DownloadUrl,
            result => { _entityFileJson[fileData.FileName] = Encoding.UTF8.GetString(result); GlobalFileLock -= 1; }, // Finish Each SimpleGetCall
            error => { Debug.Log(error); }
        );
    }

    void UploadFile(string fileName)
    {
        if (GlobalFileLock != 0)
            throw new Exception("This example overly restricts file operations for safety. Careful consideration must be made when doing multiple file operations in parallel to avoid conflict.");

        ActiveUploadFileName = fileName;

        GlobalFileLock += 1; // Start InitiateFileUploads
        var request = new PlayFab.DataModels.InitiateFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = userID, Type = entityType },
            FileNames = new List<string> { ActiveUploadFileName },
        };
        PlayFabDataAPI.InitiateFileUploads(request, OnInitFileUpload, OnInitFailed);
    }

    void OnInitFailed(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.EntityFileOperationPending)
        {
            // This is an error you should handle when calling InitiateFileUploads, but your resolution path may vary
            GlobalFileLock += 1; // Start AbortFileUploads
            var request = new PlayFab.DataModels.AbortFileUploadsRequest
            {
                Entity = new PlayFab.DataModels.EntityKey { Id = userID, Type = entityType },
                FileNames = new List<string> { ActiveUploadFileName },
            };
            PlayFabDataAPI.AbortFileUploads(request, (result) => { GlobalFileLock -= 1; UploadFile(ActiveUploadFileName); }, OnSharedFailure); GlobalFileLock -= 1; // Finish AbortFileUploads
            GlobalFileLock -= 1; // Failed InitiateFileUploads
        }
        else
            OnSharedFailure(error);
    }

    void OnInitFileUpload(PlayFab.DataModels.InitiateFileUploadsResponse response)
    {
        string payloadStr;
        if (!_entityFileJson.TryGetValue(ActiveUploadFileName, out payloadStr))
            payloadStr = "{}";
        var payload = Encoding.UTF8.GetBytes(payloadStr);

        GlobalFileLock += 1; // Start SimplePutCall
        PlayFabHttp.SimplePutCall(response.UploadDetails[0].UploadUrl,
            payload,
            FinalizeUpload,
            error => { Debug.Log(error); }
        );
        GlobalFileLock -= 1; // Finish InitiateFileUploads
    }

    void FinalizeUpload(byte[] data)
    {
        GlobalFileLock += 1; // Start FinalizeFileUploads
        var request = new PlayFab.DataModels.FinalizeFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = userID, Type = entityType },
            FileNames = new List<string> { ActiveUploadFileName },
        };
        PlayFabDataAPI.FinalizeFileUploads(request, OnUploadSuccess, OnSharedFailure);
        GlobalFileLock -= 1; // Finish SimplePutCall
    }
    void OnUploadSuccess(PlayFab.DataModels.FinalizeFileUploadsResponse result)
    {
        Debug.Log("File upload success: " + ActiveUploadFileName);
        GlobalFileLock -= 1; // Finish FinalizeFileUploads
    }


    public void GetUserData()
    {
        var request = new GetUserDataRequest() { PlayFabId = userID };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            foreach (var eachData in result.Data)
            {
                string key = eachData.Key;

                Debug.Log(eachData + " " + key);
            }

        }, DisplayPlayfabError);
    }
}
#endregion
