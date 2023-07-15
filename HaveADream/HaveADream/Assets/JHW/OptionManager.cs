using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

partial class OptionManager : Singleton<OptionManager>
{
    [SerializeField] GameObject OptionUI;

    [SerializeField] GameObject BGM_OnButton;
    [SerializeField] GameObject BGM_OffButton;
    [SerializeField] GameObject SFX_OnButton;
    [SerializeField] GameObject SFX_OffButton;
    [SerializeField] GameObject SkillLeft_OnButton;
    [SerializeField] GameObject SkillRight_OnButton;

    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SoundSlider;
    [SerializeField] GameObject AlertUI;
    [SerializeField] GameObject SkillUI;
    [SerializeField] GameObject GameMakersUI;
    [SerializeField] GameObject CollectionBackground;

    [SerializeField] GameObject UI1;
    [SerializeField] GameObject UI2;
    [SerializeField] GameObject UI3;

    public float music_volume = 1f;
    public float sound_volume = 1f;
    private bool alert_able = true;
    private bool skillUI_direction_isRight = true;
    private Color buttonOnColor = new Color(1f,1f,1f,1f);
    private Color buttonOffColor = new Color(1f,1f,1f,.2f);

    // 활성화시 색 변경
    private void OnEnable()
    {
        MusicButton_ChangeColor(true);
        SoundButton_ChangeColor(true);
        SkillButtonColorChange(isSkillUI_Right= PlayerPrefs.GetInt("isSkillUI_Right",1));
    }


    public void MusicButton_OnClick([SerializeField] bool _flag)
    {
        if (_flag) music_volume = 1f;
        else music_volume = 0f;

        MusicButton_ChangeColor(_flag);
        MusicSlider.value = music_volume;

        SoundManager.Instance.volume_BGM = music_volume;

        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
    }

    public void SoundButton_OnClick([SerializeField] bool _flag)
    {
        if (_flag) sound_volume = 1f;
        else sound_volume = 0f;

        SoundButton_ChangeColor(_flag);
        SoundSlider.value = sound_volume;

        SoundManager.Instance.volume_SFX = sound_volume;

        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
    }

    // 음악버튼 색변경
    private void MusicButton_ChangeColor(bool _flag)
    {
        // On/Off버튼 색
        if (_flag == true)
        {
            BGM_OnButton.GetComponent<Image>().color = buttonOnColor;
            BGM_OffButton.GetComponent<Image>().color = buttonOffColor;
        }
        else
        {
            BGM_OnButton.GetComponent<Image>().color = buttonOffColor;
            BGM_OffButton.GetComponent<Image>().color = buttonOnColor;
        }
    }

    // 사운드버튼 색변경
    private void SoundButton_ChangeColor(bool _flag)
    {
        // On/Off버튼 색
        if (_flag == true)
        {
            SFX_OnButton.GetComponent<Image>().color = buttonOnColor;
            SFX_OffButton.GetComponent<Image>().color = buttonOffColor;
        }
        else
        {
            SFX_OnButton.GetComponent<Image>().color = buttonOffColor;
            SFX_OffButton.GetComponent<Image>().color = buttonOnColor;
        }

        SoundSlider.value = sound_volume;
    }

    public void MusicSlider_OnChange()
    {
        music_volume = MusicSlider.value;
        if (music_volume == 0) MusicButton_OnClick(false);
        else
        {
            BGM_OnButton.GetComponent<Image>().color = buttonOnColor;
            BGM_OffButton.GetComponent<Image>().color = buttonOffColor;
        }
        SoundManager.Instance.volume_BGM = music_volume;
        SoundManager.Instance.BGM_volumeControl();
    }

    public void SoundSlider_OnChange()
    {
        sound_volume = SoundSlider.value;
        if (sound_volume == 0) SoundButton_OnClick(false);
        else
        {
            SFX_OnButton.GetComponent<Image>().color = buttonOnColor;
            SFX_OffButton.GetComponent<Image>().color = buttonOffColor;
        }
        SoundManager.Instance.volume_SFX = sound_volume;
    }

    public void OptionButton_OnClick()
    {
        OptionUI.SetActive(true);
        CollectionBackground.SetActive(true);

        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
    }

    public void BackgroundClick()
    {
        OptionUI.SetActive(false);
    }

    public void NextButtonOnClick(int _num)
    {
        switch (_num)
        {
            case 1:
                UI1.SetActive(true);
                break;
            case 2:
                UI1.SetActive(false);
                UI2.SetActive(true);
                break;
            case 3:
                UI2.SetActive(false);
                break;
            case 4:
                UI3.SetActive(true);
                UI3.GetComponent<Image>().DOFade(1f, 1f).From(0f);
                UI3.transform.GetChild(0).DOLocalMoveY(600f, 15f).From(-400f).SetEase(Ease.Linear).OnComplete(() => { UI3.SetActive(false); });
                break;
            case 5:
                UI3.SetActive(false);
                UI3.transform.GetChild(0).DOKill();
                break;
        }
    }
}

#region SkillUI
partial class OptionManager
{
    // 유저 왼쪽/오른쪽 설정 버튼
    public int isSkillUI_Right;

    // 옵션 - 스킬 UI 왼쪽변경 버튼
    public void SkillUIButton_Left_OnClick()
    {
        isSkillUI_Right = 0;
        PlayerPrefs.SetInt("isSkillUI_Right", isSkillUI_Right);
        SkillManager.Instance.ChangeSkillUIAddress();
        SkillButtonColorChange(isSkillUI_Right);

        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
    }

    // 옵션 - 스킬 UI 오른쪽변경 버튼
    public void SkillUIButton_Right_OnClick()
    {
        isSkillUI_Right = 1;
        PlayerPrefs.SetInt("isSkillUI_Right", isSkillUI_Right);
        SkillManager.Instance.ChangeSkillUIAddress();
        SkillButtonColorChange(isSkillUI_Right);

        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
    }



    private void SkillButtonColorChange(int isSkillUI_Right = 1)
    {
        if (isSkillUI_Right == 1)
        {
            SkillRight_OnButton.GetComponent<Image>().color = buttonOnColor;
            SkillLeft_OnButton.GetComponent<Image>().color = buttonOffColor;
        }
        else
        {
            SkillLeft_OnButton.GetComponent<Image>().color = buttonOnColor;
            SkillRight_OnButton.GetComponent<Image>().color = buttonOffColor;
        }
    }
}
#endregion