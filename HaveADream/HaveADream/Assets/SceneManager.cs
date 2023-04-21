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
        // �ʱ⿡ Ȩ ȭ�� �ܿ� ��� ��Ȱ��ȭ
        Scene_Change_Home();

        // �̱��� �޾ƿ���
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

    private void Scene_init()    // �� ��� ��Ȱ��ȭ
    {
        HomeScene.SetActive(false);
        StageSelectScene.SetActive(false);
        PlayScene.SetActive(false);
        ResultScene.SetActive(false);
    }

    public void Scene_Change_Home()    // Ȩ ������ ��ȯ
    {
        if (PlayScene.activeSelf == true) BackgroundManager.Instance.EraseBackground();// ���� �����÷���->�Ͻ�����->Ȩ ���� ��ȯ�ɶ� ���â ��ȯ ���� �����÷��� ��� �����
        Scene_init();
        UIGroupManager.Instance.TopUI_On();
        SkillManager.Instance.UI_Off(); // ��ų UI OFF
        //DistanceManager.Instance.DistanceUI_OFF(); // �Ÿ� ǥ�� ����
        //DistanceBarManager.Instance.DistanceBarUI_OFF();
        HomeScene.SetActive(true);
    }
    public void Scene_Change_StageSelect()    // �������� ���� ������ ��ȯ
    {
        Scene_init();
        UIGroupManager.Instance.TopUI_On();
        SkillManager.Instance.UI_Off();
        StageSelectScene.SetActive(true);
        JHW.StageSelectManager.Instance.OnEnableStageSelect();
    }
    public void Scene_Change_GamePlay()    // �����÷��� ������ ��ȯ
    {
        Scene_init();
        UIGroupManager.Instance.TopUI_Off();
        SkillManager.Instance.UI_On(); // ��ų UI ON
        //DistanceManager.Instance.DistanceUI_ON();
        PlayScene.SetActive(true);
        DistanceBarManager.Instance.DistanceBarUI_ON();
        JewelManager.Instance.StageInfo_jewel_getData(); // ���� �� �ҷ����� �� ���� ����
        DreamPieceManager.Instance.StageInfo_dreamPiece_getData(); // ������ �� �ҷ����� �� ������ ����
        WingManager.Instance.StageInfo_wing_getData(); // ���� �� �ҷ����� �� ���� ����
    }
    //Result 사용 X
    public void Scene_Change_Result()    // ���ȭ�� ������ ��ȯ
    {
        BackgroundManager.Instance.EraseBackground();// ���â ��ȯ ���� �����÷��� ��� �����
        Scene_init();
        SkillManager.Instance.UI_Off(); // ��ų UI OFF
        DistanceBarManager.Instance.DistanceBarUI_OFF();
        DistanceManager.Instance.DistanceUI_OFF(); // �Ÿ� ǥ�� ����
        ResultScene.SetActive(true);
    }
}
