using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    private SceneManager sm = null;
    void Start()
    {
        // �̱���
        sm = SceneManager.Instance;
    }
    public void ReturnHomeBtn_OnClick()
    {
        sm.Scene_Change_Home();
    }
}
