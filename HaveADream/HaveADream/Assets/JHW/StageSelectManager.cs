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

        
        public void EpisodeButton_OnClick([SerializeField] int episodeNumber) // ���Ǽҵ� Ŭ����
        {
            this.transform.GetChild(episodeNumber).gameObject.SetActive(true);
        }

        
        public void WorldButton_OnClick([SerializeField] int currentEpisodeNumber) // ����� ���ư��� ��ư Ŭ����
        {
            this.transform.GetChild(currentEpisodeNumber).gameObject.SetActive(false);
        }

        public void StageButton_OnClick([SerializeField] int currentStageNumber) // �������� ��ư Ŭ����
        {
            // ���� �����Ϳ��� ��Ʈ �� ���ҽ�Ű�� ������ ���� �� UI ����
            UserDataManager.Instance.SetUserData_heart(-1);
            UserDataManager.Instance.SaveData();
            UIGroupManager.Instance.ChangeHeartUI();

            // �����÷��̷� ��ȯ
            sm.Scene_Change_GamePlay();
        }
    }
}