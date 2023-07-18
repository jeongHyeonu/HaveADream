using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JHW
{
    partial class StageSelectManager : Singleton<StageSelectManager>
    {
        private SceneManager sm = null;
        public int curEpiNum; // ���� ���� ���Ǽҵ� ��ȣ
        private string userCurStage; // ������ ���� ������ ��������
        private GameObject userClickedStage; // ������ ���� ������ ��������
        Sequence ratTween;

        [SerializeField] private GameObject userMarker = null;
        [SerializeField] private GameObject returnToWorldBtn = null;
        [SerializeField] private GameObject StageInfo = null;

        private bool isMarkerMoving = false; // ������ �̵������� �˻�
        private Animator jerryAnim; // ������ �ִϸ��̼�
        int currentStageNumber = 0;

        // Start is called before the first frame update
        void Start()
        {
            // �̱��� �޾ƿ���
            sm = SceneManager.Instance;
            userClickedStage = null;
            ratTween = DOTween.Sequence();

            // ������ �̵� �ִϸ��̼�
            jerryAnim = userMarker.GetComponent<Animator>();
            userMarker.SetActive(true);
            jerryAnim.SetBool("isJerryMoving", false);
        }

        void Fade_StageToHome()
        {
            float loadingTime = 0.5f;
            GameObject FadeImg = GameObject.Find("LoadingBackground");
            for (int i = 0; i < FadeImg.transform.childCount; i++)
            {
                FadeImg.transform.GetChild(i).gameObject.SetActive(true);
                FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(1f, loadingTime).OnComplete(() =>
                {
                    // ���� �����ִ� ���Ǽҵ� ���� �ݱ�
                    this.transform.GetChild(curEpiNum).gameObject.SetActive(false);
                    if (i == 5)
                        sm.Scene_Change_Home();
                });
                FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(0f, loadingTime).SetDelay(loadingTime).OnComplete(() =>
                {
                    FadeImg.transform.GetChild(0).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(1).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(2).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(3).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(4).gameObject.SetActive(false);
                });
            }
        }
        void Fade_StageToPlay()
        {
            bool isActive = false;
            float loadingTime = 0.5f;
            GameObject FadeImg = GameObject.Find("LoadingBackground");
            for (int i = 0; i < FadeImg.transform.childCount; i++)
            {
                FadeImg.transform.GetChild(i).gameObject.SetActive(true);
                FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(1f, 0.1f).OnComplete(() =>
                {
                    // ���� �����ִ� ���Ǽҵ� ���� �ݱ�
                    this.transform.GetChild(curEpiNum).gameObject.SetActive(false);
                    if (isActive==false)
                    {
                        isActive = true;
                        // ����â �ݱ�
                        StageInfo.SetActive(false);



                        // Ŭ���� ��ư ũ�� �������
                        userClickedStage.transform.localScale = Vector2.one;

                        // ���� �����Ϳ��� ��Ʈ ����(���� ������ �� prefab ������ �� ��) ���ҽ�Ų�� UI ����
                        PlayFabLogin.Instance.SubtractHeart();
                        UserDataManager.Instance.SetUserData_heart(UserDataManager.Instance.GetUserData_heart() - 1);
                        UIGroupManager.Instance.ChangeHeartUI();


                        // ���� �÷��̷� ��ȯ
                        sm.Scene_Change_GamePlay();
                        // �� ����
                        BackgroundManager.Instance.GenerateBackground(curEpiNum);

                        // ���� ���
                        switch (curEpiNum)
                        {
                            case 1:
                                SoundManager.Instance.PlayBGM(SoundManager.BGM_list.GamePlayBGM_1);
                                break;
                            case 2:
                                SoundManager.Instance.PlayBGM(SoundManager.BGM_list.GamePlayBGM_2);
                                break;
                            default:
                                SoundManager.Instance.PlayBGM(SoundManager.BGM_list.GamePlayBGM_1);
                                break;
                        }
                    }
                });
                FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(0f, loadingTime).SetDelay(loadingTime).OnComplete(() =>
                {
                    FadeImg.transform.GetChild(0).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(1).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(2).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(3).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(4).gameObject.SetActive(false);
                });
            }
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
            returnToWorldBtn.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Episode " + int.Parse(userCurrentStages[0].ToCharArray()); // ������� ���Ǽҵ� ��ȣ
            Transform targetTf = GameObject.Find("Stage" + userCurrentStage).transform;
            userClickedStage = GameObject.Find("Stage" + userCurrentStage);
            userMarker.transform.SetParent(targetTf); // �������� ��ư�� ������ ��ġ
            userMarker.transform.localPosition = Vector2.zero;
            //userMarker.GetComponent<Transform>().localScale = Vector2.one; // ������

            Init_StageSelect_By_UserInfo(int.Parse(userCurrentStages[0].ToCharArray())); // �������� ���� �ҷ�����

            // �÷��̾� ��ġ�� ���� ��ġ��Ű��
            //Invoke("PlayerPosOnMap", 0.01f);
            // Ŭ���� ���������� ī�޶� �̵� (����� sliderView�� ������)
            float camera_x = userClickedStage.transform.localPosition.x - userClickedStage.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 + 100f; // r
            float camera_y = -userClickedStage.transform.localPosition.y - userClickedStage.transform.parent.parent.GetComponent<RectTransform>().rect.height / 2;
            userClickedStage.transform.parent.GetComponent<RectTransform>().transform.localPosition = new Vector3(-camera_x, camera_y);

            // ����
            SoundManager.Instance.PlayBGM(SoundManager.BGM_list.StageSelect_BGM);

        }

        public void OnEnableMapSelect() // �������� ���� �� Ȱ��ȭ��
        {

            // �����ư ��Ȱ��ȭ
            returnToWorldBtn.SetActive(false);


            // �� �ܿ� �ٸ��� �� ���ְ� �� �ѱ�
            for (int i = 0; i < this.transform.childCount; i++) this.transform.GetChild(i).gameObject.SetActive(false);
            this.transform.GetChild(0).gameObject.SetActive(true);
            WorldButton_OnClick();

            // ����
            SoundManager.Instance.PlayBGM(SoundManager.BGM_list.StageSelect_BGM);
        }

            private void PlayerPosOnMap()
        {
            // ĵ����(����) ��ġ �÷��̾� �������� ����
            ScrollRect sr = userMarker.transform.parent.parent.parent.parent.GetComponent<ScrollRect>();
            float user_y = -Camera.main.WorldToScreenPoint(userMarker.transform.position).y / userMarker.transform.parent.parent.parent.parent.GetComponent<ScrollRect>().content.rect.height;
            float user_x = userMarker.transform.position.x * 50 / userMarker.transform.parent.parent.parent.parent.GetComponent<ScrollRect>().content.rect.width;
            userMarker.transform.parent.parent.parent.parent.GetComponent<ScrollRect>().horizontalScrollbar.value = user_x;
            userMarker.transform.parent.parent.parent.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 0.5f;
        }

        public void HomeButton_OnClick() // Ȩ ȭ������ ����
        {
            // �÷��̾� ��ũ(������)�̵����̸� ����X
            if (isMarkerMoving == true) return;

            // ����â ���������� ����
            StageInfo.SetActive(false);

            // ������ ������ �������� ũ�� ���󺹱�
            if (userClickedStage != null) userClickedStage.transform.DOScale(1f, 0f);

            // ����
            SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);



            // ��������->Ȩ ���̵� ��ȯ
            Fade_StageToHome();
        }


        public void EpisodeButton_OnClick([SerializeField] int episodeNumber) // ���Ǽҵ� Ŭ����
        {
            // �÷��̾� ��ũ(������)�̵����̸� ����X
            if (isMarkerMoving == true) return;

            // ����â ���������� ����
            StageInfo.SetActive(false);

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

            float loadingTime = 0.5f;
            GameObject FadeImg = GameObject.Find("LoadingBackground");
            for (int i = 0; i < FadeImg.transform.childCount; i++)
            {
                FadeImg.transform.GetChild(i).gameObject.SetActive(true);
                FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(1f, loadingTime).OnComplete(() =>
                {
                    // ��������� ���ư��� ��ư �ѱ�
                    returnToWorldBtn.SetActive(true);

                    // �÷��̾� ��ġ�� �� n-1 �� ����
                    UserDataManager.Instance.setUserData_userCurrentStage(episodeNumber.ToString() + "-1");
                    userMarker.transform.SetParent(this.transform.GetChild(episodeNumber).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1));

                    userMarker.transform.DOLocalMove(Vector2.zero, 0f); // ���� ��ġ��
                    userMarker.GetComponent<Transform>().localScale = new Vector2(80,80); // ���� ������

                    this.transform.GetChild(episodeNumber).gameObject.SetActive(true);
                    this.transform.GetChild(0).gameObject.SetActive(false);
                    Init_StageSelect_By_UserInfo(episodeNumber);


                });
                FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(0f, loadingTime).SetDelay(loadingTime).OnComplete(() =>
                {
                    FadeImg.transform.GetChild(0).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(1).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(2).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(3).gameObject.SetActive(false);
                    FadeImg.transform.GetChild(4).gameObject.SetActive(false);
                });
            }


        }


        public void WorldButton_OnClick() // ����� ���ư��� ��ư Ŭ����
        {
            // �÷��̾� ��ũ(������)�̵����̸� ����X
            if (isMarkerMoving == true) return;

            // ����â ���������� ����
            StageInfo.SetActive(false);

            // ��������� ���ư��� ��ư ����
            returnToWorldBtn.SetActive(false);

            // ������ ���� �������� ��ư �ٽ� �����·�
            if (userClickedStage != null)
                userClickedStage.GetComponent<Transform>().localScale = Vector2.one; // ���� ������

            // ������ ��ġ
            if(curEpiNum!=0) userMarker.transform.DOKill();
            userMarker.transform.localPosition = Vector2.zero;


            GameObject TargetButton = this.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject;
            // ���� ���Ǽҵ� Ŭ���� ���� �˻�
            // ���Ǽҵ� 1 �رݿ��� - �˻���ϰ� �ٷ� interactable = true
            TargetButton.transform.GetChild(0).GetComponent<Button>().interactable = true;
            // ���Ǽҵ� 2 �رݿ���
            if (UserDataManager.Instance.GetUserData_userEpi1Data().Find(x => x.mapName == "1-13").star == 3)
            {
                TargetButton.transform.GetChild(1).GetComponent<Button>().interactable = true;
                TargetButton.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                TargetButton.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                TargetButton.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1f,1f,1f);
                TargetButton.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f);
                TargetButton.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            }
            else TargetButton.transform.GetChild(1).GetComponent<Button>().interactable = false;
            // ���Ǽҵ� 3 �رݿ���
            if (UserDataManager.Instance.GetUserData_userEpi2Data().Find(x => x.mapName == "2-18").star == 3)
            {
                TargetButton.transform.GetChild(2).GetComponent<Button>().interactable = true;
                TargetButton.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                TargetButton.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f);
                TargetButton.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f);
                TargetButton.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
            }
            else TargetButton.transform.GetChild(2).GetComponent<Button>().interactable = false;

            curEpiNum = GetCurrentEpisodeNumber();
            this.transform.GetChild(curEpiNum).gameObject.SetActive(false);
            this.transform.GetChild(0).gameObject.SetActive(true);

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
        }

        public void StageButton_OnClick([SerializeField] int currentStageNumber) // �������� ��ư Ŭ����
        {

            this.currentStageNumber = currentStageNumber;

            // ����â �ݱ�
            StageInfo.SetActive(false);

            // ������ ���� Ŭ���� �������� string ������ ���� �� ���Ǽҵ� ��ȣ üũ
            curEpiNum = GetCurrentEpisodeNumber();

            // �������� stage ��ư �ȿ� �ڽ����� ������ ������ �ۿ� �ִ� �θ�� ��ġ��Ų��
            if (userMarker.transform.parent.name != "Content") userMarker.transform.SetParent(userMarker.transform.parent.parent);

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

            // �÷��̾� ��ũ(������)�̵����̸� �̵�UX ���߰� �ٷ� ��ư���� �̵�
            if (isMarkerMoving == true)
            {
                //UX - Ŭ���� ��ư ũ��
                if (userClickedStage != null)
                {
                    userClickedStage.transform.DOScale(1f, 0.5f);
                }
                userClickedStage = this.transform.GetChild(curEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(currentStageNumber).gameObject;
                userClickedStage.transform.DOScale(1.4f, 0.5f);

                // ������ ����
                userMarker.transform.DOKill();
                jerryAnim.SetBool("isJerryMoving", false);

                // ť �ʱ�ȭ
                userMarkerPositionList = new Queue<GameObject>();

                GameObject _dest = null;

                // ������ ��ġ ����
                switch (curEpiNum)
                {
                    case 1:
                        _dest = Episode1_Content.transform.GetChild(currentStageNumber).gameObject;
                        break;
                    case 2:
                        _dest = Episode2_Content.transform.GetChild(currentStageNumber).gameObject;
                        break;
                }
                userMarker.transform.SetParent(_dest.transform.parent);
                userMarker.transform.DOLocalMove(_dest.transform.localPosition, 0);

                // ������ ������ �������� ����
                userCurStage = curEpiNum.ToString() + "-" + currentStageNumber.ToString();
                UserDataManager.Instance.setUserData_userCurrentStage(userCurStage); // ������ ���������� ������ �������� ��ġ ����

                isMarkerMoving = false;
            }
            else
            {
                //UX - Ŭ���� ��ư ũ��
                if (userClickedStage != null)
                {
                    userClickedStage.transform.DOScale(1f, 0.5f);
                }
                userClickedStage = this.transform.GetChild(curEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(currentStageNumber).gameObject;
                userClickedStage.transform.DOScale(1.4f, 0.5f);
                //UX - ������ ������
                //for(int i=)
                //userMarker.transform.SetParent(userClickedStage.transform.parent);
                //Vector2 originPos = userMarker.transform.localPosition;
                //ratTween.Append(userMarker.transform.DOLocalMove(userClickedStage.transform.localPosition, 2f).From(originPos, false));// ()=> { userMarker.transform.position = originVec; }); // ������ �̵�
                GetButtonObjectToMove(currentStageNumber);
                UserMarkerMove();

            }

            // ������ ������ �������� ����
            userCurStage = curEpiNum.ToString() + "-" + currentStageNumber.ToString();
            UserDataManager.Instance.setUserData_userCurrentStage(userCurStage); // ������ ���������� ������ �������� ��ġ ����


            // Ŭ���� ���������� ī�޶� �̵� (����� sliderView�� ������)
            float camera_x = userClickedStage.transform.localPosition.x - userClickedStage.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2;
            float camera_y = -userClickedStage.transform.localPosition.y - userClickedStage.transform.parent.parent.GetComponent<RectTransform>().rect.height / 2;
            userClickedStage.transform.parent.GetComponent<RectTransform>().transform.localPosition = new Vector3(-camera_x, camera_y);


            // �������� ����â ����
            OpenStageInfoPanel();
        }

        public void BackToEpisode_OnClick()
        {
            if (curEpiNum == 0) { HomeButton_OnClick(); return; } // ���� ���Ǽҵ� ������ Ȩȭ������

            // �÷��̾� ��ũ(������)�̵����̸� ����X
            if (isMarkerMoving == true) return;

            // ����â ���������� ����
            StageInfo.SetActive(false);

            // ����� ���ư��� ��ư Ȱ��ȭ
            returnToWorldBtn.SetActive(true);

            //����ȭ�� �ݰ� �������� ����â����
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(curEpiNum).gameObject.SetActive(true);


            // �÷��̾� ��ġ�� �� n-1 �� ����
            UserDataManager.Instance.setUserData_userCurrentStage(curEpiNum.ToString() + "-1");
            userMarker.transform.SetParent(this.transform.GetChild(curEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1));

            // ������� ���Ǽҵ� ��ȣ
            returnToWorldBtn.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Episode " + curEpiNum;


            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
        }

        private int GetCurrentEpisodeNumber() // ���� �����ִ� ���Ǽҵ� ��ȣ ����
        {
            int num = 0;
            if (this.transform.GetChild(0).gameObject.activeInHierarchy) return 0; // ���� �������¸� 0 ����
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
                        if (it.star != 3) break;// �� 3�� �ƴϸ� �㽺������ ���Ѿ
                        if (it.isClearStage == false) break;

                        // ���� �� +1
                        roadCnt++;
                    }
                    // ���� �������� �� ������ ���� �������� ���� ��ư Ȱ��ȭ
                    if (episode1Datas[episode1Datas.Count - 1].star == 3) targetStage.transform.GetChild(_stageIdx + 1).gameObject.SetActive(true);

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

                        if (it.star != 3) break;// �� 3�� �ƴϸ� �㽺������ ���Ѿ
                        if (it.isClearStage == false) break;

                        // ���� �� +1
                        roadCnt++;
                    }
                    // ���� �������� �� ������ ���� �������� ���� ��ư Ȱ��ȭ
                    if (episode2Datas[episode2Datas.Count - 1].star == 3) targetStage.transform.GetChild(_stageIdx + 1).gameObject.SetActive(true);

                    break;
            }



            //StartCoroutine(targetStage.GetComponent<RoadCreater>().RoadColorChange(roadCnt)); // Ŭ������ �������� ���� ���� ��������� ��ĥ


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
                    Debug.Log(userCurStage);
                    starCnt = epi2.Find(x => x.mapName == userCurStage).star;
                    break;
            }
            for (int i = 0; i < starCnt; i++) { StageInfo.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(true); }

            // ���� �̹���
            StageInfo.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>().sprite=StageDataManager.Instance.ClearBossImg[curEpiNum - 1];

            StageInfo.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = userCurStage; // �������� ��ȣ

            // �����Ÿ�
            int bossDistance = (int)StageDataManager.Instance.GetStageInfo(UserDataManager.Instance.GetUserData_userCurrentStage())["boss_distance"];
            GameObject bossDistanceText = StageInfo.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).gameObject;
            bossDistanceText.GetComponent<TextMeshProUGUI>().text = bossDistance.ToString() + " m";
        }

        public void StageStartButton_OnClick()
        {

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

            // ��Ʈ �������� �÷��� �Ұ����Ҷ�
            if (UserDataManager.Instance.GetUserData_heart() == 0) return;

            Fade_StageToPlay();
        }

        public void StageInfoExitButton_OnClick()
        {
            // Ŭ���� ��ư ũ�� �������
            userClickedStage.transform.localScale = Vector2.one;

            // ����â �ݱ�
            StageInfo.SetActive(false);

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
        }
    }

    #endregion

    #region ���Ǽҵ� ����/���� ��ư
    partial class StageSelectManager
    {

        public void prevButton_OnClick()
        {
            // ���� �̵����̸� ����X
            if (isMarkerMoving == true) return;

            // ����â Ȱ��ȭ���¸� ����
            if (StageInfo.activeSelf == true) StageInfo.SetActive(false);

            // ���� ���µ� ���Ǽҵ� ��ȣ
            int currentEpiNum = GetCurrentEpisodeNumber();
            this.transform.GetChild(currentEpiNum).gameObject.SetActive(false); // ���� ���µȰ� �ݰ�
            this.transform.GetChild(--currentEpiNum).gameObject.SetActive(true); // �� ������ ����

            // �÷��̾� ��ġ�� �� n-1 �� ����
            UserDataManager.Instance.setUserData_userCurrentStage(currentEpiNum.ToString() + "-1");
            userMarker.transform.SetParent(this.transform.GetChild(currentEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0), false);

            // ����Ʈ ��µǴ� ��찡 �־.. ����
            //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle2);
            //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle3);

            userMarker.transform.DOLocalMove(this.transform.GetChild(currentEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).transform.localPosition, 0f); // ���� ��ġ��
            //userMarker.transform.localScale = new Vector3(70f, 70f, 70f); // ���� ����� 1�� �Ǽ� �Ⱥ��̰� �Ǵ� ��찡 �־.. ��
            //userMarker.GetComponent<Transform>().localScale = Vector2.one; // ���� ������

            // ������� UI ���Ǽҵ� ��ȣ
            returnToWorldBtn.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Episode " + currentEpiNum;

            Init_StageSelect_By_UserInfo(currentEpiNum);

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
        }

        public void nextButton_OnClick()
        {
            // ���� �̵����̸� ����X
            if (isMarkerMoving == true) return;

            // ����â Ȱ��ȭ���¸� ����
            if (StageInfo.activeSelf == true) StageInfo.SetActive(false);

            // ���� ���µ� ���Ǽҵ� ��ȣ
            int currentEpiNum = GetCurrentEpisodeNumber();
            this.transform.GetChild(currentEpiNum).gameObject.SetActive(false); // ���� ���µȰ� �ݰ�
            this.transform.GetChild(++currentEpiNum).gameObject.SetActive(true); // �� ������ ����

            // �÷��̾� ��ġ�� �� n-1 �� ����
            UserDataManager.Instance.setUserData_userCurrentStage(currentEpiNum.ToString() + "-1");
            userMarker.transform.SetParent(this.transform.GetChild(currentEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0), false);

            // ����Ʈ ��µǴ� ��찡 �־.. ����
            //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle2);
            //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle3);

            userMarker.transform.DOLocalMove(this.transform.GetChild(currentEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).transform.localPosition, 0f); // ���� ��ġ��
            //userMarker.transform.localScale = new Vector3(70f, 70f, 70f); // ���� ����� 1�� �Ǽ� �Ⱥ��̰� �Ǵ� ��찡 �־.. ��
            //userMarker.GetComponent<Transform>().localScale = Vector2.one; // ���� ������

            Init_StageSelect_By_UserInfo(currentEpiNum);

            // ������� UI ���Ǽҵ� ��ȣ
            returnToWorldBtn.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Episode " + currentEpiNum;

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
        }
    }
    #endregion

    #region �÷��̾� UX ����
    partial class StageSelectManager
    {
        [Header("===== Episode-Stage List ======")]

        [SerializeField] public GameObject Episode1_Content; // ���Ǽҵ� 1 ��ư ����Ʈ��
        [SerializeField] public GameObject Episode2_Content; // ���Ǽҵ� 2 ��ư ����Ʈ��

        Queue<GameObject> userMarkerPositionList = new Queue<GameObject>();
        bool isJerryRight; // ������ �̵�����

        // ���� �������� ��ġ�������� Ŭ���� ������������ GameObject ����Ʈ�� �����ϴ� �Լ�, ������ ��ư�� ����Ʈ ����
        public Queue<GameObject> GetButtonObjectToMove(int _userClickedStage)
        {
            // Ʈ�� �ʱ�ȭ
            ratTween.ForceInit();

            int currentEpisode = GetCurrentEpisodeNumber();

            int userCurrentStageNumber = int.Parse(UserDataManager.Instance.GetUserData_userCurrentStage().Split("-")[1]);

            if (userCurrentStageNumber == _userClickedStage) return userMarkerPositionList;

            // ���� �̵����� ����
            if (userCurrentStageNumber < _userClickedStage) isJerryRight = true;
            else isJerryRight = false;

            switch (currentEpisode)
            {
                case 1:
                    if (userCurrentStageNumber < _userClickedStage)
                        for (int i = userCurrentStageNumber + 1; i <= _userClickedStage; i++)
                            userMarkerPositionList.Enqueue(Episode1_Content.transform.GetChild(i).gameObject);
                    else
                        for (int i = userCurrentStageNumber - 1; i >= _userClickedStage; i--)
                            userMarkerPositionList.Enqueue(Episode1_Content.transform.GetChild(i).gameObject);
                    break;
                case 2:
                    if (userCurrentStageNumber < _userClickedStage)
                        for (int i = userCurrentStageNumber + 1; i <= _userClickedStage; i++)
                            userMarkerPositionList.Enqueue(Episode2_Content.transform.GetChild(i).gameObject);
                    else
                        for (int i = userCurrentStageNumber - 1; i >= _userClickedStage; i--)
                            userMarkerPositionList.Enqueue(Episode2_Content.transform.GetChild(i).gameObject);
                    break;
            }
            return userMarkerPositionList;
        }

        // ������ marker �� ť�� ��� ������Ʈ ��ġ�� �̵�
        private void UserMarkerMove()
        {
            Debug.Log(userMarkerPositionList.Count);
            // Marker�� �� ������ ���
            if (userMarkerPositionList.Count == 0)
            {
                isMarkerMoving = false;
                jerryAnim.SetBool("isJerryMoving", isMarkerMoving);
                // ����Ʈ ����
                //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle3);
                //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle2);
                // �������� ����â ����
                //OpenStageInfoPanel();
                return;
            }



            // ������ �ִϸ��̼�
            isMarkerMoving = true;
            jerryAnim.SetBool("isJerryMoving", isMarkerMoving);
            GameObject _dest = userMarkerPositionList.Dequeue();
            float moveTime = 0.5f;

            // ���� �̵�����
            if (userMarker.gameObject.transform.position.x - _dest.transform.position.x < 0)
            {
                isJerryRight = true;
                userMarker.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                isJerryRight = false;
                userMarker.GetComponent<SpriteRenderer>().flipX = false;
            }
            // �� ���� �̵�
            userMarker.transform.SetParent(_dest.transform.parent);
            Vector2 originPos = userMarker.transform.localPosition;
            ratTween.Append(userMarker.transform.DOLocalMove(_dest.transform.localPosition, moveTime).From(originPos, false).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (_dest != userClickedStage) _dest.transform.DOShakeScale(1f, 0.5f);
                }));// ()=> { userMarker.transform.position = originVec; }); // ������ �̵�

            //// ����Ʈ
            //EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle3).transform.SetParent(userMarker.transform);
            //EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle3).transform.localScale = Vector3.one;
            //EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle3).transform.localPosition = new Vector3(0, 0, -1);
            //if (!isJerryRight) EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle3).GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
            //else EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle3).GetComponent<Transform>().localScale = Vector3.one;
            //EffectManager.Instance.PlayParticle(EffectManager.particle_list.FlyEffectParticle3);
            //// ����Ʈ
            //EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle2).transform.SetParent(userMarker.transform);
            //EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle2).transform.localScale = Vector3.one;
            //EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle2).transform.localPosition = new Vector3(0, 0, -1);
            //if (!isJerryRight) EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle2).GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
            //else EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle2).GetComponent<Transform>().localScale = Vector3.one;
            //EffectManager.Instance.PlayParticle(EffectManager.particle_list.FlyEffectParticle2);

            Invoke("UserMarkerMove", moveTime);
        }
    }
    #endregion
}