using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

partial class OptionManager : Singleton<OptionManager>
{
    [SerializeField] GameObject OptionUI;
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

    public void MusicButton_OnClick([SerializeField] bool _flag)
    {
        if (_flag) music_volume = 1f;
        else music_volume = 0f;

        MusicSlider.value = music_volume;
    }

    public void SoundButton_OnClick([SerializeField] bool _flag)
    {
        if (_flag) sound_volume = 1f;
        else sound_volume = 0f;

        SoundSlider.value = sound_volume;
    }

    public void MusicSlider_OnChange()
    {
        music_volume = MusicSlider.value;
    }

    public void SoundSlider_OnChange()
    {
        sound_volume = SoundSlider.value;
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
    // 유저 왼쪽/오른쪽 설정 버튼
    public int isSkillUI_Right;

    // 옵션 - 스킬 UI 왼쪽변경 버튼
    public void SkillUIButton_Left_OnClick()
    {
        isSkillUI_Right = 0;
        PlayerPrefs.SetInt("isSkillUI_Right", isSkillUI_Right);
        SkillManager.Instance.ChangeSkillUIAddress();
    }

    // 옵션 - 스킬 UI 오른쪽변경 버튼
    public void SkillUIButton_Right_OnClick()
    {
        isSkillUI_Right = 1;
        PlayerPrefs.SetInt("isSkillUI_Right", isSkillUI_Right);
        SkillManager.Instance.ChangeSkillUIAddress();
    }

}
#endregion