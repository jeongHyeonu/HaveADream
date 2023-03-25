using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public partial class AdsManager : MonoBehaviour, IUnityAdsListener // ������ ���� ������ �ô���, ���ٰ� �����ߴ��� Ȯ�� ���� AdsListener �� �ִ´�
{
#if UNITY_IOS
    string gameId = "5213487";
#else
    string gameId = "5213486";
#endif
    private void Start()
    {
        Advertisement.Initialize(gameId);
        Advertisement.AddListener(this);
        ShowBanner();
    }

    public void PlayRewardedAd()
    {
        if (Advertisement.IsReady("Rewarded_Android"))
        {
            Advertisement.Show("Rewarded_Android");
        }
        else
        {
            Debug.Log("������ ���� �غ���� �ʾҽ��ϴ�!");
        }
    }

    public void ShowBanner()
    {
        if (Advertisement.IsReady("banner"))
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show("banner");
        }
        else
        {
            StartCoroutine(RepeatShowBanner());
        }
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    IEnumerator RepeatShowBanner()
    {
        yield return new WaitForSeconds(1);
        ShowBanner();
    }
}
partial class AdsManager // 
{ 
    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("ADS ARE READY!");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("ADS error "+message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Rewarded ad is not ready!");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log(showResult);
        if(placementId== "Rewarded_Android" && showResult == ShowResult.Finished)
        {
            Debug.Log("player should be rewarded!");
            PlayFabLogin.Instance.AddHeart();
            PlayFabLogin.Instance.GetVirtualCurrencies();
            UIGroupManager.Instance.ChangeHeartUI();
        }
    }
}
