using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : Singleton<BackgroundManager>
{

    [SerializeField] GameObject Background1;
    [SerializeField] GameObject Background2;
    [SerializeField] GameObject Background3;

    [SerializeField] GameObject MapBackgroundPool;

    public void GenerateBackground(int epi_num)
    {
        // 맵 생성
        switch (epi_num)
        {
            case 1:
                Background1.gameObject.SetActive(true);
                break;
            case 2:
                Background2.gameObject.SetActive(true);
                break;
            case 3:
                Background3.gameObject.SetActive(true);
                break;
        }
    }

    private void OnDisable()
    {
        EraseBackground(); // 배경제거
    }


    public void EraseBackground()
    {
        // 맵 배경 지우기
        for (int i = 0; i < MapBackgroundPool.transform.childCount; i++) Destroy(MapBackgroundPool.transform.GetChild(i).gameObject);
    }
}
