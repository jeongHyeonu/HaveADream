using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField] GameObject HomeScene;
    [SerializeField] GameObject StageSelectScene;
    [SerializeField] GameObject PlayScene;
    [SerializeField] GameObject ResultScene;

    private HomeManager hm = null;

    // Start is called before the first frame update
    void Start()
    {
        // 초기에 홈 화면 외에 모두 비활성화
        Scene_Change_Home();

        // 싱글톤 받아오기
        hm = HomeManager.Instance;
    }

    public bool GetIsHomeSceneActive()
    {
        return HomeScene.activeSelf;
    }

    public bool GetIsStageSelectSceneActive()
    {
        return StageSelectScene.activeSelf;
    }

    private void Scene_init()    // 씬 모두 비활성화
    {
        HomeScene.SetActive(false);
        StageSelectScene.SetActive(false);
        PlayScene.SetActive(false);
        ResultScene.SetActive(false);
    }

    public void Scene_Change_Home()    // 홈 씬으로 전환
    {
        Scene_init();
        UIGroupManager.Instance.TopUI_On();
        SkillManager.Instance.UI_Off(); // 스킬 UI OFF
        HomeScene.SetActive(true);
    }
    public void Scene_Change_StageSelect()    // 스테이지 선택 씬으로 전환
    {
        Scene_init();
        UIGroupManager.Instance.TopUI_On();
        StageSelectScene.SetActive(true);
        JHW.StageSelectManager.Instance.OnEnableStageSelect();
    }
    public void Scene_Change_GamePlay()    // 게임플레이 씬으로 전환
    {
        Scene_init();
        UIGroupManager.Instance.TopUI_Off();
        SkillManager.Instance.UI_On(); // 스킬 UI ON
        DistanceManager.Instance.DistanceUI_ON();
        PlayScene.SetActive(true);
        DistanceBarManager.Instance.DistanceBarUI_ON();
    }
    public void Scene_Change_Result()    // 결과화면 씬으로 전환
    {
        Scene_init();
        SkillManager.Instance.UI_Off(); // 스킬 UI OFF
        DistanceManager.Instance.DistanceUI_OFF();
        DistanceBarManager.Instance.DistanceBarUI_OFF();
        ResultScene.SetActive(true);
    }
}
