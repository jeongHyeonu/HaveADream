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
        // �̱���
        sm = SceneManager.Instance;

    }
    private void OnEnable()
    {
        SkillManager.Instance.UI_Off();

        /*characterStartPos = characterUI; // ���� ��ġ�� UI ����� ���� ��ġ�� ����
        textStartPos = textUI; // ��ǥ ��ġ�� ���� ��ġ�� ����*/
        if (characterUI != null)
        {
            MoveCharcter();
        }
        if (textUI != null)
        {
            MoveText();
        }

        //��� ������
        resultScreen.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 0.05f), 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        resultScreen.transform.DOLocalMove(new Vector2(0f, 0f), 50f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
        Debug.Log("������");
        //Player.SetActive(false);
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
    //Ŭ���� ����Ʈ �ֱ�
    private void Effect()
    {
        GameObject effect = Instantiate(effectPrefab, transform.position, transform.rotation);
        Debug.Log("����");
        //.Play();
    }
}
