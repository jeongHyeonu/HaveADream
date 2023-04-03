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

    public float music_volume = 1f;
    public float sound_volume = 1f;
    private bool alert_able = true;
    private bool skillUI_direction_isRight = true;
    private Color buttonOnColor = new Color(1f,1f,0.6f,1f);
    private Color buttonOffColor = new Color(1f,1f,1f,.5f);

    // Ȱ��ȭ�� �� ����
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
    }

    public void SoundButton_OnClick([SerializeField] bool _flag)
    {
        if (_flag) sound_volume = 1f;
        else sound_volume = 0f;

        SoundButton_ChangeColor(_flag);
        SoundSlider.value = sound_volume;
    }

    // ���ǹ�ư ������
    private void MusicButton_ChangeColor(bool _flag)
    {
        // On/Off��ư ��
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

    // �����ư ������
    private void SoundButton_ChangeColor(bool _flag)
    {
        // On/Off��ư ��
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
    }

    public void OptionButton_OnClick()
    {
        OptionUI.SetActive(true);
        CollectionBackground.SetActive(true);
    }

    public void BackgroundClick()
    {
        OptionUI.SetActive(false);
    }
}

#region SkillUI
partial class OptionManager
{
    // ���� ����/������ ���� ��ư
    public int isSkillUI_Right;

    // �ɼ� - ��ų UI ���ʺ��� ��ư
    public void SkillUIButton_Left_OnClick()
    {
        isSkillUI_Right = 0;
        PlayerPrefs.SetInt("isSkillUI_Right", isSkillUI_Right);
        SkillManager.Instance.ChangeSkillUIAddress();
        SkillButtonColorChange(isSkillUI_Right);
    }

    // �ɼ� - ��ų UI �����ʺ��� ��ư
    public void SkillUIButton_Right_OnClick()
    {
        isSkillUI_Right = 1;
        PlayerPrefs.SetInt("isSkillUI_Right", isSkillUI_Right);
        SkillManager.Instance.ChangeSkillUIAddress();
        SkillButtonColorChange(isSkillUI_Right);
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