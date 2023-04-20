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


        ChangeSkillUIAddress(); // 스킬 좌/우 위치 변경
    }

    public void GetBlueJewel()
    {
        if (maxJewelCnt <= jewelCnt) return;


        this.transform.GetChild(0).GetChild(jewelCnt++).GetComponent<Button>().interactable = true;
        ChangeJewelCntText(); // 보석 수량 표시 텍스트 변경
    }

    public void GetRedJewel()
    {
        jewelCnt = 5;
        for (int i = 0; i < jewelCnt; i++)
        {
            this.transform.GetChild(0).GetChild(i).GetComponent<Button>().interactable = true;
        }
        ChangeJewelCntText(); // 보석 수량 표시 텍스트 변경
    }

    public void SkillBtn_On() // 유저가 스킬 버튼 누르면 플레이어가 비행하는 거 막기 위한 함수
    {
        // 유저가 스킬 버튼에 마우스 올리면
        PlayerMove.Instance.setIsSkillBtn(true);
    }

    public void SkillBtn_Off() // 유저가 스킬 버튼 누르면 플레이어가 비행하는 거 막기 위한 함수
    {
        // 유저가 스킬 버튼에서 손을 떼면
        PlayerMove.Instance.setIsSkillBtn(false);
    }


    private void ChangeJewelCntText()
    {
        // 텍스트 변경
        jewelCntText.GetComponent<TextMeshProUGUI>().text = "x" + jewelCnt;

        // 슬라이더 변경
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
        ChangeJewelCntText(); // 보석 수량 표시 텍스트 변경

        //사운드
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

        ChangeJewelCntText(); // 보석 수량 표시 텍스트 변경
    }
    public void HealSkill_OnClick()
    {
        PlayerMove.Instance.ChangeHealth();
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        ChangeJewelCntText(); // 보석 수량 표시 텍스트 변경
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
        ChangeJewelCntText(); // 보석 수량 표시 텍스트 변경
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
        ChangeJewelCntText(); // 보석 수량 표시 텍스트 변경
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
        ChangeJewelCntText(); // 보석 수량 표시 텍스트 변경
    }

    public void UI_Off()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}

# region 옵션창
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