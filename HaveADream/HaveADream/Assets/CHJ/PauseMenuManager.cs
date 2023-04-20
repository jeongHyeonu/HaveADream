using UnityEngine;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    [SerializeField] public GameObject PauseMenuUI;

    private SceneManager sm = null;
    public bool isGamePlaying = false;


    void Start()
    {
        // ½Ì±ÛÅæ
        sm = SceneManager.Instance;

    }

    private void OnDisable()
    {
        PauseMenuUI_OFF();
    }

    public void PauseMenuUI_ON()
    {
        Time.timeScale = 0;
        PauseMenuUI.SetActive(true);
        isGamePlaying = true;
    }
    public void PauseMenuUI_OFF()
    {
        Time.timeScale = 1;
        PauseMenuUI.SetActive(false);
        isGamePlaying = false;
    }

    public void ReturnHomeBtn_OnClick()
    {
        sm.Scene_Change_Home();

        // »ç¿îµå
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);
    }
}
