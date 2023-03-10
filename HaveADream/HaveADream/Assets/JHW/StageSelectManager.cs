using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JHW
{
    public class StageSelectManager : Singleton<StageSelectManager>
    {
        private SceneManager sm = null;

        // Start is called before the first frame update
        void Start()
        {
            // 싱글톤 받아오기
            sm = SceneManager.Instance;
        }

        public void HomeButton_OnClick() // 홈 화면으로 복귀
        {
            sm.Scene_Change_Home();
        }

        
        public void EpisodeButton_OnClick([SerializeField] int episodeNumber) // 에피소드 클릭시
        {
            this.transform.GetChild(episodeNumber).gameObject.SetActive(true);
        }

        
        public void WorldButton_OnClick([SerializeField] int currentEpisodeNumber) // 월드로 돌아가기 버튼 클릭시
        {
            this.transform.GetChild(currentEpisodeNumber).gameObject.SetActive(false);
        }

        public void StageButton_OnClick([SerializeField] int currentStageNumber) // 스테이지 버튼 클릭시
        {
            // 유저 데이터에서 하트 수 감소시키고 데이터 저장 후 UI 변경
            UserDataManager.Instance.SetUserData_heart(-1);
            UserDataManager.Instance.SaveData();
            UIGroupManager.Instance.ChangeHeartUI();

            // 게임플레이로 전환
            sm.Scene_Change_GamePlay();
        }
    }
}