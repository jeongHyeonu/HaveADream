using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TitleScreen : Singleton<TitleScreen>
{
    bool isActive = false;
    public GameObject titleObject;

    private void OnEnable()
    {

        titleObject.transform.GetChild(0).DOScale(1.0f, 2f).SetEase(Ease.InCirc).From(2f).OnComplete(() =>
        {
            titleObject.transform.GetChild(1).gameObject.SetActive(true);
            titleObject.transform.GetChild(0).gameObject.transform.DOScale(new Vector2(1.1f, 1.1f), 3f).SetLoops(-1, LoopType.Yoyo);
            titleObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().DOColor(new Color(0.5f, 0.5f, .5f), 1f).From(new Color(0f, 0f, .0f)).SetLoops(-1, LoopType.Yoyo);
            isActive = true;
        });
    }

    public void OpenTitleScreen()
    {
        titleObject.SetActive(true);
        titleObject.transform.GetChild(0).gameObject.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), 1f).From(new Color(0f,0f,0f)).SetDelay(0.5f);
        HomeManager.Instance.Home_OpenAnials();

    }

    public void TitleScreenButton_OnClick()
    {
        if (isActive == false) return;
        titleObject.transform.GetChild(0).DOScale(1.5f, 0.5f).OnComplete(() => { titleObject.gameObject.SetActive(false); });
        titleObject.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(0f, 0.4f);
        titleObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
