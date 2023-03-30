using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionManager : Singleton<OcclusionManager>
{
    [SerializeField] GameObject MapPresets_1;
    [SerializeField] GameObject MapPresets_2;
    [SerializeField] GameObject MapPresets_3;
    [SerializeField] GameObject MapPresets_4;
    [SerializeField] GameObject MapPresetSpawnPoint;

    private int floorSquareCount = 13; // 바닥에 놓인 사각형 타일 수
    private float floorSize = 2f; // 타일 사각형 크기


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapFloor"))
        {
            GameObject targetObject = collision.gameObject.transform.parent.GetChild(floorSquareCount).gameObject;
            collision.transform.position = new Vector3(targetObject.transform.position.x+ floorSize, collision.transform.position.y);
            collision.transform.SetAsLastSibling();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapPresetStartPoint"))
        {
            // 맵 프리셋 101~136
            GameObject targetObject_1 = GetRandomMapPreset_1();
            targetObject_1.transform.position = MapPresetSpawnPoint.transform.position;
            targetObject_1.SetActive(true);

            // 맵 프리셋 301~309
            GameObject targetObject_3 = GetRandomMapPreset_3();
            targetObject_3.transform.position = MapPresetSpawnPoint.transform.position;
            targetObject_3.SetActive(true);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("MapPresetEndPoint"))
        {
            collision.transform.parent.gameObject.SetActive(false);

        }
    }

    private GameObject GetRandomMapPreset_1()
    {
        int randInt = Random.Range(0, 35);
        return MapPresets_1.transform.GetChild(randInt).gameObject;
    }

    private GameObject GetRandomMapPreset_3()
    {
        int randInt = Random.Range(0, 8);
        return MapPresets_3.transform.GetChild(randInt).gameObject;
    }
}
