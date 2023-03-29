using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXeffect : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("autoDisable", 1f);
    }

    void autoDisable()
    {
        this.gameObject.SetActive(false);
    }
}
