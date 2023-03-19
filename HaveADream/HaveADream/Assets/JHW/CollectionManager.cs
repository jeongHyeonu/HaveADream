using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : Singleton<CollectionManager>
{
    [SerializeField] public GameObject CollectionUI;
    [SerializeField] GameObject CollectionBackground;

    // Start is called before the first frame update
    //void Start()
    //{
    //    BroadcastManager.Instance.Init_Broadcast();

    //    CollectionUI.transform.SetParent(HomeManager.Instance.transform.GetChild(0));
    //    CollectionUI.transform.localPosition = Vector2.zero;
    //    CollectionUI.transform.localScale = Vector2.one;
    //}

    public void BackgroundClick()
    {
        CollectionUI.SetActive(false);
    }

    public void OpenCollectionUI()
    {
        CollectionUI.SetActive(true);
        CollectionBackground.SetActive(true);
    }
}
