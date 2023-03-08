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
            // �̱��� �޾ƿ���
            sm = SceneManager.Instance;
        }

        public void HomeButton_OnClick() // Ȩ ȭ������ ����
        {
            sm.Scene_Change_Home();
        }

        [SerializeField]
        public void EpisodeButton_OnClick(int episodeNumber) // ���Ǽҵ� Ŭ����
        {
            this.transform.GetChild(episodeNumber).gameObject.SetActive(true);
        }

        [SerializeField]
        public void WorldButton_OnClick(int currentEpisodeNumber) // ����� ���ư��� ��ư Ŭ����
        {
            this.transform.GetChild(currentEpisodeNumber).gameObject.SetActive(false);
        }
    }
}