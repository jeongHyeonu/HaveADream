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

    // 멈출 효과음
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


    // 효과음 목록
    public enum SFX_list
    {
        GOLD_GET,
        JEWEL_GET,
    }

    // 배경음 목록
    public enum BGM_list
    {
        temp_BGM,
    }

    //////////  Default Method  //////////
    // Start is called before the first frame update
    void Start()
    {
        // 리스트에 넣은 SFX audioClip 을 모두 dictionary에 저장
        for (int i = 0; i < SFX_datas.Count; i++)
        {
            if (SFX_datas[i].audio == null) continue; // 효과음 없으면 저장X
            SFX_audioclips.Add(SFX_datas[i].sfx_name, SFX_datas[i].audio);
        }
        // 리스트에 넣은 BGM audioClip 을 모두 dictionary에 저장
        for (int i = 0; i < BGM_datas.Count; i++)
        {
            if (BGM_datas[i].audio == null) continue; // 배경음 없으면 저장X
            BGM_audioclips.Add(BGM_datas[i].bgm_name, BGM_datas[i].audio);
        }
    }


    // 사운드 재생 - 배경
    public object PlayBGM(params object[] _arg)
    {
        // 사운드 이름
        BGM_list playSoundName = (BGM_list)_arg[0];

        // 사운드 객체
        GameObject soundObject = null;

        // 만약 실행중인 BGM 이 있다면 중단하고 다른 BGM으로 변경
        if (GameObject.Find("SoundManager/BGM").transform.childCount >= 1)
        {
            soundObject = GameObject.Find("SoundManager/BGM").transform.GetChild(0).gameObject;
            AudioSource audioSource = soundObject.GetComponent<AudioSource>(); // 컴포넌트 불러오기
            audioSource.clip = BGM_audioclips[playSoundName]; // 음악 불러오기
            audioSource.Play(); // 음악 재생
        }

        // 실행중인 BGM 이 없다면 사운드 오브젝트 생성 후 BGM에 장착
        else
        {
            soundObject = new GameObject("Sound");
            soundObject.transform.parent = GameObject.Find("SoundManager/BGM").transform;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // 컴포넌트 생성
            audioSource.clip = BGM_audioclips[playSoundName]; // 음악 불러오기
            audioSource.loop = true; // 반복재생
            audioSource.Play(); // 음악 재생
        }

        return true;
    }

    // 사운드 재생 - 효과음 (풀링 적용됨)
    public void PlaySFX(SFX_list _type)
    {
        // 사운드 이름
        SFX_list playSoundName = _type;

        // 효과음 풀 없으면 생성
        GameObject soundPool = GameObject.Find(playSoundName + "Pool");
        if (soundPool == null)
        {
            soundPool = new GameObject(playSoundName + "Pool");
            soundPool.transform.parent = GameObject.Find("SoundManager/SFX").transform;
        }

        // 사운드 오브젝트 선택
        GameObject soundObject;
        // 사운드 풀에 오브젝트가 없으면 새로 만들고 풀에 넣자
        if (soundPool.transform.childCount==0)
        {
            // 사운드 오브젝트 생성
            soundObject = new GameObject(playSoundName + "Sound");
            soundObject.transform.parent = soundPool.transform; // 사운드풀에 저장
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // 컴포넌트 생성
            //audioSource.clip = GetOrAddAudioClip(playSoundName, SoundType.SFX);
            audioSource.clip = SFX_audioclips[playSoundName];
            audioSource.volume = volume_SFX;
            soundObject.SetActive(false);
        }
        // 사운드 오브젝트 선택
        int idx = 0;
        while(idx<soundPool.transform.childCount)
        {
            // 사용중이지 않은 사운드 오브젝트 고를 때까지 idx 증가
            if (soundPool.transform.GetChild(idx).gameObject.activeSelf == true) idx++;
            else break;
        }
        // 만약 idx가 pool의 최대 인덱스까지 도착하면 오브젝트 새로 만들고 저장
        if (idx==soundPool.transform.childCount)
        {
            // 사운드 오브젝트 생성
            soundObject = new GameObject(playSoundName + "Sound");
            soundObject.transform.parent = soundPool.transform; // 사운드풀에 저장
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // 컴포넌트 생성
            //audioSource.clip = GetOrAddAudioClip(playSoundName, SoundType.SFX);
            audioSource.clip = SFX_audioclips[playSoundName];
            audioSource.volume = volume_SFX;
            soundObject.SetActive(false);
        }
            
        // 사운드오브젝트 오브젝트 선택 후 활성화
        soundObject = soundPool.transform.GetChild(idx).gameObject;
        soundObject.SetActive(true);

        // 사운드 재생 (Play -> PlayOneshot 으로 변경됨)
        //soundObject.GetComponent<AudioSource>().PlayOneShot(GetOrAddAudioClip(playSoundName, SoundType.SFX),volume_SFX);
        soundObject.GetComponent<AudioSource>().PlayOneShot(SFX_audioclips[playSoundName], volume_SFX * 0.1f);

        // 사운드 다 재생되면 비활성화
        StartCoroutine(soundSetActive(soundObject));
    }

    IEnumerator soundSetActive(GameObject soundObject)
    {
        // 지연시간 뒤 사운드 오브젝트 비활성화
        yield return new WaitForSeconds(soundObject.GetComponent<AudioSource>().clip.length);

        soundObject.SetActive(false);
        soundObject.transform.SetAsFirstSibling(); // 자식 앞으로 이동
        StopCoroutine(soundSetActive(null));
    } 

    // 배경 볼륨조절
    public void BGM_volumeControl()
    {
    }

    // 효과음 볼륨조절
    public void SFX_volumeControl()
    {
    }
    
    // 음악 일시정지 또는 플레이
    public void BGM_pauseOrPlay()
    {
    }


    // 음악 재생/일시정지 버튼
    public void BGM_playButton()
    {
    }

    // 지연시간 뒤 효과음 정지
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

	// 효과음 재생중인지 체크
	public object SFX_isPlaying(params object[] _arg)
    {
        SFX_list toCheckSfx = (SFX_list)_arg[0];
        if (GameObject.Find(toCheckSfx.ToString() + "Pool").transform.GetChild(0).gameObject.activeSelf == true) return true;
        else return false;
    }
}