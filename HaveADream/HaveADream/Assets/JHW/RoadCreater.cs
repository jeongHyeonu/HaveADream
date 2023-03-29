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
        
        // ���������� ��������� ���� ����
        element.transform.SetParent(_dest.transform);
        element.transform.localPosition = _dest.transform.parent.localPosition;

        Vector2 vec2;
        vec2.x = _dest.transform.localPosition.x - _start.transform.localPosition.x;
        vec2.y = _dest.transform.localPosition.y - _start.transform.localPosition.y;
        float rot = Mathf.Atan2(vec2.y, vec2.x) * Mathf.Rad2Deg;
        element.transform.eulerAngles = new Vector3(0, 0, rot + 90.0f);

        float length = Vector3.Distance(_start.transform.localPosition, _dest.transform.localPosition) + 25;
        // ���� �ڿ� ���ϴ� ���(25)�� ���ڱ� ������Ʈ 1�� ����

        int footCntPerLength = (int)(length) / 25; // �̶� 25�� ���ڱ� ������Ʈ 1�� ����
        element.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); // ���� ���� ó�� ���ڱ��� ������ ����
        for (int i = 1; i < footCntPerLength; i++)
        {
            element.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        }

        element.transform.SetParent(roadList.transform);
        element.transform.localScale = Vector2.one;
    }

    public IEnumerator RoadColorChange(int cnt) // �Ű����� ����ŭ(Ŭ������ ���Ǽҵ忡 ����) ���� �� ����
    {
        while (true)
        {
            if (road_isComplete == false) yield return new WaitForSeconds(0.1f); // �� ���� �ȉ����� �ɶ����� ���
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
