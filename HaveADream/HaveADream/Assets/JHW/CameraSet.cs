using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    // ó���� ī�޶� ��ġ�� �� ��� ��찡 �־ ī�޶� �����ϴ� �뵵�� ��ũ��Ʈ�Դϴ�.
    // StageSelect1�� Episode1�� Canvas, Episode2�� Canvas�� �����ŵ�ϴ�.

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
