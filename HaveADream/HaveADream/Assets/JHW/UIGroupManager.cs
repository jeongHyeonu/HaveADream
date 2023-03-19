using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGroupManager : Singleton<UIGroupManager>
{
    // Start is called before the first frame update
    [SerializeField] GameObject CollectionButton;
    [SerializeField] GameObject HeartUI;
    [SerializeField] GameObject BroadcastButton;
    [SerializeField] GameObject OptionButton;
    [SerializeField] GameObject BackgroundUI;
    [SerializeField] GameObject FadeBackground;

    public void TopUI_Off()
    {
        // 화면 우측상단 Heart, Gold, Diamond, Option UI 삭제
        CollectionButton.SetActive(false);
        HeartUI.SetActive(false);
        BroadcastButton.SetActive(false);
        OptionButton.SetActive(false);
    }

    public void TopUI_On()
    {
        // 화면 우측상단 Heart, Gold, Diamond, Option UI 등장
        CollectionButton.SetActive(true);
        HeartUI.SetActive(true);
        BroadcastButton.SetActive(true);
        OptionButton.SetActive(true);
    }

    public void ChangeHeartUI()
    {
        GameObject userHeart_text = HeartUI.transform.GetChild(0).GetChild(0).gameObject;
        userHeart_text.GetComponent<TextMeshProUGUI>().text = UserDataManager.Instance.GetUserData_heart().ToString() + "/" + UserDataManager.Instance.GetUserData_maxHeart().ToString();
    }

    public void Background_OnClick()
    {
        CollectionManager.Instance.BackgroundClick();
        BroadcastManager.Instance.BackgroundClick();
        OptionManager.Instance.BackgroundClick();
        BackgroundUI.SetActive(false);
    }

    public void HeartImg_OnClick()
    {
        HeartManager.Instance.isHeartClicked = !HeartManager.Instance.isHeartClicked;
    }

    public void FadeInOut()
    {
        float fadeTime = 1f;
        FadeBackground.GetComponent<Image>().DOFade(0f, fadeTime);

    }
}
