using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeartManager : Singleton<HeartManager>
{
    [SerializeField] public TextMeshProUGUI heartText;
    [SerializeField] public TextMeshProUGUI heartRechargeTimeText;
    public float secondsLeftToRefresh = 1f;
    public bool isHeartClicked;
    private bool isDataLoaded;
    public bool isHeartZero;

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

    // Update is called once per frame
    void Update()
    {
        secondsLeftToRefresh -= Time.deltaTime;

        if (pfl.isLogined == false) return; // 로그인안되있으면 실행X
        if (!isDataLoaded) { PlayFabLogin.Instance.GetVirtualCurrencies(); isDataLoaded = true; } // 데이터 로드 안했다면 서버로부터 데이터 불러오기

        if (!SceneManager.Instance.GetIsHomeSceneActive() && !SceneManager.Instance.GetIsStageSelectSceneActive()) return; // 홈, 스테이지 셀렉트 씬 아니면 실행 X

        heartRechargeTimeText.gameObject.SetActive(isHeartClicked);
        if (isHeartClicked) // 유저가 하트 이미지 클릭 시
        {
            if (udm.GetUserData_heart() >= udm.GetUserData_maxHeart()) // 현재 유저가 가진 하트 체크 후 최대 체력이면 텍스트 변경 X
            {
                heartRechargeTimeText.gameObject.SetActive(false);
                return;
            }
            else
            {
                TimeSpan time = TimeSpan.FromSeconds(secondsLeftToRefresh);
                heartRechargeTimeText.text = time.ToString("mm':'ss");
                heartRechargeTimeText.gameObject.SetActive(isHeartClicked);
            }
        }
        if (isHeartZero) // 유저 데이터의 하트가 0이면
        {
            TimeSpan time = TimeSpan.FromSeconds(secondsLeftToRefresh);
            heartText.text = time.ToString("mm':'ss");
        }

        if (secondsLeftToRefresh < 0) PlayFabLogin.Instance.GetVirtualCurrencies();
    }
}
