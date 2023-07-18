using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

partial class HomeManager : Singleton<HomeManager>
{

    private SceneManager sm = null;
    [SerializeField] GameObject Animals = null;
    [SerializeField] GameObject AnimalsBackground = null;
    [SerializeField] GameObject SkillSelectUI;
    [SerializeField] GameObject UserProfileUI;
    [SerializeField] GameObject UserProfileUI_obj1;
    [SerializeField] GameObject UserProfileUI_obj2;

    void Start()
    {
        // 싱글톤 받아오기
        sm = SceneManager.Instance;
        // 음악 재생
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);

        // 유저 이름
        string userName = PlayerPrefs.GetString("userName", "사용자 이름");
        userName_input.text = userName;
        userName_profile.text = userName;
    }

    
    public void PlayButton_OnClick()
    {
        // 플레이 버튼, 홈->스테이지 페이드 전환
        Fade_HomeToStage();
    }

    public void MapButton_OnClick()
    {
        // 플레이 버튼, 홈->스테이지 맵 화면 페이드 전환
        Fade_HomeToMap();
    }

    void Fade_HomeToStage()
    {
        float loadingTime = 0.5f;
        GameObject FadeImg = GameObject.Find("LoadingBackground");
        for (int i = 0; i < FadeImg.transform.childCount; i++)
        {
            FadeImg.transform.GetChild(i).gameObject.SetActive(true);
            FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(1f, loadingTime).OnComplete( () => { 
                if(i==5)
                sm.Scene_Change_StageSelect(); 
            });
            FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(0f, loadingTime).SetDelay(loadingTime).OnComplete(() => {
                FadeImg.transform.GetChild(0).gameObject.SetActive(false);
                FadeImg.transform.GetChild(1).gameObject.SetActive(false);
                FadeImg.transform.GetChild(2).gameObject.SetActive(false);
                FadeImg.transform.GetChild(3).gameObject.SetActive(false);
                FadeImg.transform.GetChild(4).gameObject.SetActive(false);
            });
        }
    }

    void Fade_HomeToMap()
    {
        float loadingTime = 0.5f;
        GameObject FadeImg = GameObject.Find("LoadingBackground");
        for (int i = 0; i < FadeImg.transform.childCount; i++)
        {
            FadeImg.transform.GetChild(i).gameObject.SetActive(true);
            FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(1f, loadingTime).OnComplete(() => {
                if (i == 5)
                    sm.Scene_Change_MapSelect();
            });
            FadeImg.transform.GetChild(i).GetComponent<Image>().DOFade(0f, loadingTime).SetDelay(loadingTime).OnComplete(() => {
                FadeImg.transform.GetChild(0).gameObject.SetActive(false);
                FadeImg.transform.GetChild(1).gameObject.SetActive(false);
                FadeImg.transform.GetChild(2).gameObject.SetActive(false);
                FadeImg.transform.GetChild(3).gameObject.SetActive(false);
                FadeImg.transform.GetChild(4).gameObject.SetActive(false);
            });
        }
    }
}

#region Home - Button
partial class HomeManager : Singleton<HomeManager>
{
    int curAnimalNumber = 0;
    GameObject curAnimal; // 현재 클릭한 동물 오브젝트
    Vector2 tempVec2;
    bool _isClicked = false;
    float UX_Duration = 0.5f;

