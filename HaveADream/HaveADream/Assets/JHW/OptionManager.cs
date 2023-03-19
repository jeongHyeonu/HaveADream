using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : Singleton<OptionManager>
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
