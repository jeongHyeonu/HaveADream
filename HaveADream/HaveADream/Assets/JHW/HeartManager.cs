using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : Singleton<HeartManager>
{
    [SerializeField] public TextMeshProUGUI topUIheartText; // ȭ�� ��� UI �� ��Ʈ �ؽ�Ʈ

    [SerializeField] public TextMeshProUGUI inUIheartText; // ��� UI ��Ʈ �̹��� Ŭ���� �����ϴ� UI�� ������ �ؽ�Ʈ
    [SerializeField] public TextMeshProUGUI heartRechargeTimeText;

    [SerializeField] public Button adsButton; // ���� ��ư

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

    // �����κ��� ��Ʈ ���� �ִ� ��Ʈ �ҷ�����, ���� �ֱ⸶�� ����, Ȩ/�������� �ƴϸ� ����X
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
        if (pfl.isLogined == false) return; // �α��ξȵ������� ����X
        if (!isDataLoaded) { StartCoroutine("getUserHeartPeriod"); isDataLoaded = true; } // ������ �ε� ���ߴٸ� �����κ��� ������ �ҷ����� // ? �ڷ�ƾ���� �����ֱ� �����ؾ��ϳ�..?


        if (UserDataManager.Instance.GetUserData_heart() >= UserDataManager.Instance.GetUserData_maxHeart()) // ��Ʈ�� �ִ� ��Ʈ���� �Ѵ°�� ���� X
        {
            secondsLeftToRefresh = 0;
            heartRechargeTimeText.text = "00:00";
            adsButton.GetComponent<Button>().interactable = false;
            return;
        }
        else // ��Ʈ�� �ִ� �� �ȳѴ°��
        {
            adsButton.GetComponent<Button>().interactable = true;
        }

        if (!SceneManager.Instance.GetIsHomeSceneActive() && !SceneManager.Instance.GetIsStageSelectSceneActive()) return; // Ȩ, �������� ����Ʈ �� �ƴϸ� ���� X

        heartRechargeTimeText.gameObject.SetActive(isHeartClicked);
        if (isHeartClicked) // ������ ��Ʈ �̹��� Ŭ�� ��
        {
            TimeSpan time = TimeSpan.FromSeconds(secondsLeftToRefresh);
            heartRechargeTimeText.text = time.ToString("mm':'ss");
            heartRechargeTimeText.gameObject.SetActive(isHeartClicked);
            inUIheartText.text = UserDataManager.Instance.GetUserData_heart().ToString() + "/" + UserDataManager.Instance.GetUserData_maxHeart(); 
        }
        if (isHeartZero) // ���� �������� ��Ʈ�� 0�̸�
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
