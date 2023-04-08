using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

partial class HomeManager : Singleton<HomeManager>
{

    private SceneManager sm = null;
    [SerializeField] GameObject Animals = null;
    [SerializeField] GameObject AnimalsBackground = null;

    void Start()
    {
        // �̱��� �޾ƿ���
        sm = SceneManager.Instance;
        // ���� ���
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);
    }

    public void PlayButton_OnClick()
    {
        sm.Scene_Change_StageSelect();
    }




    public void Home_OpenAnials()
    {
        // �ر��� ���� ����

        // Epi 1 ���� �ҷ��ͼ� �˻�
        List<Episode1Data> epi1 = UserDataManager.Instance.GetUserData_userEpi1Data();
        if (epi1[0].isClearStage == false) return; // ����1-1 Ŭ���� ���ߴٸ� ����X (����1-1 Ŭ���� �� ����)
        Animals.transform.GetChild(1).gameObject.SetActive(true);
        if (epi1[12].isClearStage == true) // ����1-13 (����1 ������������������) Ŭ�����ߴٸ�
        {
            Animals.transform.GetChild(1).GetChild(0).gameObject.SetActive(true); // ���� �̹��� Ȱ��ȭ
            Animals.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "����"; // �ؽ�Ʈ ���� ����
        }
        else
        {
            int progressNum = 1;
            for (int i = 1; i < epi1.Count; i++)
            {
                if (epi1[i].isClearStage) progressNum++;
                else
                {
                    Animals.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<size=10>�������� 1 Ŭ���� �� �ر�</size>\n���൵ : " + progressNum + "/" + epi1.Count; // �ؽ�Ʈ ���� ����
                    return;
                }
            }
        }

        // Epi 2 ���� �ҷ��ͼ� �˻�
        List<Episode2Data> epi2 = UserDataManager.Instance.GetUserData_userEpi2Data();
        if (epi2[0].isClearStage == false) return; // ����2-1 Ŭ���� ���ߴٸ� ����X (����2-1 Ŭ���� �� ����)
        Animals.transform.GetChild(2).gameObject.SetActive(true);
        if (epi2[17].isClearStage == true) // ����2-18 (����2 ������������������) Ŭ�����ߴٸ�
        {
            Animals.transform.GetChild(2).GetChild(0).gameObject.SetActive(true); // ���� �̹��� Ȱ��ȭ
            Animals.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "����"; // �ؽ�Ʈ ���� ����
        }
        else
        {
            int progressNum = 1;
            for (int i = 1; i < epi2.Count; i++)
            {
                if (epi2[i].isClearStage) progressNum++;
                else
                {
                    Animals.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<size=10>�������� 2 Ŭ���� �� �ر�</size>\n���൵ : " + progressNum + "/" + epi2.Count; // �ؽ�Ʈ ���� ����
                    return;
                }
            }
        }
    }
}

#region Home - Button
partial class HomeManager : Singleton<HomeManager>
{
    int curAnimalNumber = 0;
    GameObject curAnimal; // ���� Ŭ���� ���� ������Ʈ
    Vector2 tempVec2;
    bool _isClicked = false;
    float UX_Duration = 0.5f;

    public void Click_Animal([SerializeField] GameObject AnimalObject) // ����(���رݽ� "?" ������)Ŭ���� �� ������ Ȯ��
    {
        // �޹�� Ȱ��ȭ���¸� ����X
        if (AnimalsBackground.activeSelf == true) return;

        // 0.5�ʵ� ��� ���� �� �ְ� Ȱ��ȭ
        Invoke("Background_Interactable_true", .5f);

        // ���� Ŭ����
        //curAnimal = AnimalObject; // ���� ������ ���� ������Ʈ
        //curAnimal.transform.SetAsLastSibling(); // ���� ���� ����
        //curAnimal.transform.GetChild(1).gameObject.SetActive(true); // �ؽ�Ʈ Ȱ��ȭ
        //curAnimal.transform.DOScale(3f, UX_Duration); // ���� �̹��� ũ�� ũ��
        //AnimalsBackground.gameObject.SetActive(true); // ���� Ŭ���� �޹��
        //AnimalsBackground.GetComponent<Image>()
        //    .DOFade(0.2f, UX_Duration)
        //    .OnStart( ()=> { AnimalsBackground.GetComponent<Image>().color = new Color(0, 0, 0, 0f); } );

        //// ���� �߾� Ȯ��, �̶� tempVec2�� ������ġ �ӽ����� �� �޹��Ŭ���� ������ҷ� �̵�
        //tempVec2 = curAnimal.transform.position;
        //float height = 2 * Camera.main.orthographicSize;
        //float width = height * Camera.main.aspect;
        //curAnimal.transform.DOLocalMove(new Vector2(width / 2, height / 2), UX_Duration);

        curAnimal = AnimalObject; // ���� ������ ���� ������Ʈ
        curAnimal.transform.GetChild(1).gameObject.SetActive(true); // �ؽ�Ʈ Ȱ��ȭ
        Animals.transform.DOLocalMove(new Vector2(-curAnimal.transform.localPosition.x, -curAnimal.transform.localPosition.y), UX_Duration); // UI canvas �̵�
        AnimalsBackground.gameObject.SetActive(true); // ���� Ŭ���� �޹��
        curAnimal.transform.DOScale(1.4f, UX_Duration); // ���� �̹��� ũ�� ũ��

        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
    }

    public void Click_AnimalBackground() // ���� Ȯ��� �� �޹�� Ŭ����
    {
        // ���� �� ��� �������� ���� �� �־ üũ
        if (_isClicked == true) return;
        _isClicked = true;

        // 0.5�ʵ� ��� ���� �� �ְ� ��Ȱ��ȭ
        Invoke("Background_Interactable_false", .5f);

        // ���� Ŭ�� �� �޹�� Ŭ����
        curAnimal.transform.GetChild(1).gameObject.SetActive(false); // �ؽ�Ʈ ��Ȱ��ȭ
        AnimalsBackground.GetComponent<Image>().DOFade(0f, UX_Duration).OnComplete(() => { AnimalsBackground.gameObject.SetActive(false); } );

        // ���� ��ǥ�� �̵� �� ũ�� �۰�
        //curAnimal.transform.DOMove(tempVec2, UX_Duration);
        Animals.transform.DOLocalMove(Vector2.zero, UX_Duration); // UI canvas �̵�
        curAnimal.transform.DOScale(1f, UX_Duration);
    }

    private void Background_Interactable_true()
    {
        AnimalsBackground.GetComponent<Button>().interactable = true;
    }

    private void Background_Interactable_false()
    {
        AnimalsBackground.GetComponent<Button>().interactable = false;
        _isClicked = false;
    }
}
#endregion