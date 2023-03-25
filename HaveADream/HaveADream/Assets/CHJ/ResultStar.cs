using UnityEngine;

public class ResultStar : Singleton<ResultStar>
{

    [SerializeField] GameObject Star1;
    [SerializeField] GameObject Star2;
    [SerializeField] GameObject Star3;

    void Start()
    {
        gameObject.SetActive(true);
        CheckStar();
        //별 0개인 경우
    }
    void CheckStar()
    {
        Debug.Log("호출");
        if (DataManager.Instance.ResultStars == 0)
        {
            Debug.Log("별1");
            Star1.gameObject.SetActive(false);
            Star2.gameObject.SetActive(false);
            Star3.gameObject.SetActive(false);

        }
        else if (DataManager.Instance.ResultStars == 1)
        {
            Star1.SetActive(true);
            Star2.SetActive(false);
            Star3.SetActive(false);
        }
        else if (DataManager.Instance.ResultStars == 2)
        {
            Star1.gameObject.SetActive(true);
            Star2.gameObject.SetActive(true);
            Star3.gameObject.SetActive(false);
        }
        else if (DataManager.Instance.ResultStars == 2 && DataManager.Instance.DreamPieceScore >= 6)
        {
            Star1.SetActive(true);
            Star2.gameObject.SetActive(true);
            Star3.gameObject.SetActive(true);
        }
    }

}
