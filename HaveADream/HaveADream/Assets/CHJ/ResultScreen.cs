using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : Singleton<ResultScreen>
{
    bool isActive = false;
    public GameObject titleObject;


    private void OnEnable()
    {

        titleObject.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 0.05f), 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        titleObject.transform.DOLocalMove(new Vector2(-75f, 75f), 20f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
        isActive = true;

    }

    public void OpenResultScreen()
    {
        titleObject.SetActive(true);

    }

    public void ResultScreenButton_OnClick()
    {
        if (isActive == false) return;
        titleObject.transform.GetChild(0).DOScale(1.5f, 0.5f).OnComplete(() => { titleObject.gameObject.SetActive(false); });
        titleObject.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(0f, 0.4f);
        titleObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