    public void Click_Animal([SerializeField] GameObject AnimalObject) // 동물(미해금시 "?" 아이콘)클릭시 그 아이콘 확대
    {
        // 뒷배경 활성화상태면 실행X
        if (AnimalsBackground.activeSelf == true) return;

        // 0.5초뒤 배경 누를 수 있게 활성화
        Invoke("Background_Interactable_true", .5f);

        // 동물 클릭시
        //curAnimal = AnimalObject; // 현재 선택한 동물 오브젝트
        //curAnimal.transform.SetAsLastSibling(); // 계층 순서 변경
        //curAnimal.transform.GetChild(1).gameObject.SetActive(true); // 텍스트 활성화
        //curAnimal.transform.DOScale(3f, UX_Duration); // 동물 이미지 크기 크게
        //AnimalsBackground.gameObject.SetActive(true); // 동물 클릭시 뒷배경
        //AnimalsBackground.GetComponent<Image>()
        //    .DOFade(0.2f, UX_Duration)
        //    .OnStart( ()=> { AnimalsBackground.GetComponent<Image>().color = new Color(0, 0, 0, 0f); } );

        //// 동물 중앙 확대, 이때 tempVec2에 동물위치 임시저장 후 뒷배경클릭시 원래장소로 이동
        //tempVec2 = curAnimal.transform.position;
        //float height = 2 * Camera.main.orthographicSize;
        //float width = height * Camera.main.aspect;
        //curAnimal.transform.DOLocalMove(new Vector2(width / 2, height / 2), UX_Duration);

        curAnimal = AnimalObject; // 현재 선택한 동물 오브젝트
        curAnimal.transform.GetChild(1).gameObject.SetActive(true); // 텍스트 활성화
        Animals.transform.DOLocalMove(new Vector2(-curAnimal.transform.localPosition.x, -curAnimal.transform.localPosition.y), UX_Duration); // UI canvas 이동
        AnimalsBackground.gameObject.SetActive(true); // 동물 클릭시 뒷배경
        curAnimal.transform.DOScale(1.4f, UX_Duration); // 동물 이미지 크기 크게

        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
    }

    public void Click_AnimalBackground() // 동물 확대된 후 뒷배경 클릭시
    {
        // 동물 밖 배경 연속으로 누를 수 있어서 체크
        if (_isClicked == true) return;
        _isClicked = true;

        // 0.5초뒤 배경 누를 수 있게 비활성화
        Invoke("Background_Interactable_false", .5f);

        // 동물 클릭 후 뒷배경 클릭시
        curAnimal.transform.GetChild(1).gameObject.SetActive(false); // 텍스트 비활성화
        AnimalsBackground.GetComponent<Image>().DOFade(0f, UX_Duration).OnComplete(() => { AnimalsBackground.gameObject.SetActive(false); } );

        // 원래 좌표로 이동 및 크기 작게
        //curAnimal.transform.DOMove(tempVec2, UX_Duration);
        Animals.transform.DOLocalMove(Vector2.zero, UX_Duration); // UI canvas 이동
        curAnimal.transform.DOScale(1f, UX_Duration);
    }

    private void Background_Interactable_true()
    {
        AnimalsBackground.GetComponent<Button>().interactable = true;
    }

    private void Background_Interactable_false()
    {
        AnimalsBackground.GetComponent<Button>().interactable = false;
        _isClicked = false;
    }
}
#endregion

#region 홈 타일
partial class HomeManager : Singleton<HomeManager>
{
    [SerializeField] GameObject homeTile;

    private void OnEnable()
    {
        // 홈화면 활성화시 해금한 동물 보여주기
        Home_OpenAnials();

        //UX
        homeTile.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 0.05f), 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        homeTile.transform.DOLocalMove(new Vector2(-75f, 75f), 20f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
    }
}
#endregion

# region 동물 리스트
partial class HomeManager
{
    [Header("=== Home - AnimalList ===")]

    [SerializeField] GameObject AnimalObjects;
    [SerializeField] GameObject PrevButton;
    [SerializeField] GameObject NextButton;
    [SerializeField] GameObject skillPanel;
    [SerializeField] GameObject skillInfoUI;
    [SerializeField] Sprite lockedAnimal;
    [SerializeField] Sprite lockedSkill;

    [SerializeField] List<Sprite> skillSprite;
    [SerializeField] List<GameObject> skillObjectList;
    [SerializeField] GameObject lionProfileImage;
    [SerializeField] GameObject monkeyProfileImage;

    // 유저프로필사진
    [SerializeField] List<GameObject> UserImageList;

    // 마지막 스테이지 
    [SerializeField] TextMeshProUGUI lastStage;

    // 유저 이름
    [SerializeField] TMP_InputField userName_input;
    [SerializeField] TextMeshProUGUI userName_profile;

