using TMPro;
using UnityEngine;
using UnityEngine.UI;

partial class SkillManager : Singleton<SkillManager>
{


    [SerializeField] GameObject speedSkill;
    [SerializeField] GameObject invinsibleSkill;
    [SerializeField] GameObject healSkill;
    [SerializeField] GameObject absorbSkill;
    [SerializeField] GameObject shieldSkill;

    [SerializeField] GameObject jewelCntText;
    [SerializeField] GameObject SkillSlider;

    [SerializeField] GameObject Player;

    private const int maxJewelCnt = 5;
    private int jewelCnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        speedSkill.GetComponent<Button>().interactable = false;
        invinsibleSkill.GetComponent<Button>().interactable = false;
        healSkill.GetComponent<Button>().interactable = false;
        absorbSkill.GetComponent<Button>().interactable = false;
        shieldSkill.GetComponent<Button>().interactable = false;


        ChangeSkillUIAddress(); // ��ų ��/�� ��ġ ����
    }

    public void GetBlueJewel()
    {
        if (maxJewelCnt <= jewelCnt) return;


        this.transform.GetChild(0).GetChild(jewelCnt++).GetComponent<Button>().interactable = true;
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }

    public void GetRedJewel()
    {
        jewelCnt = 5;
        for (int i = 0; i < jewelCnt; i++)
        {
            this.transform.GetChild(0).GetChild(i).GetComponent<Button>().interactable = true;
        }
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }

    public void SkillBtn_On() // ������ ��ų ��ư ������ �÷��̾ �����ϴ� �� ���� ���� �Լ�
    {
        // ������ ��ų ��ư�� ���콺 �ø���
        PlayerMove.Instance.setIsSkillBtn(true);
    }

    public void SkillBtn_Off() // ������ ��ų ��ư ������ �÷��̾ �����ϴ� �� ���� ���� �Լ�
    {
        // ������ ��ų ��ư���� ���� ����
        PlayerMove.Instance.setIsSkillBtn(false);
    }


    private void ChangeJewelCntText()
    {
        // �ؽ�Ʈ ����
        jewelCntText.GetComponent<TextMeshProUGUI>().text = "x" + jewelCnt;

        // �����̴� ����
        SkillSlider.GetComponent<Slider>().value = (float)jewelCnt / maxJewelCnt;
    }

    private void SpeedSkill_RollBack()
    {
        PlayerMove.Instance.ChangeSpeed(1f);
    }

    public void SpeedSkill_OnClick()
    {
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        PlayerMove.Instance.ChangeSpeed(2f);
        Invoke("SpeedSkill_RollBack", 3f);
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����

        //����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Skill2);
    }
    private void InvinsibleSkill_RollBack()
    {

        if (PlayerMove.Instance.wingCnt == 0)
        {
            MapMove.Instance.mapSpeed = 6f;
        }
        else if (PlayerMove.Instance.wingCnt == 1)
        {
            MapMove.Instance.mapSpeed = 7.5f;
        }
        else if (PlayerMove.Instance.wingCnt == 2)
        {
            MapMove.Instance.mapSpeed = 9f;
        }
        else if (PlayerMove.Instance.wingCnt == 3)
        {
            MapMove.Instance.mapSpeed = 11.5f;
        }
        else if (PlayerMove.Instance.wingCnt == 4)
        {
            MapMove.Instance.mapSpeed = 13f;
        }

        PlayerMove.Instance.ChangeLayer(Player, 20);
        PlayerMove.Instance.ChangeColor();

    }

    public void InvinsibleSkill_OnClick()
    {
        PlayerMove.Instance.ChangeInvisible();
        Invoke("InvinsibleSkill_RollBack", 1f);
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;

        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }
    public void HealSkill_OnClick()
    {
        PlayerMove.Instance.ChangeHealth();
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }
    private void AbsorbSkill_RollBack()
    {
        PlayerMove.Instance.MagneticField.SetActive(false);
    }
    public void AbsorbSkill_OnClick()
    {
        PlayerMove.Instance.Magnet();
        Invoke("AbsorbSkill_RollBack", 1.0f);
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }

    private void ShieldSkill_RollBack()
    {
        //PlayerMove.Instance.ChangeColor();
    }
    public void ShieldSkill_OnClick()
    {
        PlayerMove.Instance.Shield();
        if (PlayerMove.Instance.isShield == true)
        {
            Invoke("ShieldSkill_RollBack", 0f);
        }
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }

    public void UI_On()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
        speedSkill.GetComponent<Button>().interactable = false;
        invinsibleSkill.GetComponent<Button>().interactable = false;
        healSkill.GetComponent<Button>().interactable = false;
        absorbSkill.GetComponent<Button>().interactable = false;
        shieldSkill.GetComponent<Button>().interactable = false;

        jewelCnt = 0;
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }

    public void UI_Off()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}

# region �ɼ�â
partial class SkillManager
{
    public void ChangeSkillUIAddress(int _dirrection = 1)
    {
        _dirrection = PlayerPrefs.GetInt("isSkillUI_Right", 1);
        switch (_dirrection)
        {
            case 0:
                Vector3 vec = new Vector3(-this.transform.GetChild(0).GetComponent<RectTransform>().rect.width / 1.7f, 0f, 0f);
                this.transform.GetChild(0).transform.localPosition = vec;
                break;
            case 1:
                this.transform.GetChild(0).transform.localPosition = Vector3.zero;
                break;
        }
    }
}
#endregion