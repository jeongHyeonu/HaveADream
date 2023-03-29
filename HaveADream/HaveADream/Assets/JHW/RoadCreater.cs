using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadCreater : MonoBehaviour
{
    [SerializeField] public List<GameObject> StageList;
    [SerializeField] GameObject roadObject;
    [SerializeField] GameObject roadList;

    bool road_isComplete = false;

    int stageListCnt;

    private void Start()
    {
        stageListCnt = StageList.Count;
        for(int i = 0; i < stageListCnt-1; i++)
        {
            CreateRoad(StageList[i], StageList[i+1]);
        }
        road_isComplete = true;
    }

    private void CreateRoad(GameObject _start, GameObject _dest)
    {
        GameObject element = Instantiate(roadObject);
        
        // 도착지에서 출발지까지 도로 생성
        element.transform.SetParent(_dest.transform);
        element.transform.localPosition = _dest.transform.parent.localPosition;

        Vector2 vec2;
        vec2.x = _dest.transform.localPosition.x - _start.transform.localPosition.x;
        vec2.y = _dest.transform.localPosition.y - _start.transform.localPosition.y;
        float rot = Mathf.Atan2(vec2.y, vec2.x) * Mathf.Rad2Deg;
        element.transform.eulerAngles = new Vector3(0, 0, rot + 90.0f);

        float length = Vector3.Distance(_start.transform.localPosition, _dest.transform.localPosition) + 25;
        // 길이 뒤에 더하는 상수(25)는 발자국 오브젝트 1개 길이

        int footCntPerLength = (int)(length) / 25; // 이때 25는 발자국 오브젝트 1개 길이
        element.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); // 도로 생성 처음 발자국은 꺼놓고 시작
        for (int i = 1; i < footCntPerLength; i++)
        {
            element.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        }

        element.transform.SetParent(roadList.transform);
        element.transform.localScale = Vector2.one;
    }

    public IEnumerator RoadColorChange(int cnt) // 매개변수 값만큼(클리어한 에피소드에 따라) 도로 색 변경
    {
        while (true)
        {
            if (road_isComplete == false) yield return new WaitForSeconds(0.1f); // 길 생성 안됬으면 될때까지 대기
            else break;
        }
        for(int i = 0; i <cnt; i++)
        {
            if (i == roadList.transform.childCount) break;
            for (int roadBundleIdx = 1; roadBundleIdx < 20; roadBundleIdx++)
            {
                if (roadList.transform.GetChild(i).GetChild(0).GetChild(roadBundleIdx).gameObject.activeSelf == false) break;
                for (int j = 0; j < 3; j++)
                {
                    roadList.transform.GetChild(i).GetChild(0).GetChild(roadBundleIdx).GetChild(j).GetComponent<Image>().color = new Color(1f, 1f, 0.1f, 1f);
                }
            }
        }
    }
}
