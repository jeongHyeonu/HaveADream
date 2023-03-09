using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] GameObject speedSkill;
    [SerializeField] GameObject invinsibleSkill;
    [SerializeField] GameObject healSkill;
    [SerializeField] GameObject absorbSkill;
    [SerializeField] GameObject shieldSkill;

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
        this.transform.GetChild(jewelCnt++).GetComponent<Button>().interactable = true;
    }

    public void GetRedJewel()
    {
        jewelCnt = 5;
        for(int i = 0; i < 5; i++)
        {
            this.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }

    public void SpeedSkill_OnClick()
    {
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
    }
    public void InvinsibleSkill_OnClick()
    {
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
    }
    public void HealSkill_OnClick()
    {
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
    }
    public void AbsorbSkill_OnClick()
    {
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
    }
    public void ShieldSkill_OnClick()
    {
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
        this.transform.GetChild(--jewelCnt).GetComponent<Button>().interactable = false;
    }
}
