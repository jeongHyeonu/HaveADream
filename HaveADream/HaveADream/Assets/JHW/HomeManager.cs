using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

partial class HomeManager : Singleton<HomeManager>
{

    private SceneManager sm = null;
    [SerializeField] GameObject Animals = null;
    [SerializeField] GameObject AnimalsBackground = null;

    void Start()
    {
        // 싱글톤 받아오기
        sm = SceneManager.Instance;
        // 음악 재생
        SoundManager.Instance.PlayBGM(SoundManager.BGM_list.Home_BGM);
    }

    public void PlayButton_OnClick()
    {
        sm.Scene_Change_StageSelect();
    }




    public void Home_OpenAnials()
    {
        // 해금한 동물 오픈

        // Epi 1 정보 불러와서 검사
        List<Episode1Data> epi1 = UserDataManager.Instance.GetUserData_userEpi1Data();
        if (epi1[0].isClearStage == false) return; // 에피1-1 클리어 안했다면 실행X (에피1-1 클리어 시 오픈)
        Animals.transform.GetChild(1).gameObject.SetActive(true);
        if (epi1[12].isClearStage == true) // 에피1-13 (에피1 마지막스테이지까지) 클리어했다면
        {
            Animals.transform.GetChild(1).GetChild(0).gameObject.SetActive(true); // 동물 이미지 활성화
            Animals.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "사자"; // 텍스트 내용 변경
        }
        else
        {
            int progressNum = 1;
            for (int i = 1; i < epi1.Count; i++)
            {
                if (epi1[i].isClearStage) progressNum++;
                else
                {
                    Animals.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<size=10>스테이지 1 클리어 시 해금</size>\n진행도 : " + progressNum + "/" + epi1.Count; // 텍스트 내용 변경
                    return;
                }
            }
        }

        // Epi 2 정보 불러와서 검사
        List<Episode2Data> epi2 = UserDataManager.Instance.GetUserData_userEpi2Data();
        if (epi2[0].isClearStage == false) return; // 에피2-1 클리어 안했다면 실행X (에피2-1 클리어 시 오픈)
        Animals.transform.GetChild(2).gameObject.SetActive(true);
        if (epi2[17].isClearStage == true) // 에피2-18 (에피2 마지막스테이지까지) 클리어했다면
        {
            Animals.transform.GetChild(2).GetChild(0).gameObject.SetActive(true); // 동물 이미지 활성화
            Animals.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "물범"; // 텍스트 내용 변경
        }
        else
        {
            int progressNum = 1;
            for (int i = 1; i < epi2.Count; i++)
            {
                if (epi2[i].isClearStage) progressNum++;
                else
                {
                    Animals.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<size=10>스테이지 2 클리어 시 해금</size>\n진행도 : " + progressNum + "/" + epi2.Count; // 텍스트 내용 변경
                    return;
                }
            }
        }
    }
}

#region Home - Button
partial class HomeManager : Singleton<HomeManager>
{
    int curAnimalNumber = 0;
    GameObject curAnimal; // 현재 클릭한 동물 오브젝트
    Vector2 tempVec2;
    bool _isClicked = false;
    float UX_Duration = 0.5f;

    public void Click_Animal([SerializeField] GameObject AnimalObject) // 동물(미해금시 "?" 아이콘)클릭시 그 아이콘 확대
    {
        // 뒷배경 활성화상태면 실행X
        if (AnimalsBackground.activeSelf == true) return;

        // 0.5초뒤 배경 누를 수 있게 활성화
        Invoke("Background_Interactable_true", .5f);

        // 동물 클릭시
        //curAnimal = AnimalObject; // 현재 선택한 동물 오브젝트
        //curAnimal.transform.SetAsLastSibling(); // 계층 순서 변경
        //curAnimal.transform.GetChild(1).gameObject.SetActive(true); // 텍스트 활성화
        //curAnimal.transform.DOScale(3f, UX_Duration); // 동물 이미지 크기 크게
        //AnimalsBackground.gameObject.SetActive(true); // 동물 클릭시 뒷배경
        //AnimalsBackground.GetComponent<Image>()
        //    .DOFade(0.2f, UX_Duration)
        //    .OnStart( ()=> { AnimalsBackground.GetComponent<Image>().color = new Color(0, 0, 0, 0f); } );

        //// 동물 중앙 확대, 이때 tempVec2에 동물위치 임시저장 후 뒷배경클릭시 원래장소로 이동
        //tempVec2 = curAnimal.transform.position;
        //float height = 2 * Camera.main.orthographicSize;
        //float width = height * Camera.main.aspect;
        //curAnimal.transform.DOLocalMove(new Vector2(width / 2, height / 2), UX_Duration);

        curAnimal = AnimalObject; // 현재 선택한 동물 오브젝트
        curAnimal.transform.GetChild(1).gameObject.SetActive(true); // 텍스트 활성화
        Animals.transform.DOLocalMove(new Vector2(-curAnimal.transform.localPosition.x, -curAnimal.transform.localPosition.y), UX_Duration); // UI canvas 이동
        AnimalsBackground.gameObject.SetActive(true); // 동물 클릭시 뒷배경
        curAnimal.transform.DOScale(1.4f, UX_Duration); // 동물 이미지 크기 크게

        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.Button);
    }

    public void Click_AnimalBackground() // 동물 확대된 후 뒷배경 클릭시
    {
        // 동물 밖 배경 연속으로 누를 수 있어서 체크
        if (_isClicked == true) return;
        _isClicked = true;

        // 0.5초뒤 배경 누를 수 있게 비활성화
        Invoke("Background_Interactable_false", .5f);

        // 동물 클릭 후 뒷배경 클릭시
        curAnimal.transform.GetChild(1).gameObject.SetActive(false); // 텍스트 비활성화
        AnimalsBackground.GetComponent<Image>().DOFade(0f, UX_Duration).OnComplete(() => { AnimalsBackground.gameObject.SetActive(false); } );

        // 원래 좌표로 이동 및 크기 작게
        //curAnimal.transform.DOMove(tempVec2, UX_Duration);
        Animals.transform.DOLocalMove(Vector2.zero, UX_Duration); // UI canvas 이동
        curAnimal.transform.DOScale(1f, UX_Duration);
    }

    private void Background_Interactable_true()
    {
        AnimalsBackground.GetComponent<Button>().interactable = true;
    }

    private void Background_Interactable_false()
    {
        AnimalsBackground.GetComponent<Button>().interactable = false;
        _isClicked = false;
    }
}
#endregion