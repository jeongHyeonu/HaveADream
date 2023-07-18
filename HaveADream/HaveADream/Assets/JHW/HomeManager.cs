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
        // �̱��� �޾ƿ���
        sm = SceneManager.Instance;
        // ���� ���
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);

        // ���� �̸�
        string userName = PlayerPrefs.GetString("userName", "����� �̸�");
        userName_input.text = userName;
        userName_profile.text = userName;
    }

    
    public void PlayButton_OnClick()
    {
        // �÷��� ��ư, Ȩ->�������� ���̵� ��ȯ
        Fade_HomeToStage();
    }

    public void MapButton_OnClick()
    {
        // �÷��� ��ư, Ȩ->�������� �� ȭ�� ���̵� ��ȯ
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
    GameObject curAnimal; // ���� Ŭ���� ���� ������Ʈ
    Vector2 tempVec2;
    bool _isClicked = false;
    float UX_Duration = 0.5f;

    public void Click_Animal([SerializeField] GameObject AnimalObject) // ����(���رݽ� "?" ������)Ŭ���� �� ������ Ȯ��
    {
        // �޹�� Ȱ��ȭ���¸� ����X
        if (AnimalsBackground.activeSelf == true) return;

        // 0.5�ʵ� ��� ���� �� �ְ� Ȱ��ȭ
        Invoke("Background_Interactable_true", .5f);

        // ���� Ŭ����
        //curAnimal = AnimalObject; // ���� ������ ���� ������Ʈ
        //curAnimal.transform.SetAsLastSibling(); // ���� ���� ����
        //curAnimal.transform.GetChild(1).gameObject.SetActive(true); // �ؽ�Ʈ Ȱ��ȭ
        //curAnimal.transform.DOScale(3f, UX_Duration); // ���� �̹��� ũ�� ũ��
        //AnimalsBackground.gameObject.SetActive(true); // ���� Ŭ���� �޹��
        //AnimalsBackground.GetComponent<Image>()
        //    .DOFade(0.2f, UX_Duration)
        //    .OnStart( ()=> { AnimalsBackground.GetComponent<Image>().color = new Color(0, 0, 0, 0f); } );

        //// ���� �߾� Ȯ��, �̶� tempVec2�� ������ġ �ӽ����� �� �޹��Ŭ���� ������ҷ� �̵�
        //tempVec2 = curAnimal.transform.position;
        //float height = 2 * Camera.main.orthographicSize;
        //float width = height * Camera.main.aspect;
        //curAnimal.transform.DOLocalMove(new Vector2(width / 2, height / 2), UX_Duration);

        curAnimal = AnimalObject; // ���� ������ ���� ������Ʈ
        curAnimal.transform.GetChild(1).gameObject.SetActive(true); // �ؽ�Ʈ Ȱ��ȭ
        Animals.transform.DOLocalMove(new Vector2(-curAnimal.transform.localPosition.x, -curAnimal.transform.localPosition.y), UX_Duration); // UI canvas �̵�
        AnimalsBackground.gameObject.SetActive(true); // ���� Ŭ���� �޹��
        curAnimal.transform.DOScale(1.4f, UX_Duration); // ���� �̹��� ũ�� ũ��

        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
    }

    public void Click_AnimalBackground() // ���� Ȯ��� �� �޹�� Ŭ����
    {
        // ���� �� ��� �������� ���� �� �־ üũ
        if (_isClicked == true) return;
        _isClicked = true;

        // 0.5�ʵ� ��� ���� �� �ְ� ��Ȱ��ȭ
        Invoke("Background_Interactable_false", .5f);

        // ���� Ŭ�� �� �޹�� Ŭ����
        curAnimal.transform.GetChild(1).gameObject.SetActive(false); // �ؽ�Ʈ ��Ȱ��ȭ
        AnimalsBackground.GetComponent<Image>().DOFade(0f, UX_Duration).OnComplete(() => { AnimalsBackground.gameObject.SetActive(false); } );

        // ���� ��ǥ�� �̵� �� ũ�� �۰�
        //curAnimal.transform.DOMove(tempVec2, UX_Duration);
        Animals.transform.DOLocalMove(Vector2.zero, UX_Duration); // UI canvas �̵�
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

#region Ȩ Ÿ��
partial class HomeManager : Singleton<HomeManager>
{
    [SerializeField] GameObject homeTile;

    private void OnEnable()
    {
        // Ȩȭ�� Ȱ��ȭ�� �ر��� ���� �����ֱ�
        Home_OpenAnials();

        //UX
        homeTile.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 0.05f), 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        homeTile.transform.DOLocalMove(new Vector2(-75f, 75f), 20f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
    }
}
#endregion

# region ���� ����Ʈ
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

    // ���������ʻ���
    [SerializeField] List<GameObject> UserImageList;

    // ������ �������� 
    [SerializeField] TextMeshProUGUI lastStage;

    // ���� �̸�
    [SerializeField] TMP_InputField userName_input;
    [SerializeField] TextMeshProUGUI userName_profile;

    LinkedList<GameObject> animalList = new LinkedList<GameObject>();
    GameObject selectedObj = null;
    int cur_animalIndex = 0;
    int cleared_animalCount = 0;


    // �ر��� ���� ����
    public void Home_OpenAnials()
    {
        cleared_animalCount = 0;

        // Epi 1 ���� �ҷ��ͼ� �˻�
        List<Episode1Data> epi1 = UserDataManager.Instance.GetUserData_userEpi1Data();
        //if (epi1[0].isClearStage == false) return; // ����1-1 Ŭ���� ���ߴٸ� ����X (����1-1 Ŭ���� �� ����)
        Animals.transform.GetChild(1).gameObject.SetActive(true);
        if (epi1[12].isClearStage == true) // ����1-13 (����1 ������������������) Ŭ�����ߴٸ�
        {
            //Animals.transform.GetChild(1).GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f); // �ڹ��� ������ ��Ȱ��ȭ
            Animals.transform.GetChild(++cleared_animalCount).GetComponent<Image>().sprite = StageDataManager.Instance.ClearBossImg[0]; // ���� �̹��� Ȱ��ȭ
            if(Animals.transform.GetChild(cleared_animalCount).GetChild(2).GetChild(0).GetComponent<Image>().sprite == lockedSkill) // ���� ��ų �̹����� ��� �̹����� �Ǿ� �ִٸ� 
                Animals.transform.GetChild(cleared_animalCount).GetChild(2).GetChild(0).GetComponent<Image>().sprite = skillSprite[5]; // ���� ��ų �̹��� ����
            Animals.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // �ؽ�Ʈ ���� ����
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
                    Animals.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<size=10>���Ǽҵ�1�� Ŭ�����ϼ���</size>\n���൵ : " + progressNum + "/" + epi1.Count; // �ؽ�Ʈ ���� ����
                    lastStage.text = "1-" + progressNum;
                    break;
                }
            }
        }

        // Epi 2 ���� �ҷ��ͼ� �˻�
        List<Episode2Data> epi2 = UserDataManager.Instance.GetUserData_userEpi2Data();
        //if (epi2[0].isClearStage == false) return; // ����2-1 Ŭ���� ���ߴٸ� ����X (����2-1 Ŭ���� �� ����)
        Animals.transform.GetChild(2).gameObject.SetActive(true);
        if (epi2[17].isClearStage == true) // ����2-18 (����2 ������������������) Ŭ�����ߴٸ�
        {
            Animals.transform.GetChild(++cleared_animalCount).GetComponent<Image>().sprite = StageDataManager.Instance.ClearBossImg[1]; // ���� �̹��� Ȱ��ȭ
            if (Animals.transform.GetChild(cleared_animalCount).GetChild(2).GetChild(0).GetComponent<Image>().sprite == lockedSkill) // ���� ��ų �̹����� ��� �̹����� �Ǿ� �ִٸ� 
                Animals.transform.GetChild(cleared_animalCount).GetChild(2) .GetChild(0).GetComponent<Image>().sprite = skillSprite[6]; // ���� ��ų
            Animals.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // �ؽ�Ʈ ���� ����
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
                    Animals.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<size=10>���Ǽҵ�2�� Ŭ�����ϼ���</size>\n���൵ : " + progressNum + "/" + epi2.Count; // �ؽ�Ʈ ���� ����
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
            // ��ų�г� ���� ��ġ
            skillPanel.transform.DOLocalMoveY(-80f, .5f).SetEase(Ease.OutFlash);
        }
        NextButton.SetActive(true);

        // ���� ������ ������ ��� ��ų�г� ��Ȱ��ȭ, ���� �����̸� Ȱ��ȭ
        if (cur_animalIndex <= cleared_animalCount) skillPanel.SetActive(true);

        //UX.. �ϵ��ڵ� �˼��մϴ�!!
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

        // ���� ������ ������ ��� ��ų�г� ��Ȱ��ȭ
        if (cur_animalIndex > cleared_animalCount) skillPanel.SetActive(false);

        // ��ų�г� �Ʒ��� ��ġ
        if(cur_animalIndex==1)skillPanel.transform.DOLocalMoveY(-135f, .5f).SetEase(Ease.OutFlash);

        //UX.. �ϵ��ڵ� �˼��մϴ�!!
        AnimalObjects.transform.DOLocalMoveX(-AnimalObjects.transform.GetChild(cur_animalIndex).localPosition.x, .5f).SetEase(Ease.OutExpo);
        if (cur_animalIndex >= 2) { AnimalObjects.transform.GetChild(cur_animalIndex - 2)?.DOScale(0f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex - 2).GetComponent<Image>().DOFade(0f, .5f);  }
        if (cur_animalIndex >= 1) { AnimalObjects.transform.GetChild(cur_animalIndex - 1)?.DOScale(0.6f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex - 1).GetComponent<Image>().DOFade(.5f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex - 1).GetChild(2).GetComponent<Image>().DOFade(.5f, .5f); }
        AnimalObjects.transform.GetChild(cur_animalIndex).DOScale(1f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex).GetComponent<Image>().DOFade(1f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex).GetChild(2).GetComponent<Image>().DOFade(1f, .5f);
        if (cur_animalIndex <= AnimalObjects.transform.childCount - 2) { AnimalObjects.transform.GetChild(cur_animalIndex + 1)?.DOScale(0.6f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex + 1).GetComponent<Image>().DOFade(.5f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex + 1).GetChild(2).GetComponent<Image>().DOFade(.5f, .5f); }
        if (cur_animalIndex <= AnimalObjects.transform.childCount - 3) { AnimalObjects.transform.GetChild(cur_animalIndex + 2)?.DOScale(0f, .5f); AnimalObjects.transform.GetChild(cur_animalIndex + 2).GetComponent<Image>().DOFade(0f, .5f); }
    }


    public void SkillSelect([SerializeField] GameObject _obj)
    {
        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

        // ��� ��ų ������ ���
        if (_obj.transform.GetChild(0).GetComponent<Image>().sprite == lockedSkill) 
                {selectedObj?.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); selectedObj = null; return; }

        if (selectedObj == null) // �ٲ� ��ų ù��° ����
        {
            selectedObj = _obj;
            selectedObj.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        else // �ٲ� ��ų �ι��� ����
        {
            //���� ������ ù��° ��ų�� ������ �ι�° ��ų�� ���� ��ų�� ��� ���� X , 
            Sprite s1 = selectedObj.transform.GetChild(0).GetComponent<Image>().sprite;
            Sprite s2 = _obj.transform.GetChild(0).GetComponent<Image>().sprite;
            if (skillSprite.IndexOf(s1) < 5 && skillSprite.IndexOf(s2) < 5) { _obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); selectedObj?.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); selectedObj = null; return; };
            
            // ��ų �׷� ���� �ִ� ��ų�̸� ���� X
            if (selectedObj.transform.parent==_obj.transform.parent) { _obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); selectedObj?.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); selectedObj = null; return; }

            // ������ ��ų �̹��� ����
            selectedObj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Sprite tempSprite = selectedObj.transform.GetChild(0).GetComponent<Image>().sprite;
            selectedObj.transform.GetChild(0).GetComponent<Image>().sprite = _obj.transform.GetChild(0).GetComponent<Image>().sprite;
            _obj.transform.GetChild(0).GetComponent<Image>().sprite = tempSprite;
            selectedObj = null;

            // ��ų�Ŵ����� �ִ� ��ų �̹��� ����
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


    // ��ų���� ȭ�� �¿���
    public void SkillPanel_OnOff(bool b)
    {
        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

        if (b) SkillSelectUI.SetActive(true);
        else SkillSelectUI.SetActive(false);
    }

    // ���� ������ȭ�� �¿���
    public void UserProfile_OnOff(bool b)
    {
        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

        if (b) UserProfileUI.SetActive(true);
        else UserProfileUI.SetActive(false);
    }
    
    // ������ȭ�� �̹��� ������
    public void UserProfile_ChangeUserImage(bool b)
    {
        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);

        UserProfileUI_obj1.SetActive(b);
        UserProfileUI_obj2.SetActive(!b);


    }

    // �����̹��� Ŭ��
    public void UserProfile_ClickImage(GameObject obj)
    {
        // ����ִ��̹����� ����X
        if (obj.GetComponent<Image>().sprite == lockedSkill) return;
        for(int i=0;i< UserImageList.Count; i++)
        {
            UserImageList[i].GetComponent<Image>().sprite = obj.GetComponent<Image>().sprite;
        }
        UserProfile_ChangeUserImage(true);
    }

    // ����� �̸� ���� 
    public void UserName_Change(TMP_InputField input)
    {
        PlayerPrefs.SetString("userName", input.text);
        userName_input.text = input.text;
        userName_profile.text = input.text;
    }

    // ��ų ? ������ Ŭ���� ��ų ���� ȭ��
    public void SkillInfo_OnOff(bool b)
    {
        SkillSelectUI.transform.GetChild(1).gameObject.SetActive(!b); // �ϵ��ڵ� �˼��մϴ�.. ��ų ����â UI Ȱ��/��Ȱ��ȭ
        skillInfoUI.SetActive(b); // ��ų ����â Ȱ��/��Ȱ��ȭ
    }
}
#endregion