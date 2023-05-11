using UnityEngine;

public class GetAnimalPanel : MonoBehaviour
{
    [SerializeField] GameObject getAnimalPanel;
    [SerializeField] GameObject Player;

    private SceneManager sm = null;
    // Start is called before the first frame update
    void Start()
    {
        // ½Ì±ÛÅæ
        sm = SceneManager.Instance;

    }
    private void OnEnable()
    {
        SkillManager.Instance.UI_Off();
        //Player.SetActive(false);
    }
    private void OnDisable()
    {
        //Player.SetActive(true);
    }

    public void ReturnStageBtn_OnClick()
    {
        sm.Scene_Change_StageSelect();


        // »ç¿îµå
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.StageSelect_BGM);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            getAnimalPanel.SetActive(false);
        }

    }
}
