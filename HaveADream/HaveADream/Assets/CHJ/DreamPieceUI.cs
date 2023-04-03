using UnityEngine;
using UnityEngine.UI;

public class DreamPieceUI : MonoBehaviour
{

    [SerializeField] Image DpBar;
    [SerializeField] float GoalScore;
    public float DpScore;


    void Awake()
    {
        DpBar.GetComponent<Image>();
        //DataManager.Instance.DreamPieceScore = DpScore;
        DpScore = 0.0f;
    }

    void Update()
    {
        if (DpBar != null)
        {

            DpBar.fillAmount = DataManager.Instance.DreamPieceScore / GoalScore;
        }
    }
}
