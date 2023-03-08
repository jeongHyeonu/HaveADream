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

        [SerializeField]
        public void EpisodeButton_OnClick(int episodeNumber) // 에피소드 클릭시
        {
            this.transform.GetChild(episodeNumber).gameObject.SetActive(true);
        }

        [SerializeField]
        public void WorldButton_OnClick(int currentEpisodeNumber) // 월드로 돌아가기 버튼 클릭시
        {
            this.transform.GetChild(currentEpisodeNumber).gameObject.SetActive(false);
        }
    }
}