    LinkedList<GameObject> animalList = new LinkedList<GameObject>();
    GameObject selectedObj = null;
    int cur_animalIndex = 0;
    int cleared_animalCount = 0;


    // 해금한 동물 오픈
    public void Home_OpenAnials()
    {
        cleared_animalCount = 0;

        // Epi 1 정보 불러와서 검사
        List<Episode1Data> epi1 = UserDataManager.Instance.GetUserData_userEpi1Data();
        //if (epi1[0].isClearStage == false) return; // 에피1-1 클리어 안했다면 실행X (에피1-1 클리어 시 오픈)
        Animals.transform.GetChild(1).gameObject.SetActive(true);
        if (epi1[12].isClearStage == true) // 에피1-13 (에피1 마지막스테이지까지) 클리어했다면
        {
            //Animals.transform.GetChild(1).GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f); // 자물쇠 아이콘 비활성화
            Animals.transform.GetChild(++cleared_animalCount).GetComponent<Image>().sprite = StageDataManager.Instance.ClearBossImg[0]; // 동물 이미지 활성화
            if(Animals.transform.GetChild(cleared_animalCount).GetChild(2).GetChild(0).GetComponent<Image>().sprite == lockedSkill) // 동물 스킬 이미지가 잠긴 이미지로 되어 있다면 
                Animals.transform.GetChild(cleared_animalCount).GetChild(2).GetChild(0).GetComponent<Image>().sprite = skillSprite[5]; // 동물 스킬 이미지 변경
            Animals.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // 텍스트 내용 변경
            monkeyProfileImage.GetComponent<Image>().sprite = StageDataManager.Instance.ClearBossImg[0];
            lastStage.text = "1-13";
        }
        else
        {
            int progressNum = 0;
            for (int i = 0; i < epi1.Count; i++)
            {
                if (epi1[i].isClearStage) progressNum++;
                else
                {
                    AnimalObjects.transform.GetChild(1).GetComponent<Image>().sprite = lockedAnimal;
                    Animals.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<size=10>에피소드1을 클리어하세요</size>\n진행도 : " + progressNum + "/" + epi1.Count; // 텍스트 내용 변경
                    lastStage.text = "1-" + progressNum;
                    break;
                }
            }
        }

        // Epi 2 정보 불러와서 검사
        List<Episode2Data> epi2 = UserDataManager.Instance.GetUserData_userEpi2Data();
        //if (epi2[0].isClearStage == false) return; // 에피2-1 클리어 안했다면 실행X (에피2-1 클리어 시 오픈)
        Animals.transform.GetChild(2).gameObject.SetActive(true);
        if (epi2[17].isClearStage == true) // 에피2-18 (에피2 마지막스테이지까지) 클리어했다면
        {
            Animals.transform.GetChild(++cleared_animalCount).GetComponent<Image>().sprite = StageDataManager.Instance.ClearBossImg[1]; // 동물 이미지 활성화
            if (Animals.transform.GetChild(cleared_animalCount).GetChild(2).GetChild(0).GetComponent<Image>().sprite == lockedSkill) // 동물 스킬 이미지가 잠긴 이미지로 되어 있다면 
                Animals.transform.GetChild(cleared_animalCount).GetChild(2) .GetChild(0).GetComponent<Image>().sprite = skillSprite[6]; // 동물 스킬
            Animals.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // 텍스트 내용 변경
            lionProfileImage.GetComponent<Image>().sprite = StageDataManager.Instance.ClearBossImg[1];
            lastStage.text = "2-18";
        }
        else
        {
            int progressNum = 0;
            for (int i = 0; i < epi2.Count; i++)
            {
                if (epi2[i].isClearStage) progressNum++;
                else
                {
                    AnimalObjects.transform.GetChild(2).GetComponent<Image>().sprite = lockedAnimal;
                    Animals.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<size=10>에피소드2를 클리어하세요</size>\n진행도 : " + progressNum + "/" + epi2.Count; // 텍스트 내용 변경
                    lastStage.text = "2-" + progressNum;
                    break;
                }
            }
        }

        AnimalObjects.transform.GetChild(3).GetComponent<Image>().sprite = lockedAnimal;
        AnimalObjects.transform.GetChild(4).GetComponent<Image>().sprite = lockedAnimal;
    }

