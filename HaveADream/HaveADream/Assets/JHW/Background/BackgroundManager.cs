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
        // ¸Ê »ý¼º
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



    public void EraseBackground()
    {
        // ¸Ê ¹è°æ Áö¿ì±â
        for (int i = 0; i < MapBackgroundPool.transform.childCount; i++) Destroy(MapBackgroundPool.transform.GetChild(i).gameObject);
    }
}
