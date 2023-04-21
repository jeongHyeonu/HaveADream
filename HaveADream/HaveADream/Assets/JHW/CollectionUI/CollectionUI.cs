using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUI : MonoBehaviour
{
    [SerializeField] List<GameObject> animalList;

    private void OnEnable()
    {
        List<Episode1Data> epi1 = UserDataManager.Instance.GetUserData_userEpi1Data();
        List<Episode2Data> epi2 = UserDataManager.Instance.GetUserData_userEpi2Data();

        if (epi1[epi1.Count-1].star == 3) OpenAnimal(animalList[0]);
        if (epi2[epi2.Count-1].star == 3) OpenAnimal(animalList[0]);
    }

    private void OpenAnimal(GameObject target)
    {
        target.transform.gameObject.GetComponent<Image>().color = new Color(.9f, .9f, .9f, 1);
        target.transform.GetChild(1).gameObject.SetActive(false);
        target.transform.GetChild(2).gameObject.SetActive(true);
    }
}
