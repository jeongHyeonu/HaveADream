using UnityEngine;
using UnityEngine.UI;

public class DreamPieceUI : MonoBehaviour
{

    [SerializeField] Image BackgroundBar;
    [SerializeField] int GoalScore;
    public float currentScore;

    private void Update()
    {
        BackgroundBar.fillAmount = currentScore / GoalScore;
    }
}
