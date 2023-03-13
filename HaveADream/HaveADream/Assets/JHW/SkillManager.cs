using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] GameObject speedSkill;
    [SerializeField] GameObject invinsibleSkill;
    [SerializeField] GameObject healSkill;
    [SerializeField] GameObject absorbSkill;
    [SerializeField] GameObject shieldSkill;

    [SerializeField] GameObject jewelCntText;

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
        jewelCntText.GetComponent<TextMeshProUGUI>().text = jewelCnt + " / " + maxJewelCnt;
    }

    private void SpeedSkill_RollBack()
    {
        PlayerMove.Instance.ChangeSpeed(1f);
    }

    public void SpeedSkill_OnClick()
    {
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        PlayerMove.Instance.ChangeSpeed(5f);
        Invoke("SpeedSkill_RollBack", 3f);
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }
    public void InvinsibleSkill_OnClick()
    {
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }
    public void HealSkill_OnClick()
    {
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }
    public void AbsorbSkill_OnClick()
    {
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }
    public void ShieldSkill_OnClick()
    {
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        ChangeJewelCntText(); // ���� ���� ǥ�� �ؽ�Ʈ ����
    }

    public void UI_On()
    {
        for(int i = 0; i < this.transform.childCount; i++)
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