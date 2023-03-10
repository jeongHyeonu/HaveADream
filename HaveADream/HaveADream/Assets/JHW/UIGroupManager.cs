using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        // 화면 우측상단 Heart, Gold, Diamond, Option UI 삭제
        HeartUI.SetActive(false);
        GoldUI.SetActive(false);
        DiamodUI.SetActive(false);
        OptionUI.SetActive(false);
    }

    public void TopUI_On()
    {
        // 화면 우측상단 Heart, Gold, Diamond, Option UI 등장
        HeartUI.SetActive(true);
        GoldUI.SetActive(true);
        DiamodUI.SetActive(true);
        OptionUI.SetActive(true);
    }

    public void ChangeHeartUI()
    {
        GameObject userHeart_text = HeartUI.transform.GetChild(0).GetChild(0).gameObject;
        userHeart_text.GetComponent<TextMeshProUGUI>().text = UserDataManager.Instance.GetUserData_heart().ToString() + "/" + UserDataManager.Instance.GetUserData_maxHeart().ToString();
    }
}
