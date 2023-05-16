using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class GetAnimalPanel : Singleton<GetAnimalPanel>
{
    [SerializeField] GameObject getAnimalPanel;


    [SerializeField] GameObject character;
    [SerializeField] RectTransform characterUI;
    [SerializeField] RectTransform characterStartPos;
    [SerializeField] RectTransform characterTargetPos;

    [SerializeField] GameObject text;
    [SerializeField] RectTransform textUI;
    [SerializeField] RectTransform textStartPos;
    [SerializeField] RectTransform textTargetPos;

    [SerializeField] GameObject box;

    [SerializeField] GameObject resultScreen;

    [SerializeField] GameObject effectPrefab;

    private SceneManager sm = null;

    private void Awake()
    {
        character.GetComponent<Image>();
        text.GetComponent<Image>();
        box.GetComponent<Image>();
        resultScreen.GetComponent<Image>();

    }
    void Start()
    {
        // 싱글톤
        sm = SceneManager.Instance;

    }
    private void OnEnable()
    {
        SkillManager.Instance.UI_Off();

        /*characterStartPos = characterUI; // 시작 위치는 UI 요소의 현재 위치로 설정
        textStartPos = textUI; // 목표 위치는 시작 위치로 설정*/
        if (characterUI != null)
        {
            MoveCharcter();
        }
        if (textUI != null)
        {
            MoveText();
        }

        //배경 움직임
        resultScreen.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 0.05f), 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        resultScreen.transform.DOLocalMove(new Vector2(0f, 0f), 50f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
        Debug.Log("움직임");
        //Player.SetActive(false);
    }
    private void OnDisable()
    {
        //Player.SetActive(true);
    }

    public void ReturnHomeBtn_OnClick()
    {
        sm.Scene_Change_Home();


        // 사운드
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);
    }

    // Update is called once per frame
    void Update()
    {
        resultScreen.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 0.05f), 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        resultScreen.transform.DOLocalMove(new Vector2(0f, 0f), 50f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);



        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            AnimalButton_OnClick();
            Invoke("ReturnHomeBtn_OnClick", 3f);
        }


    }

    public void AnimalButton_OnClick()
    {
        character.SetActive(true);
        text.SetActive(true);
        box.SetActive(false);
        MoveCharcter();
        MoveText();
        //Effect();
        /* MoveCharcter();
         MoveText();*/
    }

    private void MoveCharcter()
    {

        characterUI.anchoredPosition = new Vector2(characterStartPos.localPosition.x - 150f, characterStartPos.localPosition.y);

    }

    private void MoveText()
    {

        textUI.anchoredPosition = new Vector2(textStartPos.localPosition.x - 350f, textStartPos.localPosition.y);

    }
    //클릭시 이펙트 넣기
    private void Effect()
    {
        GameObject effect = Instantiate(effectPrefab, transform.position, transform.rotation);
        Debug.Log("폭발");
        //.Play();
    }
}
