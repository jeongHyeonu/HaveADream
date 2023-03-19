using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BroadcastManager : Singleton<BroadcastManager>
{
    [SerializeField] public GameObject BroadcastUI;
    [SerializeField] GameObject BroadcastItem;
    [SerializeField] GameObject BroadcastBackground;

    public Dictionary<string, string> broadCast = null;

    public void Init_Broadcast()
    {
        BroadcastBackground.transform.SetParent(HomeManager.Instance.transform.GetChild(0));
        BroadcastBackground.transform.localPosition = Vector2.zero;
        BroadcastBackground.transform.localScale = Vector2.one;

        BroadcastUI.transform.SetParent(HomeManager.Instance.transform.GetChild(0));
        BroadcastUI.transform.localPosition = Vector2.zero;
        BroadcastUI.transform.localScale = Vector2.one;
    }

    public void BroadcastButton_OnClick()
    {
        OpenBroadcastUI();
        PlayFabLogin.Instance.ReadTitleNews();
    }

    public void CreateBroadcastItem(TitleNewsItem item)
    {
        GameObject newsItem = Instantiate(BroadcastItem);

        // 시간
        newsItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Timestamp.ToString();
        // 제목
        newsItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.Title.ToString();

        GameObject BroadcastCanvas = BroadcastUI.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        newsItem.transform.SetParent(BroadcastCanvas.transform);
        newsItem.transform.localScale = Vector2.one;
        BroadcastCanvas.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(BroadcastCanvas.transform.GetComponent<RectTransform>().sizeDelta.x, BroadcastCanvas.transform.GetComponent<RectTransform>().sizeDelta.y + newsItem.GetComponent<RectTransform>().rect.height+20f);

        newsItem.SetActive(true);
    }

    public void BackgroundClick()
    {
        BroadcastUI.SetActive(false);
    }

    public void OpenBroadcastUI()
    {
        BroadcastUI.SetActive(true);
        BroadcastBackground.SetActive(true);
    }
}
