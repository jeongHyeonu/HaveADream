using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataManager : Singleton<StageDataManager>
{
    List<Dictionary<string, object>> data;

    [SerializeField] public List<Sprite> BindBossImg;
    [SerializeField] public List<Sprite> ClearBossImg;

    public Dictionary<string, object> stageInfoDict = new Dictionary<string, object>();

    // Start is called before the first frame update
    void Start()
    {
        data = CSVReader.Read("stageInfoData");

        for(int i = 0; i < data.Count; i++)
        {
            stageInfoDict.Add((int)data[i]["episode"] + "-" + (int)data[i]["stage"], data[i]);
        }
    }

    public Dictionary<string, object> GetStageInfo(string _key)
    {
        return (Dictionary<string, object>)stageInfoDict[_key];
    }
}


