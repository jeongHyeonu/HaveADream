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

    public bool GetIsGamePlaying()
    {
        if (PlayScene.activeSelf == true) return true;
        else return false;
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
        if(PlayScene.activeSelf==true)BackgroundManager.Instance.EraseBackground();// 만약 게임플레이->일시정지->홈 으로 전환될때 결과창 전환 전에 게임플레이 배경 지우기
        Scene_init();
        UIGroupManager.Instance.TopUI_On();
        SkillManager.Instance.UI_Off(); // 스킬 UI OFF
        DistanceManager.Instance.DistanceUI_OFF(); // 거리 표시 끄기
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
        JewelManager.Instance.StageInfo_jewel_getData(); // 보석 수 불러오기 및 보석 생성
        DreamPieceManager.Instance.StageInfo_dreamPiece_getData(); // 꿈조각 수 불러오기 및 꿈조각 생성
        WingManager.Instance.StageInfo_wing_getData(); // 날개 수 불러오기 및 날개 생성
    }
    public void Scene_Change_Result()    // 결과화면 씬으로 전환
    {
        BackgroundManager.Instance.EraseBackground();// 결과창 전환 전에 게임플레이 배경 지우기
        Scene_init();
        SkillManager.Instance.UI_Off(); // 스킬 UI OFF
        DistanceManager.Instance.DistanceUI_OFF(); // 거리 표시 끄기
        ResultScene.SetActive(true);
    }
}
