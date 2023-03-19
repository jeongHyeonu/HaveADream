using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboadManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaderboardButton_OnClick()
    {
        PlayFabLogin.Instance.SendLeaderboad(2);
    }
}
