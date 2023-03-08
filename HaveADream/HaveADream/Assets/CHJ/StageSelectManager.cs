using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    private SceneManager sm = null;
    void Start()
    {
        // ΩÃ±€≈Ê
        sm = SceneManager.Instance;
    }

    [SerializeField]
    public void Stage1Button_OnClick()
    {
        sm.Scene_Change_GamePlay();
    }
}
