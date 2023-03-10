using System.Collections;
using System.Collections.Generic;
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
    }
    public void Scene_Change_GamePlay()    // �����÷��� ������ ��ȯ
    {
        Scene_init();
        UIGroupManager.Instance.TopUI_Off();
        SkillManager.Instance.UI_On(); // ��ų UI ON
        PlayScene.SetActive(true);
    }
    public void Scene_Change_Result()    // ���ȭ�� ������ ��ȯ
    {
        Scene_init();
        SkillManager.Instance.UI_Off(); // ��ų UI OFF
        ResultScene.SetActive(true);
    }
}
