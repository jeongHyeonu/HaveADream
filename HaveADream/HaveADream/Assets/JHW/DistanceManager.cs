using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceManager : Singleton<DistanceManager>
{
    [SerializeField] public GameObject DistanceUI;
    [SerializeField] TextMeshProUGUI distance_text;
    [SerializeField] float distance = 0f;

    public bool isGamePlaying = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        isGamePlaying = true;
    }

    private void OnDisable()
    {
        isGamePlaying = false;
    }

    public void DistanceUI_ON()
    {
        this.gameObject.SetActive(true);
    }
    public void DistanceUI_OFF()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGamePlaying == false) return;
        distance += PlayerMove.Instance.GetPlayerSpeed() * Time.deltaTime * 0.001f;
        distance_text.SetText(Mathf.Round(distance).ToString());
    }
}
