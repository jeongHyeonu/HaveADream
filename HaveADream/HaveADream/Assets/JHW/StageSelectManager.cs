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
        public int curEpiNum; // 유저 현재 에피소드 번호
        private string userCurStage; // 유저가 현재 선택한 스테이지
        private GameObject userClickedStage; // 유저가 현재 선택한 스테이지
        Sequence ratTween;

        [SerializeField] private GameObject userMarker = null;
        [SerializeField] private GameObject returnToWorldBtn = null;
        [SerializeField] private GameObject StageInfo = null;

        private bool isMarkerMoving = false; // 쥐제리 이동중인지 검사
        private Animator jerryAnim; // 쥐제리 애니메이션
        int currentStageNumber = 0;

        // Start is called before the first frame update
        void Start()
        {
            // 싱글톤 받아오기
            sm = SceneManager.Instance;
            userClickedStage = null;
            ratTween = DOTween.Sequence();

            // 쥐제리 이동 애니메이션
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
                    // 현재 열려있는 에피소드 지도 닫기
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
                    // 현재 열려있는 에피소드 지도 닫기
                    this.transform.GetChild(curEpiNum).gameObject.SetActive(false);
                    if (isActive==false)
                    {
                        isActive = true;
                        // 정보창 닫기
                        StageInfo.SetActive(false);



                        // 클릭한 버튼 크기 원래대로
                        userClickedStage.transform.localScale = Vector2.one;

                        // 유저 데이터에서 하트 감소(서버 데이터 및 prefab 데이터 둘 다) 감소시킨후 UI 변경
                        PlayFabLogin.Instance.SubtractHeart();
                        UserDataManager.Instance.SetUserData_heart(UserDataManager.Instance.GetUserData_heart() - 1);
                        UIGroupManager.Instance.ChangeHeartUI();


                        // 게임 플레이로 전환
                        sm.Scene_Change_GamePlay();
                        // 맵 생성
                        BackgroundManager.Instance.GenerateBackground(curEpiNum);

                        // 음악 재생
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
        public void OnEnableStageSelect() // 스테이지 선택 씬 활성화시
        {

            // 월드버튼 활성화
            returnToWorldBtn.SetActive(true);



            // 스테이지 선택 창 활성화 시
            // 유저가 마지막으로 눌렀던 스테이지, 현재 스테이지 체크 후 그 위치로 플레이어(쥐제리) 위치시키기
            string userCurrentStage = UserDataManager.Instance.GetUserData_userCurrentStage();
            string[] userCurrentStages = userCurrentStage.Split('-');
            this.transform.GetChild(int.Parse(userCurrentStages[0].ToCharArray())).gameObject.SetActive(true); // 에피소드 맵 활성화
            returnToWorldBtn.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Episode " + int.Parse(userCurrentStages[0].ToCharArray()); // 좌측상단 에피소드 번호
            Transform targetTf = GameObject.Find("Stage" + userCurrentStage).transform;
            userClickedStage = GameObject.Find("Stage" + userCurrentStage);
            userMarker.transform.SetParent(targetTf); // 스테이지 버튼에 쥐제리 위치
            userMarker.transform.localPosition = Vector2.zero;
            //userMarker.GetComponent<Transform>().localScale = Vector2.one; // 사이즈

            Init_StageSelect_By_UserInfo(int.Parse(userCurrentStages[0].ToCharArray())); // 스테이지 정보 불러오기

            // 플레이어 위치로 지도 위치시키기
            //Invoke("PlayerPosOnMap", 0.01f);
            // 클릭한 스테이지로 카메라 이동 (사실은 sliderView가 움직임)
            float camera_x = userClickedStage.transform.localPosition.x - userClickedStage.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 + 100f; // r
            float camera_y = -userClickedStage.transform.localPosition.y - userClickedStage.transform.parent.parent.GetComponent<RectTransform>().rect.height / 2;
            userClickedStage.transform.parent.GetComponent<RectTransform>().transform.localPosition = new Vector3(-camera_x, camera_y);

            // 사운드
            SoundManager.Instance.PlayBGM(SoundManager.BGM_list.StageSelect_BGM);

        }

        public void OnEnableMapSelect() // 스테이지 선택 씬 활성화시
        {

            // 월드버튼 비활성화
            returnToWorldBtn.SetActive(false);


            // 맵 외에 다른거 다 꺼주고 맵 켜기
            for (int i = 0; i < this.transform.childCount; i++) this.transform.GetChild(i).gameObject.SetActive(false);
            this.transform.GetChild(0).gameObject.SetActive(true);
            WorldButton_OnClick();

            // 사운드
            SoundManager.Instance.PlayBGM(SoundManager.BGM_list.StageSelect_BGM);
        }

            private void PlayerPosOnMap()
        {
            // 캔버스(지도) 위치 플레이어 기준으로 설정
            ScrollRect sr = userMarker.transform.parent.parent.parent.parent.GetComponent<ScrollRect>();
            float user_y = -Camera.main.WorldToScreenPoint(userMarker.transform.position).y / userMarker.transform.parent.parent.parent.parent.GetComponent<ScrollRect>().content.rect.height;
            float user_x = userMarker.transform.position.x * 50 / userMarker.transform.parent.parent.parent.parent.GetComponent<ScrollRect>().content.rect.width;
            userMarker.transform.parent.parent.parent.parent.GetComponent<ScrollRect>().horizontalScrollbar.value = user_x;
            userMarker.transform.parent.parent.parent.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 0.5f;
        }

        public void HomeButton_OnClick() // 홈 화면으로 복귀
        {
            // 플레이어 마크(쥐제리)이동중이면 실행X
            if (isMarkerMoving == true) return;

            // 정보창 열려있으면 끄기
            StageInfo.SetActive(false);

            // 유저가 선택한 스테이지 크기 원상복귀
            if (userClickedStage != null) userClickedStage.transform.DOScale(1f, 0f);

            // 사운드
            SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);



            // 스테이지->홈 페이드 전환
            Fade_StageToHome();
        }


        public void EpisodeButton_OnClick([SerializeField] int episodeNumber) // 에피소드 클릭시
        {
            // 플레이어 마크(쥐제리)이동중이면 실행X
            if (isMarkerMoving == true) return;

            // 정보창 열려있으면 끄기
            StageInfo.SetActive(false);

            // 사운드
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

            float loadingTime = 0.5f;
            GameObject FadeImg = GameObject.Find("LoadingBackground");
            for (int i = 0; i < FadeImg.transform.childCount; i++)
            {
                FadeImg.transform.GetChild(i).gameObject.SetActive(true);
                FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(1f, loadingTime).OnComplete(() =>
                {
                    // 월드맵으로 돌아가기 버튼 켜기
                    returnToWorldBtn.SetActive(true);

                    // 플레이어 위치는 걍 n-1 로 고정
                    UserDataManager.Instance.setUserData_userCurrentStage(episodeNumber.ToString() + "-1");
                    userMarker.transform.SetParent(this.transform.GetChild(episodeNumber).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1));

                    userMarker.transform.DOLocalMove(Vector2.zero, 0f); // 원래 위치로
                    userMarker.GetComponent<Transform>().localScale = new Vector2(80,80); // 원래 사이즈

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


        public void WorldButton_OnClick() // 월드로 돌아가기 버튼 클릭시
        {
            // 플레이어 마크(쥐제리)이동중이면 실행X
            if (isMarkerMoving == true) return;

            // 정보창 열려있으면 끄기
            StageInfo.SetActive(false);

            // 월드맵으로 돌아가기 버튼 끄기
            returnToWorldBtn.SetActive(false);

            // 유저가 누른 스테이지 버튼 다시 원상태로
            if (userClickedStage != null)
                userClickedStage.GetComponent<Transform>().localScale = Vector2.one; // 원래 사이즈

            // 쥐제리 위치
            if(curEpiNum!=0) userMarker.transform.DOKill();
            userMarker.transform.localPosition = Vector2.zero;


            GameObject TargetButton = this.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject;
            // 유저 에피소드 클리어 여부 검사
            // 에피소드 1 해금여부 - 검사안하고 바로 interactable = true
            TargetButton.transform.GetChild(0).GetComponent<Button>().interactable = true;
            // 에피소드 2 해금여부
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
            // 에피소드 3 해금여부
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

            // 사운드
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
        }

        public void StageButton_OnClick([SerializeField] int currentStageNumber) // 스테이지 버튼 클릭시
        {

            this.currentStageNumber = currentStageNumber;

            // 정보창 닫기
            StageInfo.SetActive(false);

            // 유저가 현재 클릭한 스테이지 string 값으로 저장 및 에피소드 번호 체크
            curEpiNum = GetCurrentEpisodeNumber();

            // 쥐제리가 stage 버튼 안에 자식으로 있으면 꺼내서 밖에 있는 부모로 위치시킨다
            if (userMarker.transform.parent.name != "Content") userMarker.transform.SetParent(userMarker.transform.parent.parent);

            // 사운드
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

            // 플레이어 마크(쥐제리)이동중이면 이동UX 멈추고 바로 버튼으로 이동
            if (isMarkerMoving == true)
            {
                //UX - 클릭한 버튼 크기
                if (userClickedStage != null)
                {
                    userClickedStage.transform.DOScale(1f, 0.5f);
                }
                userClickedStage = this.transform.GetChild(curEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(currentStageNumber).gameObject;
                userClickedStage.transform.DOScale(1.4f, 0.5f);

                // 움직임 멈춤
                userMarker.transform.DOKill();
                jerryAnim.SetBool("isJerryMoving", false);

                // 큐 초기화
                userMarkerPositionList = new Queue<GameObject>();

                GameObject _dest = null;

                // 쥐제리 위치 조정
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

                // 유저가 선택한 스테이지 저장
                userCurStage = curEpiNum.ToString() + "-" + currentStageNumber.ToString();
                UserDataManager.Instance.setUserData_userCurrentStage(userCurStage); // 유저가 마지막으로 선택한 스테이지 위치 갱신

                isMarkerMoving = false;
            }
            else
            {
                //UX - 클릭한 버튼 크기
                if (userClickedStage != null)
                {
                    userClickedStage.transform.DOScale(1f, 0.5f);
                }
                userClickedStage = this.transform.GetChild(curEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(currentStageNumber).gameObject;
                userClickedStage.transform.DOScale(1.4f, 0.5f);
                //UX - 쥐제리 아이콘
                //for(int i=)
                //userMarker.transform.SetParent(userClickedStage.transform.parent);
                //Vector2 originPos = userMarker.transform.localPosition;
                //ratTween.Append(userMarker.transform.DOLocalMove(userClickedStage.transform.localPosition, 2f).From(originPos, false));// ()=> { userMarker.transform.position = originVec; }); // 쥐제리 이동
                GetButtonObjectToMove(currentStageNumber);
                UserMarkerMove();

            }

            // 유저가 선택한 스테이지 저장
            userCurStage = curEpiNum.ToString() + "-" + currentStageNumber.ToString();
            UserDataManager.Instance.setUserData_userCurrentStage(userCurStage); // 유저가 마지막으로 선택한 스테이지 위치 갱신


            // 클릭한 스테이지로 카메라 이동 (사실은 sliderView가 움직임)
            float camera_x = userClickedStage.transform.localPosition.x - userClickedStage.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2;
            float camera_y = -userClickedStage.transform.localPosition.y - userClickedStage.transform.parent.parent.GetComponent<RectTransform>().rect.height / 2;
            userClickedStage.transform.parent.GetComponent<RectTransform>().transform.localPosition = new Vector3(-camera_x, camera_y);


            // 스테이지 정보창 오픈
            OpenStageInfoPanel();
        }

        public void BackToEpisode_OnClick()
        {
            if (curEpiNum == 0) { HomeButton_OnClick(); return; } // 열린 에피소드 없으면 홈화면으로

            // 플레이어 마크(쥐제리)이동중이면 실행X
            if (isMarkerMoving == true) return;

            // 정보창 열려있으면 끄기
            StageInfo.SetActive(false);

            // 월드로 돌아가기 버튼 활성화
            returnToWorldBtn.SetActive(true);

            //지도화면 닫고 스테이지 선택창으로
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(curEpiNum).gameObject.SetActive(true);


            // 플레이어 위치는 걍 n-1 로 고정
            UserDataManager.Instance.setUserData_userCurrentStage(curEpiNum.ToString() + "-1");
            userMarker.transform.SetParent(this.transform.GetChild(curEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1));

            // 좌측상단 에피소드 번호
            returnToWorldBtn.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Episode " + curEpiNum;


            // 사운드
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
        }

        private int GetCurrentEpisodeNumber() // 현재 열려있는 에피소드 번호 리턴
        {
            int num = 0;
            if (this.transform.GetChild(0).gameObject.activeInHierarchy) return 0; // 지도 열린상태면 0 리턴
            while (true)
            {
                if (this.transform.GetChild(++num).gameObject.activeSelf == true) return num;
            }
        }

        private void Init_StageSelect_By_UserInfo(int _stageNum) // 유저 데이터에 있는대로 스테이지 초기화
        {
            GameObject targetStage = this.transform.GetChild(_stageNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            // 만약 에피소드 번호 (_stageNum) 가 1이면 StageSelect1/Episode1/Canvas/Scroll View/Viewport/Content , (여기서 GetChild(1)시 Stage1-1 스테이지 오브젝트 선택)
            int _stageIdx; // 스테이지 인덱스번호
            int roadCnt = 0; // 도로 수, 나중에 RoadCreater에서 색, UX 변경

            switch (_stageNum)
            {
                case 1:
                    List<Episode1Data> episode1Datas = UserDataManager.Instance.GetUserData_userEpi1Data();
                    _stageIdx = 0;

                    foreach (Episode1Data it in episode1Datas)
                    {
                        _stageIdx++;
                        // 버튼 활성화
                        targetStage.transform.GetChild(_stageIdx).GetComponent<Button>().interactable = true;
                        targetStage.transform.GetChild(_stageIdx).GetChild(0).gameObject.SetActive(true);
                        // 별 개수
                        for (int i = 0; i < it.star; i++)
                        {
                            targetStage.transform.GetChild(_stageIdx).GetChild(2).GetChild(i).GetChild(0).gameObject.SetActive(true);
                        }
                        if (it.star != 3) break;// 별 3개 아니면 담스테이지 못넘어감
                        if (it.isClearStage == false) break;

                        // 도로 수 +1
                        roadCnt++;
                    }
                    // 만약 스테이지 다 깼으면 다음 스테이지 가는 버튼 활성화
                    if (episode1Datas[episode1Datas.Count - 1].star == 3) targetStage.transform.GetChild(_stageIdx + 1).gameObject.SetActive(true);

                    break;

                case 2:
                    List<Episode2Data> episode2Datas = UserDataManager.Instance.GetUserData_userEpi2Data();
                    _stageIdx = 0;

                    foreach (Episode2Data it in episode2Datas)
                    {
                        _stageIdx++;
                        // 버튼 활성화
                        targetStage.transform.GetChild(_stageIdx).GetComponent<Button>().interactable = true;
                        targetStage.transform.GetChild(_stageIdx).GetChild(0).gameObject.SetActive(true);

                        // 별 개수
                        for (int i = 0; i < it.star; i++)
                        {
                            targetStage.transform.GetChild(_stageIdx).GetChild(2).GetChild(i).GetChild(0).gameObject.SetActive(true);
                        }

                        if (it.star != 3) break;// 별 3개 아니면 담스테이지 못넘어감
                        if (it.isClearStage == false) break;

                        // 도로 수 +1
                        roadCnt++;
                    }
                    // 만약 스테이지 다 깼으면 다음 스테이지 가는 버튼 활성화
                    if (episode2Datas[episode2Datas.Count - 1].star == 3) targetStage.transform.GetChild(_stageIdx + 1).gameObject.SetActive(true);

                    break;
            }



            //StartCoroutine(targetStage.GetComponent<RoadCreater>().RoadColorChange(roadCnt)); // 클리어한 스테이지 따라서 도로 노란색으로 색칠


        }
    }

    #region 스테이지 정보창
    partial class StageSelectManager : Singleton<StageSelectManager>
    {
        private void OpenStageInfoPanel()
        {
            // 스테이지 정보창 활성화
            StageInfo.SetActive(true);

            // 유저가 클릭한 에피소드 번호에 따라 별 개수 찾아서 반영
            int starCnt = 0;  // 별 개수
            for (int i = 0; i < 3; i++) { StageInfo.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false); } // 초기 별 이미지 안보이게
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

            // 보스 이미지
            StageInfo.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>().sprite=StageDataManager.Instance.ClearBossImg[curEpiNum - 1];

            StageInfo.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = userCurStage; // 스테이지 번호

            // 보스거리
            int bossDistance = (int)StageDataManager.Instance.GetStageInfo(UserDataManager.Instance.GetUserData_userCurrentStage())["boss_distance"];
            GameObject bossDistanceText = StageInfo.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).gameObject;
            bossDistanceText.GetComponent<TextMeshProUGUI>().text = bossDistance.ToString() + " m";
        }

        public void StageStartButton_OnClick()
        {

            // 사운드
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

            // 하트 부족으로 플레이 불가능할때
            if (UserDataManager.Instance.GetUserData_heart() == 0) return;

            Fade_StageToPlay();
        }

        public void StageInfoExitButton_OnClick()
        {
            // 클릭한 버튼 크기 원래대로
            userClickedStage.transform.localScale = Vector2.one;

            // 정보창 닫기
            StageInfo.SetActive(false);

            // 사운드
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
        }
    }

    #endregion

    #region 에피소드 이전/다음 버튼
    partial class StageSelectManager
    {

        public void prevButton_OnClick()
        {
            // 제리 이동중이면 실행X
            if (isMarkerMoving == true) return;

            // 정보창 활성화상태면 끄기
            if (StageInfo.activeSelf == true) StageInfo.SetActive(false);

            // 현재 오픈된 에피소드 번호
            int currentEpiNum = GetCurrentEpisodeNumber();
            this.transform.GetChild(currentEpiNum).gameObject.SetActive(false); // 현재 오픈된거 닫고
            this.transform.GetChild(--currentEpiNum).gameObject.SetActive(true); // 그 이전꺼 오픈

            // 플레이어 위치는 걍 n-1 로 고정
            UserDataManager.Instance.setUserData_userCurrentStage(currentEpiNum.ToString() + "-1");
            userMarker.transform.SetParent(this.transform.GetChild(currentEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0), false);

            // 이펙트 출력되는 경우가 있어서.. 끄기
            //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle2);
            //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle3);

            userMarker.transform.DOLocalMove(this.transform.GetChild(currentEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).transform.localPosition, 0f); // 원래 위치로
            //userMarker.transform.localScale = new Vector3(70f, 70f, 70f); // 가끔 사이즈가 1이 되서 안보이게 되는 경우가 있어서.. ㅠ
            //userMarker.GetComponent<Transform>().localScale = Vector2.one; // 원래 사이즈

            // 좌측상단 UI 에피소드 번호
            returnToWorldBtn.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Episode " + currentEpiNum;

            Init_StageSelect_By_UserInfo(currentEpiNum);

            // 사운드
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
        }

        public void nextButton_OnClick()
        {
            // 제리 이동중이면 실행X
            if (isMarkerMoving == true) return;

            // 정보창 활성화상태면 끄기
            if (StageInfo.activeSelf == true) StageInfo.SetActive(false);

            // 현재 오픈된 에피소드 번호
            int currentEpiNum = GetCurrentEpisodeNumber();
            this.transform.GetChild(currentEpiNum).gameObject.SetActive(false); // 현재 오픈된거 닫고
            this.transform.GetChild(++currentEpiNum).gameObject.SetActive(true); // 그 다음꺼 오픈

            // 플레이어 위치는 걍 n-1 로 고정
            UserDataManager.Instance.setUserData_userCurrentStage(currentEpiNum.ToString() + "-1");
            userMarker.transform.SetParent(this.transform.GetChild(currentEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0), false);

            // 이펙트 출력되는 경우가 있어서.. 끄기
            //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle2);
            //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle3);

            userMarker.transform.DOLocalMove(this.transform.GetChild(currentEpiNum).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).transform.localPosition, 0f); // 원래 위치로
            //userMarker.transform.localScale = new Vector3(70f, 70f, 70f); // 가끔 사이즈가 1이 되서 안보이게 되는 경우가 있어서.. ㅠ
            //userMarker.GetComponent<Transform>().localScale = Vector2.one; // 원래 사이즈

            Init_StageSelect_By_UserInfo(currentEpiNum);

            // 좌측상단 UI 에피소드 번호
            returnToWorldBtn.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Episode " + currentEpiNum;

            // 사운드
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
        }
    }
    #endregion

    #region 플레이어 UX 관련
    partial class StageSelectManager
    {
        [Header("===== Episode-Stage List ======")]

        [SerializeField] public GameObject Episode1_Content; // 에피소드 1 버튼 리스트들
        [SerializeField] public GameObject Episode2_Content; // 에피소드 2 버튼 리스트들

        Queue<GameObject> userMarkerPositionList = new Queue<GameObject>();
        bool isJerryRight; // 쥐제리 이동방향

        // 현재 스테이지 위치에서부터 클릭한 스테이지까지 GameObject 리스트들 리턴하는 함수, 움직일 버튼들 리스트 리턴
        public Queue<GameObject> GetButtonObjectToMove(int _userClickedStage)
        {
            // 트윈 초기화
            ratTween.ForceInit();

            int currentEpisode = GetCurrentEpisodeNumber();

            int userCurrentStageNumber = int.Parse(UserDataManager.Instance.GetUserData_userCurrentStage().Split("-")[1]);

            if (userCurrentStageNumber == _userClickedStage) return userMarkerPositionList;

            // 제리 이동방향 결정
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

        // 쥐제리 marker 를 큐에 담긴 오브젝트 위치로 이동
        private void UserMarkerMove()
        {
            Debug.Log(userMarkerPositionList.Count);
            // Marker가 다 움직일 경우
            if (userMarkerPositionList.Count == 0)
            {
                isMarkerMoving = false;
                jerryAnim.SetBool("isJerryMoving", isMarkerMoving);
                // 이펙트 제거
                //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle3);
                //EffectManager.Instance.StopParticle(EffectManager.particle_list.FlyEffectParticle2);
                // 스테이지 정보창 오픈
                //OpenStageInfoPanel();
                return;
            }



            // 움직임 애니메이션
            isMarkerMoving = true;
            jerryAnim.SetBool("isJerryMoving", isMarkerMoving);
            GameObject _dest = userMarkerPositionList.Dequeue();
            float moveTime = 0.5f;

            // 제리 이동방향
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
            // 길 따라서 이동
            userMarker.transform.SetParent(_dest.transform.parent);
            Vector2 originPos = userMarker.transform.localPosition;
            ratTween.Append(userMarker.transform.DOLocalMove(_dest.transform.localPosition, moveTime).From(originPos, false).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (_dest != userClickedStage) _dest.transform.DOShakeScale(1f, 0.5f);
                }));// ()=> { userMarker.transform.position = originVec; }); // 쥐제리 이동

            //// 이펙트
            //EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle3).transform.SetParent(userMarker.transform);
            //EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle3).transform.localScale = Vector3.one;
            //EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle3).transform.localPosition = new Vector3(0, 0, -1);
            //if (!isJerryRight) EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle3).GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
            //else EffectManager.Instance.GetParticleObject(EffectManager.particle_list.FlyEffectParticle3).GetComponent<Transform>().localScale = Vector3.one;
            //EffectManager.Instance.PlayParticle(EffectManager.particle_list.FlyEffectParticle3);
            //// 이펙트
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