using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : Singleton<HomeManager>
{

    private SceneManager sm = null;


    void Start()
    {
        // ΩÃ±€≈Ê πﬁæ∆ø¿±‚
        sm = SceneManager.Instance;

    }

    public void PlayButton_OnClick()
    {
        sm.Scene_Change_StageSelect();
    }
}
