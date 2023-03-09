using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGroupManager : Singleton<UIGroupManager>
{
    // Start is called before the first frame update
    [SerializeField] GameObject HeartUI;
    [SerializeField] GameObject GoldUI;
    [SerializeField] GameObject DiamodUI;
    [SerializeField] GameObject OptionUI;

    public void TopUI_Off()
    {
        // ȭ�� ������� Heart, Gold, Diamond, Option UI ����
        HeartUI.SetActive(false);
        GoldUI.SetActive(false);
        DiamodUI.SetActive(false);
        OptionUI.SetActive(false);
    }

    public void TopUI_On()
    {
        // ȭ�� ������� Heart, Gold, Diamond, Option UI ����
        HeartUI.SetActive(true);
        GoldUI.SetActive(true);
        DiamodUI.SetActive(true);
        OptionUI.SetActive(true);
    }
}