    public void PrevAnimal()
    {
        cur_animalIndex--;
        if (cur_animalIndex <= 0) {
            PrevButton.SetActive(false);
            // 스킬패널 위로 배치
            skillPanel.transform.DOLocalMoveY(-80f, .5f).SetEase(Ease.OutFlash);
        }
        NextButton.SetActive(true);

        // 아직 못얻은 동물일 경우 스킬패널 비활성화, 얻은 동물이면 활성화
        if (cur_animalIndex <= cleared_animalCount) skillPanel.SetActive(true);

        //UX.. 하드코딩 죄송합니다!!
        AnimalObjects.transform.DOLocalMoveX(-AnimalObjects.transform.GetChild(cur_animalIndex).localPosition.x, .5f).SetEase(Ease.OutExpo);
        if (cur_animalIndex >= 2) { AnimalObjects.transform.GetChild(cur_animalIndex - 2)?.DOScale(0f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex - 2).GetComponent<Image>().DOFade(0f, .5f); }
        if (cur_animalIndex >= 1) { AnimalObjects.transform.GetChild(cur_animalIndex - 1)?.DOScale(0.6f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex - 1).GetComponent<Image>().DOFade(.5f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex - 1).GetChild(2).GetComponent<Image>().DOFade(.5f, .5f); }
        AnimalObjects.transform.GetChild(cur_animalIndex).DOScale(1f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex).GetComponent<Image>().DOFade(1f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex).GetChild(2).GetComponent<Image>().DOFade(1f, .5f);
        if (cur_animalIndex <= AnimalObjects.transform.childCount - 2) { AnimalObjects.transform.GetChild(cur_animalIndex + 1)?.DOScale(0.6f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex + 1).GetComponent<Image>().DOFade(.5f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex + 1).GetChild(2).GetComponent<Image>().DOFade(.5f, .5f); }
        if (cur_animalIndex <= AnimalObjects.transform.childCount - 3) { AnimalObjects.transform.GetChild(cur_animalIndex + 2)?.DOScale(0f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex + 2).GetComponent<Image>().DOFade(0f, .5f); } 
    }

    public void NextAnimal()
    {
        cur_animalIndex++;
        if(cur_animalIndex >= AnimalObjects.transform.childCount-1) NextButton.SetActive(false);
        PrevButton.SetActive(true);

        // 아직 못얻은 동물일 경우 스킬패널 비활성화
        if (cur_animalIndex > cleared_animalCount) skillPanel.SetActive(false);

        // 스킬패널 아래로 배치
        if(cur_animalIndex==1)skillPanel.transform.DOLocalMoveY(-135f, .5f).SetEase(Ease.OutFlash);

        //UX.. 하드코딩 죄송합니다!!
        AnimalObjects.transform.DOLocalMoveX(-AnimalObjects.transform.GetChild(cur_animalIndex).localPosition.x, .5f).SetEase(Ease.OutExpo);
        if (cur_animalIndex >= 2) { AnimalObjects.transform.GetChild(cur_animalIndex - 2)?.DOScale(0f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex - 2).GetComponent<Image>().DOFade(0f, .5f);  }
        if (cur_animalIndex >= 1) { AnimalObjects.transform.GetChild(cur_animalIndex - 1)?.DOScale(0.6f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex - 1).GetComponent<Image>().DOFade(.5f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex - 1).GetChild(2).GetComponent<Image>().DOFade(.5f, .5f); }
        AnimalObjects.transform.GetChild(cur_animalIndex).DOScale(1f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex).GetComponent<Image>().DOFade(1f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex).GetChild(2).GetComponent<Image>().DOFade(1f, .5f);
        if (cur_animalIndex <= AnimalObjects.transform.childCount - 2) { AnimalObjects.transform.GetChild(cur_animalIndex + 1)?.DOScale(0.6f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex + 1).GetComponent<Image>().DOFade(.5f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex + 1).GetChild(2).GetComponent<Image>().DOFade(.5f, .5f); }
        if (cur_animalIndex <= AnimalObjects.transform.childCount - 3) { AnimalObjects.transform.GetChild(cur_animalIndex + 2)?.DOScale(0f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex + 2).GetComponent<Image>().DOFade(0f, .5f); }
    }


