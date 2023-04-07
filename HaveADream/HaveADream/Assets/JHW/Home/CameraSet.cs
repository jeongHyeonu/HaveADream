using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    // 처음에 카메라 위치를 못 잡는 경우가 있어서 카메라를 고정하는 용도의 스크립트입니다.
    // StageSelect1의 Episode1의 Canvas, Episode2의 Canvas에 적용시킵니다.

    void Start()
    {
        this.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (this.name == "EffectManager")
        {
            this.GetComponent<Canvas>().planeDistance = 1;
            this.GetComponent<Canvas>().sortingOrder = 9999;
        }
    }
}
