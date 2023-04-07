using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] Dictionary<BGM_list, AudioClip> BGM_audioclips = new Dictionary<BGM_list, AudioClip>();
    [SerializeField] Dictionary<SFX_list, AudioClip> SFX_audioclips = new Dictionary<SFX_list, AudioClip>();

    [SerializeField] private float volume_BGM = 1f;
    [SerializeField] private float volume_SFX = 1f;

    [SerializeField] public List<BGM_Datas> BGM_datas = new List<BGM_Datas>();
    [SerializeField] public List<SFX_Datas> SFX_datas = new List<SFX_Datas>();

    // ���� ȿ����
    private SFX_list toStopSfx;
    private BGM_list toStopBGM;

    private enum SoundType {
        BGM,
        SFX,
    }

    [System.Serializable]
    [SerializeField]
    public struct SFX_Datas
    {
        public SFX_list sfx_name;
        public AudioClip audio;
    }

    [System.Serializable]
    [SerializeField]
    public struct BGM_Datas
    {
        public BGM_list bgm_name;
        public AudioClip audio;
    }


    // ȿ���� ���
    public enum SFX_list
    {
        GOLD_GET,
        JEWEL_GET,
    }

    // ����� ���
    public enum BGM_list
    {
        temp_BGM,
    }

    //////////  Default Method  //////////
    // Start is called before the first frame update
    void Start()
    {
        // ����Ʈ�� ���� SFX audioClip �� ��� dictionary�� ����
        for (int i = 0; i < SFX_datas.Count; i++)
        {
            if (SFX_datas[i].audio == null) continue; // ȿ���� ������ ����X
            SFX_audioclips.Add(SFX_datas[i].sfx_name, SFX_datas[i].audio);
        }
        // ����Ʈ�� ���� BGM audioClip �� ��� dictionary�� ����
        for (int i = 0; i < BGM_datas.Count; i++)
        {
            if (BGM_datas[i].audio == null) continue; // ����� ������ ����X
            BGM_audioclips.Add(BGM_datas[i].bgm_name, BGM_datas[i].audio);
        }
    }


    // ���� ��� - ���
    public object PlayBGM(params object[] _arg)
    {
        // ���� �̸�
        BGM_list playSoundName = (BGM_list)_arg[0];

        // ���� ��ü
        GameObject soundObject = null;

        // ���� �������� BGM �� �ִٸ� �ߴ��ϰ� �ٸ� BGM���� ����
        if (GameObject.Find("SoundManager/BGM").transform.childCount >= 1)
        {
            soundObject = GameObject.Find("SoundManager/BGM").transform.GetChild(0).gameObject;
            AudioSource audioSource = soundObject.GetComponent<AudioSource>(); // ������Ʈ �ҷ�����
            audioSource.clip = BGM_audioclips[playSoundName]; // ���� �ҷ�����
            audioSource.Play(); // ���� ���
        }

        // �������� BGM �� ���ٸ� ���� ������Ʈ ���� �� BGM�� ����
        else
        {
            soundObject = new GameObject("Sound");
            soundObject.transform.parent = GameObject.Find("SoundManager/BGM").transform;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // ������Ʈ ����
            audioSource.clip = BGM_audioclips[playSoundName]; // ���� �ҷ�����
            audioSource.loop = true; // �ݺ����
            audioSource.Play(); // ���� ���
        }

        return true;
    }

    // ���� ��� - ȿ���� (Ǯ�� �����)
    public void PlaySFX(SFX_list _type)
    {
        // ���� �̸�
        SFX_list playSoundName = _type;

        // ȿ���� Ǯ ������ ����
        GameObject soundPool = GameObject.Find(playSoundName + "Pool");
        if (soundPool == null)
        {
            soundPool = new GameObject(playSoundName + "Pool");
            soundPool.transform.parent = GameObject.Find("SoundManager/SFX").transform;
        }

        // ���� ������Ʈ ����
        GameObject soundObject;
        // ���� Ǯ�� ������Ʈ�� ������ ���� ����� Ǯ�� ����
        if (soundPool.transform.childCount==0)
        {
            // ���� ������Ʈ ����
            soundObject = new GameObject(playSoundName + "Sound");
            soundObject.transform.parent = soundPool.transform; // ����Ǯ�� ����
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // ������Ʈ ����
            //audioSource.clip = GetOrAddAudioClip(playSoundName, SoundType.SFX);
            audioSource.clip = SFX_audioclips[playSoundName];
            audioSource.volume = volume_SFX;
            soundObject.SetActive(false);
        }
        // ���� ������Ʈ ����
        int idx = 0;
        while(idx<soundPool.transform.childCount)
        {
            // ��������� ���� ���� ������Ʈ �� ������ idx ����
            if (soundPool.transform.GetChild(idx).gameObject.activeSelf == true) idx++;
            else break;
        }
        // ���� idx�� pool�� �ִ� �ε������� �����ϸ� ������Ʈ ���� ����� ����
        if (idx==soundPool.transform.childCount)
        {
            // ���� ������Ʈ ����
            soundObject = new GameObject(playSoundName + "Sound");
            soundObject.transform.parent = soundPool.transform; // ����Ǯ�� ����
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // ������Ʈ ����
            //audioSource.clip = GetOrAddAudioClip(playSoundName, SoundType.SFX);
            audioSource.clip = SFX_audioclips[playSoundName];
            audioSource.volume = volume_SFX;
            soundObject.SetActive(false);
        }
            
        // ���������Ʈ ������Ʈ ���� �� Ȱ��ȭ
        soundObject = soundPool.transform.GetChild(idx).gameObject;
        soundObject.SetActive(true);

        // ���� ��� (Play -> PlayOneshot ���� �����)
        //soundObject.GetComponent<AudioSource>().PlayOneShot(GetOrAddAudioClip(playSoundName, SoundType.SFX),volume_SFX);
        soundObject.GetComponent<AudioSource>().PlayOneShot(SFX_audioclips[playSoundName], volume_SFX * 0.1f);

        // ���� �� ����Ǹ� ��Ȱ��ȭ
        StartCoroutine(soundSetActive(soundObject));
    }

    IEnumerator soundSetActive(GameObject soundObject)
    {
        // �����ð� �� ���� ������Ʈ ��Ȱ��ȭ
        yield return new WaitForSeconds(soundObject.GetComponent<AudioSource>().clip.length);

        soundObject.SetActive(false);
        soundObject.transform.SetAsFirstSibling(); // �ڽ� ������ �̵�
        StopCoroutine(soundSetActive(null));
    } 

    // ��� ��������
    public void BGM_volumeControl()
    {
    }

    // ȿ���� ��������
    public void SFX_volumeControl()
    {
    }
    
    // ���� �Ͻ����� �Ǵ� �÷���
    public void BGM_pauseOrPlay()
    {
    }


    // ���� ���/�Ͻ����� ��ư
    public void BGM_playButton()
    {
    }

    // �����ð� �� ȿ���� ����
    public object SFX_stop(params object[] _arg)
    {
        return true;
    }
    public void sfxStop()
    {
        GameObject.Find(toStopSfx.ToString() + "Pool").transform.GetChild(0).gameObject.SetActive(false);
    }

	private void BgmStop()
	{
		GameObject.Find("Sound").transform.gameObject.SetActive(false);
	}

	public object BGM_stop(params object[] _arg)
	{
		toStopBGM = (BGM_list)_arg[0];
		float playtime = 1.0f;
		Invoke("BgmStop", playtime);
		return true;
	}

	// ȿ���� ��������� üũ
	public object SFX_isPlaying(params object[] _arg)
    {
        SFX_list toCheckSfx = (SFX_list)_arg[0];
        if (GameObject.Find(toCheckSfx.ToString() + "Pool").transform.GetChild(0).gameObject.activeSelf == true) return true;
        else return false;
    }
}