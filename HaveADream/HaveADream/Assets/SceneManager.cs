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

    private void Scene_init()    // �� ��� ��Ȱ��ȭ
    {
        HomeScene.SetActive(false);
        StageSelectScene.SetActive(false);
        PlayScene.SetActive(false);
        ResultScene.SetActive(false);
    }

    public void Scene_Change_Home()    // Ȩ ������ ��ȯ
    {
        Scene_init();
        UIGroupManager.Instance.TopUI_On();
        SkillManager.Instance.UI_Off(); // ��ų UI OFF
        HomeScene.SetActive(true);
    }
    public void Scene_Change_StageSelect()    // �������� ���� ������ ��ȯ
    {
        Scene_init();
        UIGroupManager.Instance.TopUI_On();
        StageSelectScene.SetActive(true);
        JHW.StageSelectManager.Instance.OnEnableStageSelect();
    }
    public void Scene_Change_GamePlay()    // �����÷��� ������ ��ȯ
    {
        Scene_init();
        UIGroupManager.Instance.TopUI_Off();
        SkillManager.Instance.UI_On(); // ��ų UI ON
        DistanceManager.Instance.DistanceUI_ON();
        PlayScene.SetActive(true);
        DistanceBarManager.Instance.DistanceBarUI_ON();
    }
    public void Scene_Change_Result()    // ���ȭ�� ������ ��ȯ
    {
        Scene_init();
        SkillManager.Instance.UI_Off(); // ��ų UI OFF
        DistanceManager.Instance.DistanceUI_OFF();
        DistanceBarManager.Instance.DistanceBarUI_OFF();
        ResultScene.SetActive(true);
    }
}