    public void SkillSelect([SerializeField] GameObject _obj)
    {
        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

        // 잠긴 스킬 선택할 경우
        if (_obj.transform.GetChild(0).GetComponent<Image>().sprite == lockedSkill) 
                {selectedObj?.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); selectedObj = null; return; }

        if (selectedObj == null) // 바꿀 스킬 첫번째 선택
        {
            selectedObj = _obj;
            selectedObj.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        else // 바꿀 스킬 두번재 선택
        {
            //만약 선택한 첫번째 스킬과 선택한 두번째 스킬이 기존 스킬일 경우 실행 X , 
            Sprite s1 = selectedObj.transform.GetChild(0).GetComponent<Image>().sprite;
            Sprite s2 = _obj.transform.GetChild(0).GetComponent<Image>().sprite;
            if (skillSprite.IndexOf(s1) < 5 && skillSprite.IndexOf(s2) < 5) { _obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); selectedObj?.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); selectedObj = null; return; };
            
            // 스킬 그룹 내에 있는 스킬이면 실행 X
            if (selectedObj.transform.parent==_obj.transform.parent) { _obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); selectedObj?.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); selectedObj = null; return; }

            // 선택한 스킬 이미지 변경
            selectedObj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Sprite tempSprite = selectedObj.transform.GetChild(0).GetComponent<Image>().sprite;
            selectedObj.transform.GetChild(0).GetComponent<Image>().sprite = _obj.transform.GetChild(0).GetComponent<Image>().sprite;
            _obj.transform.GetChild(0).GetComponent<Image>().sprite = tempSprite;
            selectedObj = null;

            // 스킬매니저에 있는 스킬 이미지 변경
            for(int i = 0; i < 5; i++)
            {
                SkillManager.Instance.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = skillPanel.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
                if (cur_animalIndex == 1)
                {
                    SkillManager.Instance.ChangeDecreaseSpeed = true;
                }
                else if (cur_animalIndex == 2)
                {
                    SkillManager.Instance.ChangeGetJeweltoDreamPiece = true;
                }
            }

        }
    }


    // 스킬선택 화면 온오프
    public void SkillPanel_OnOff(bool b)
    {
        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

        if (b) SkillSelectUI.SetActive(true);
        else SkillSelectUI.SetActive(false);
    }

    // 유저 프로필화면 온오프
    public void UserProfile_OnOff(bool b)
    {
        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

        if (b) UserProfileUI.SetActive(true);
        else UserProfileUI.SetActive(false);
    }
    
    // 프로필화면 이미지 누르면
    public void UserProfile_ChangeUserImage(bool b)
    {
        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

        UserProfileUI_obj1.SetActive(b);
        UserProfileUI_obj2.SetActive(!b);


    }

    // 동물이미지 클릭
    public void UserProfile_ClickImage(GameObject obj)
    {
        // 잠겨있는이미지면 실행X
        if (obj.GetComponent<Image>().sprite == lockedSkill) return;
        for(int i=0;i< UserImageList.Count; i++)
        {
            UserImageList[i].GetComponent<Image>().sprite = obj.GetComponent<Image>().sprite;
        }
        UserProfile_ChangeUserImage(true);
    }

    // 사용자 이름 변경 
    public void UserName_Change(TMP_InputField input)
    {
        PlayerPrefs.SetString("userName", input.text);
        userName_input.text = input.text;
        userName_profile.text = input.text;
    }

    // 스킬 ? 아이콘 클릭시 스킬 정보 화면
    public void SkillInfo_OnOff(bool b)
    {
        SkillSelectUI.transform.GetChild(1).gameObject.SetActive(!b); // 하드코딩 죄송합니다.. 스킬 선택창 UI 활성/비활성화
        skillInfoUI.SetActive(b); // 스킬 설명창 활성/비활성화
    }
}
#endregion