using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : Singleton<HeartManager>
{
    [SerializeField] public TextMeshProUGUI topUIheartText; // 화면 상단 UI 내 하트 텍스트

    [SerializeField] public TextMeshProUGUI inUIheartText; // 상단 UI 하트 이미지 클릭시 등장하는 UI에 나오는 텍스트
    [SerializeField] public TextMeshProUGUI heartRechargeTimeText;

    [SerializeField] public Button adsButton; // 광고 버튼

    [SerializeField] public GameObject HeartUI;

    public float secondsLeftToRefresh = 0f;
    public bool isHeartClicked;
    private bool isDataLoaded;
    public bool isHeartZero;

    private int userHeartCnt = 0;

    UserDataManager udm = null;
    PlayFabLogin pfl = null;

    private void Start()
    {
        pfl = PlayFabLogin.Instance;
        udm = UserDataManager.Instance;
        isHeartClicked = false;
        isDataLoaded = false;
        isHeartZero = false;
    }

    public void BackgroundClick()
    {
        HeartUI.SetActive(false);
    }

    // 서버로부터 하트 수와 최대 하트 불러오기, 일정 주기마다 실행, 홈/스테이지 아니면 실행X
    IEnumerator getUserHeartPeriod()
    {
        yield return new WaitForSeconds(5f);
        //if (SceneManager.Instance.GetIsGamePlaying())
            PlayFabLogin.Instance.GetVirtualCurrencies();
        StartCoroutine("getUserHeartPeriod");
    }

    void Update()
    {
        secondsLeftToRefresh -= Time.deltaTime;

        if (pfl == null) return;
        if (pfl.isLogined == false) return; // 로그인안되있으면 실행X
        if (!isDataLoaded) { StartCoroutine("getUserHeartPeriod"); isDataLoaded = true; } // 데이터 로드 안했다면 서버로부터 데이터 불러오기 // ? 코루틴으로 일정주기 실행해야하나..?


        if (UserDataManager.Instance.GetUserData_heart() >= UserDataManager.Instance.GetUserData_maxHeart()) // 하트가 최대 하트수를 넘는경우 실행 X
        {
            secondsLeftToRefresh = 0;
            heartRechargeTimeText.text = "00:00";
            adsButton.GetComponent<Button>().interactable = false;
            return;
        }
        else // 하트가 최대 수 안넘는경우
        {
            adsButton.GetComponent<Button>().interactable = true;
        }

        if (!SceneManager.Instance.GetIsHomeSceneActive() && !SceneManager.Instance.GetIsStageSelectSceneActive()) return; // 홈, 스테이지 셀렉트 씬 아니면 실행 X

        heartRechargeTimeText.gameObject.SetActive(isHeartClicked);
        if (isHeartClicked) // 유저가 하트 이미지 클릭 시
        {
            TimeSpan time = TimeSpan.FromSeconds(secondsLeftToRefresh);
            heartRechargeTimeText.text = time.ToString("mm':'ss");
            heartRechargeTimeText.gameObject.SetActive(isHeartClicked);
            inUIheartText.text = UserDataManager.Instance.GetUserData_heart().ToString() + "/" + UserDataManager.Instance.GetUserData_maxHeart(); 
        }
        if (isHeartZero) // 유저 데이터의 하트가 0이면
        {
            TimeSpan time = TimeSpan.FromSeconds(secondsLeftToRefresh);
            topUIheartText.text = time.ToString("mm':'ss");
            inUIheartText.text = "0/15";
        }

        if (secondsLeftToRefresh < 0) ; //PlayFabLogin.Instance.GetVirtualCurrencies();
    }

    IEnumerator GetHeartOneSecond()
    {
        
        PlayFabLogin.Instance.GetVirtualCurrencies();
        yield return new WaitForSeconds(1f);
    }
}
