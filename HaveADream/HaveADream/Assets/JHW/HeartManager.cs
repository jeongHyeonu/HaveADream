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

        if (pfl.isLogined == false) return; // �α��ξȵ������� ����X
        if (!isDataLoaded) { PlayFabLogin.Instance.GetVirtualCurrencies(); isDataLoaded = true; } // ������ �ε� ���ߴٸ� �����κ��� ������ �ҷ�����

        if (!SceneManager.Instance.GetIsHomeSceneActive() && !SceneManager.Instance.GetIsStageSelectSceneActive()) return; // Ȩ, �������� ����Ʈ �� �ƴϸ� ���� X

        heartRechargeTimeText.gameObject.SetActive(isHeartClicked);
        if (isHeartClicked) // ������ ��Ʈ �̹��� Ŭ�� ��
        {
            if (udm.GetUserData_heart() >= udm.GetUserData_maxHeart()) // ���� ������ ���� ��Ʈ üũ �� �ִ� ü���̸� �ؽ�Ʈ ���� X
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
        if (isHeartZero) // ���� �������� ��Ʈ�� 0�̸�
        {
            TimeSpan time = TimeSpan.FromSeconds(secondsLeftToRefresh);
            heartText.text = time.ToString("mm':'ss");
        }

        if (secondsLeftToRefresh < 0) PlayFabLogin.Instance.GetVirtualCurrencies();
    }
}
