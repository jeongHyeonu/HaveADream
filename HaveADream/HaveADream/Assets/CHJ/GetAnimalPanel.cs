using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class GetAnimalPanel : Singleton<GetAnimalPanel>
{

    private SceneManager sm = null;

    private void Awake()
    {


    }
    void Start()
    {
        // �̱���
        sm = SceneManager.Instance;

    }
    private void OnEnable()
    {
        SkillManager.Instance.UI_Off();
        /*// �������� ����
        // ������ ������ �������� ����
        string key = UserDataManager.Instance.GetUserData_userCurrentStage();

        curEpiNum = (int)StageDataManager.Instance.GetStageInfo(key)["episode"];
        userCurStage = (int)StageDataManager.Instance.GetStageInfo(key)["stage"];

        if(curEpiNum ==1)
        {
            if(userCurStage ==13)
            {
                resultScreen.SetActive(false);
                getAnimalPanel1.SetActive(true);
            }
        }
        else if (curEpiNum ==2)
        {
            if(userCurStage ==18) 
            {
                resultScreen.SetActive(false);
                getAnimalPanel2.SetActive(true);
            }
        }*/


    }
    private void OnDisable()
    {
        //Player.SetActive(true);
    }

    public void ReturnHomeBtn_OnClick()
    {
        sm.Scene_Change_Home();

        // ����
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            ReturnHomeBtn_OnClick();
        }


    }


    //Ŭ���� ����Ʈ �ֱ�
    private void Effect()
    {
        //GameObject effect = Instantiate(effectPrefab, transform.position, transform.rotation);
        Debug.Log("����");
        //.Play();
    }
}
