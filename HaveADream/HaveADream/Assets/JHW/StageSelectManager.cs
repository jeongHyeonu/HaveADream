using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JHW
{
    partial class StageSelectManager : Singleton<StageSelectManager>
    {
        private SceneManager sm = null;
        private int curEpiNum; // ���� ���� ���Ǽҵ� ��ȣ
        private string userCurStage; // ������ ���� ������ ��������
        private GameObject userClickedStage; // ������ ���� ������ ��������
        Sequence ratTween;

        [SerializeField] private GameObject userMarker = null;
        [SerializeField] private GameObject returnToWorldBtn = null;
        [SerializeField] private GameObject StageInfo = null;

        // Start is called before the first frame update
        void Start()
        {
            // �̱��� �޾ƿ���
            sm = SceneManager.Instance;
            userClickedStage = null;
            ratTween = DOTween.Sequence();
        }

        public void OnEnableStageSelect() // �������� ���� �� Ȱ��ȭ��
        {
            // �����ư Ȱ��ȭ
            returnToWorldBtn.SetActive(true);

            // �������� ���� â Ȱ��ȭ ��
            // ������ ���������� ������ ��������, ���� �������� üũ �� �� ��ġ�� �÷��̾�(������) ��ġ��Ű��
            string userCurrentStage = UserDataManager.Instance.GetUserData_userCurrentStage();
            string[] userCurrentStages = userCurrentStage.Split('-');
            this.transform.GetChild(int.Parse(userCurrentStages[0].ToCharArray())).gameObject.SetActive(true); // ���Ǽҵ� �� Ȱ��ȭ
            userMarker.transform.SetParent(GameObject.Find("Stage" + userCurrentStage).transform); // �������� ��ư�� ������ ��ġ
            userMarker.transform.localPosition = Vector2.zero;
            userMarker.GetComponent<Transform>().localScale = Vector2.one; // ���� ������

            Init_StageSelect_By_UserInfo(int.Parse(userCurrentStages[0].ToCharArray())); // �������� ���� �ҷ�����
        }

        public void HomeButton_OnClick() // Ȩ ȭ������ ����
        {
            // ����â ���������� ����
            StageInfo.SetActive(false);

            // ������ ������ �������� ũ�� ���󺹱�
            if (userClickedStage != null) userClickedStage.transform.DOScale(1f, 0f);

            sm.Scene_Change_Home();
        }

        
        public void EpisodeButton_OnClick([SerializeField] int episodeNumber) // ���Ǽҵ� Ŭ����
        {
            // ����â ���������� ����
            StageInfo.SetActive(false);

            // ��������� ���ư��� ��ư �ѱ�
            returnToWorldBtn.SetActive(true);

            // �÷��̾� ��ġ�� �� n-1 �� ����
            UserDataManager.Instance.setUserData_userCurrentStage(episodeNumber.ToString() + "-1");
            userMarker.transform.SetParent(this.transform.GetChild(episodeNumber).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1));

            userMarker.transform.DOLocalMove(Vector2.zero, 0f); // ���� ��ġ��
            userMarker.GetComponent<Transform>().localScale = Vector2.one; // ���� ������

            this.transform.GetChild(episodeNumber).gameObject.SetActive(true);
            this.transform.GetChild(0).gameObject.SetActive(false);
            Init_StageSelect_By_UserInfo(episodeNumber);
        }

        
        public void WorldButton_OnClick() // ����� ���ư��� ��ư Ŭ����
        {
            // ����â ���������� ����
            StageInfo.SetActive(false);

            // ��������� ���ư��� ��ư ����
            returnToWorldBtn.SetActive(false);

            // ������ ���� �������� ��ư �ٽ� �����·�
            if(userClickedStage!=null)
            userClickedStage.GetComponent<Transform>().localScale = Vector2.one; // ���� ������

            // ������ ��ġ
            DOTween.KillAll();
            userMarker.transform.localPosition = Vector2.zero;
            

            GameObject TargetButton = this.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject;
            // ���� ���Ǽҵ� Ŭ���� ���� �˻�
            // ���Ǽҵ� 1 �رݿ��� - �˻���ϰ� �ٷ� interactable = true
            TargetButton.transform.GetChild(0).GetComponent<Button>().interactable = true;
            // ���Ǽҵ� 2 �رݿ���
            if (UserDataManager.Instance.GetUserData_userEpi1Data().Find(x => x.mapName == "1-13").isClearStage == true) TargetButton.transform.GetChild(1).GetComponent<Button>().interactable = true;
            else TargetButton.transform.GetChild(1).GetComponent<Button>().interactable = false;
            // ���Ǽҵ� 3 �رݿ���
            if (UserDataManager.Instance.GetUserData_userEpi2Data().Find(x => x.mapName == "2-18").isClearStage == true) TargetButton.transform.GetChild(2).GetComponent<Button>().interactable = true;
            else TargetButton.transform.GetChild(2).GetComponent<Button>().interactable = false;

            curEpiNum = GetCurrentEpisodeNumber();
            this.transform.GetChild(curEpiNum).gameObject.SetActive(false);
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

        public void StageButton_OnClick([SerializeField] int currentStageNumber) // �������� ��ư Ŭ����
        {
            // ������ ���� Ŭ���� �������� string ������ ���� �� ���Ǽҵ� ��ȣ üũ
            curEpiNum = GetCurrentEpisodeNumber();

            //UX - Ŭ���� ��ư ũ��
            if (userClickedStage != null)
            {
                userClickedStage.transform.DOScale(1f, 0.5f);
            }
            userClickedStage = this.transform.GetChild(curEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(currentStageNumber).gameObject;
            userClickedStage.transform.DOScale(1.4f, 0.5f);
            //UX - ������ ������
            userMarker.transform.SetParent(userClickedStage.transform.parent);
            Vector2 originPos = userMarker.transform.localPosition;
            ratTween.Append(userMarker.transform.DOLocalMove(userClickedStage.transform.localPosition, 2f).From(originPos, false));// ()=> { userMarker.transform.position = originVec; }); // ������ �̵�

            userCurStage = curEpiNum.ToString() + "-" + currentStageNumber.ToString();
            UserDataManager.Instance.setUserData_userCurrentStage(userCurStage); // ������ ���������� ������ �������� ��ġ

            // �����÷��̷� ��ȯ
            // sm.Scene_Change_GamePlay();

            // �������� ����â ����
            OpenStageInfoPanel();
        }

        public void BackToEpisode_OnClick()
        {
            // ����â ���������� ����
            StageInfo.SetActive(false);

            // ����� ���ư��� ��ư Ȱ��ȭ
            returnToWorldBtn.SetActive(true);

            //����ȭ�� �ݰ� �������� ����â����
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(curEpiNum).gameObject.SetActive(true);
        }

        private int GetCurrentEpisodeNumber() // ���� �����ִ� ���Ǽҵ� ��ȣ ����
        {
            int num = 0;
            while (true)
            {
                if (this.transform.GetChild(++num).gameObject.activeSelf == true) return num;
            }
        }

        private void Init_StageSelect_By_UserInfo(int _stageNum) // ���� �����Ϳ� �ִ´�� �������� �ʱ�ȭ
        {
            GameObject targetStage = this.transform.GetChild(_stageNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            // ���� ���Ǽҵ� ��ȣ (_stageNum) �� 1�̸� StageSelect1/Episode1/Canvas/Scroll View/Viewport/Content , (���⼭ GetChild(1)�� Stage1-1 �������� ������Ʈ ����)
            int _stageIdx; // �������� �ε�����ȣ
            int roadCnt = 0; // ���� ��, ���߿� RoadCreater���� ��, UX ����

            switch (_stageNum)
            {
                case 1:
                    List<Episode1Data> episode1Datas = UserDataManager.Instance.GetUserData_userEpi1Data();
                     _stageIdx = 0;

                    foreach (Episode1Data it in episode1Datas)
                    {
                        _stageIdx++;
                        // ��ư Ȱ��ȭ
                        targetStage.transform.GetChild(_stageIdx).GetComponent<Button>().interactable = true;
                        targetStage.transform.GetChild(_stageIdx).GetChild(0).gameObject.SetActive(true);
                        // �� ����
                        for (int i = 0; i < it.star; i++) 
                        {
                            targetStage.transform.GetChild(_stageIdx).GetChild(2).GetChild(i).GetChild(0).gameObject.SetActive(true);
                        }
                        if (it.isClearStage == false) break;

                        // ���� �� +1
                        roadCnt++;
                    }
                    // ���� �������� �� ������ ���� �������� ���� ��ư Ȱ��ȭ
                    if (_stageIdx == episode1Datas.Count) targetStage.transform.GetChild(_stageIdx + 1).gameObject.SetActive(true);

                    break;

                case 2:
                    List<Episode2Data> episode2Datas = UserDataManager.Instance.GetUserData_userEpi2Data();
                    _stageIdx = 0;

                    foreach (Episode2Data it in episode2Datas)
                    {
                        _stageIdx++;
                        // ��ư Ȱ��ȭ
                        targetStage.transform.GetChild(_stageIdx).GetComponent<Button>().interactable = true;
                        targetStage.transform.GetChild(_stageIdx).GetChild(0).gameObject.SetActive(true);

                        // �� ����
                        for (int i = 0; i < it.star; i++)
                        {
                            targetStage.transform.GetChild(_stageIdx).GetChild(2).GetChild(i).GetChild(0).gameObject.SetActive(true);
                        }
                        if (it.isClearStage == false) break;

                        // ���� �� +1
                        roadCnt++;
                    }
                    // ���� �������� �� ������ ���� �������� ���� ��ư Ȱ��ȭ
                    if (_stageIdx == episode2Datas.Count) targetStage.transform.GetChild(_stageIdx + 1).gameObject.SetActive(true);

                    break;
            }
            StartCoroutine(targetStage.GetComponent<RoadCreater>().RoadColorChange(roadCnt)); // Ŭ������ �������� ���� ���� ��������� ��ĥ
        }
    }

    #region �������� ����â
    partial class StageSelectManager : Singleton<StageSelectManager>
    {
        private void OpenStageInfoPanel()
        {
            // �������� ����â Ȱ��ȭ
            StageInfo.SetActive(true);

            // ������ Ŭ���� ���Ǽҵ� ��ȣ�� ���� �� ���� ã�Ƽ� �ݿ�
            int starCnt = 0;  // �� ����
            for (int i = 0; i < 3; i++) { StageInfo.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false); } // �ʱ� �� �̹��� �Ⱥ��̰�
            switch (curEpiNum)
            {
                case 1:
                    List<Episode1Data> epi1 = UserDataManager.Instance.GetUserData_userEpi1Data();
                    starCnt = epi1.Find(x => x.mapName == userCurStage).star;
                    break;
                case 2:
                    List<Episode2Data> epi2 = UserDataManager.Instance.GetUserData_userEpi2Data();
                    starCnt = epi2.Find(x => x.mapName == userCurStage).star;
                    break;
            }
            for (int i = 0; i < starCnt; i++) { StageInfo.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(true); }

            StageInfo.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = userCurStage; // �������� ��ȣ
        }

        public void StageStartButton_OnClick()
        {
            // ��Ʈ �������� �÷��� �Ұ����Ҷ�
            if (UserDataManager.Instance.GetUserData_heart() == 0) return;

            // ����â �ݱ�
            StageInfo.SetActive(false);

            // ���� �÷��̷� ��ȯ
            sm.Scene_Change_GamePlay();

            // Ŭ���� ��ư ũ�� �������
            userClickedStage.transform.localScale = Vector2.one;

            // ���� �����Ϳ��� ��Ʈ ����(���� ������ �� prefab ������ �� ��) ���ҽ�Ų�� UI ����
            PlayFabLogin.Instance.SubtractHeart();
            UserDataManager.Instance.SetUserData_heart(UserDataManager.Instance.GetUserData_heart()-1);
            UIGroupManager.Instance.ChangeHeartUI();
        }

        public void StageInfoExitButton_OnClick()
        {
            // Ŭ���� ��ư ũ�� �������
            userClickedStage.transform.localScale = Vector2.one;

            // ����â �ݱ�
            StageInfo.SetActive(false);
        }
    }

    #endregion

    #region ���Ǽҵ� ����/���� ��ư
    partial class StageSelectManager
    {
        
        public void prevButton_OnClick()
        {
            // ����â Ȱ��ȭ���¸� ����
            if (StageInfo.activeSelf == true) StageInfo.SetActive(false);

            // ���� ���µ� ���Ǽҵ� ��ȣ
            int currentEpiNum = GetCurrentEpisodeNumber();
            this.transform.GetChild(currentEpiNum).gameObject.SetActive(false); // ���� ���µȰ� �ݰ�
            this.transform.GetChild(--currentEpiNum).gameObject.SetActive(true); // �� ������ ����

            DOTween.KillAll();

            // �÷��̾� ��ġ�� �� n-1 �� ����
            UserDataManager.Instance.setUserData_userCurrentStage(currentEpiNum.ToString() + "-1");
            userMarker.transform.SetParent(this.transform.GetChild(currentEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1));

            userMarker.transform.DOLocalMove(Vector2.zero, 0f); // ���� ��ġ��
            userMarker.GetComponent<Transform>().localScale = Vector2.one; // ���� ������

            Init_StageSelect_By_UserInfo(currentEpiNum);
        }

        public void nextButton_OnClick()
        {
            // ����â Ȱ��ȭ���¸� ����
            if (StageInfo.activeSelf == true) StageInfo.SetActive(false);

            // ���� ���µ� ���Ǽҵ� ��ȣ
            int currentEpiNum = GetCurrentEpisodeNumber();
            this.transform.GetChild(currentEpiNum).gameObject.SetActive(false); // ���� ���µȰ� �ݰ�
            this.transform.GetChild(++currentEpiNum).gameObject.SetActive(true); // �� ������ ����

            DOTween.KillAll();

            // �÷��̾� ��ġ�� �� n-1 �� ����
            UserDataManager.Instance.setUserData_userCurrentStage(currentEpiNum.ToString() + "-1");
            userMarker.transform.SetParent(this.transform.GetChild(currentEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1));
            
            ratTween.Kill();

            userMarker.transform.DOLocalMove(Vector2.zero, 0f); // ���� ��ġ��
            userMarker.GetComponent<Transform>().localScale = Vector2.one; // ���� ������

            Init_StageSelect_By_UserInfo(currentEpiNum);
        }
    }
    #endregion
}