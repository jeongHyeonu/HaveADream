using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    private SceneManager sm = null;
    void Start()
    {
        // �̱���
        sm = SceneManager.Instance;
    }

    public void Stage1Button_OnClick()
    {
        sm.Scene_Change_GamePlay();
    }
}
