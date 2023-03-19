using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : Singleton<SkillManager>
{
    SpriteRenderer sr;
    [SerializeField] private Color newColor; // 변경할 색상

    [SerializeField] GameObject speedSkill;
    [SerializeField] GameObject invinsibleSkill;
    [SerializeField] GameObject healSkill;
    [SerializeField] GameObject absorbSkill;
    [SerializeField] GameObject shieldSkill;

    [SerializeField] GameObject jewelCntText;

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
        jewelCntText.GetComponent<TextMeshProUGUI>().text = jewelCnt + " / " + maxJewelCnt;
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
    }
    private void InvinsibleSkill_RollBack()
    {
        Player.layer = 20;
        MapMove.Instance.mapSpeed = 5f;
        Renderer renderer = Player.GetComponent<Renderer>(); // Renderer 컴포넌트 가져오기
        renderer.material.color = newColor; // 색상 변경

        PlayerMove.Instance.isInvisible = false;


    }

    public void InvinsibleSkill_OnClick()
    {
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        PlayerMove.Instance.ChangeInvisible();
        Invoke("InvinsibleSkill_RollBack", 1f);

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
        Invoke("AbsorbSkill_RollBack", 3f);
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(0).GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        ChangeJewelCntText(); // 보석 수량 표시 텍스트 변경
    }

    private void ShieldSkill_RollBack()
    {
        Player.layer = 20;
        Renderer renderer = Player.GetComponent<Renderer>(); // Renderer 컴포넌트 가져오기
        renderer.material.color = newColor; // 색상 변경
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
