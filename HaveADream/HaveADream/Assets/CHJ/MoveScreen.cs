using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MoveScreen : MonoBehaviour
{
    [SerializeField] GameObject Screen;

    private void OnEnable()
    {

        //UX
        Screen.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 0.05f), 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        Screen.transform.DOLocalMove(new Vector2(-75f, 75f), 20f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
    }

}
