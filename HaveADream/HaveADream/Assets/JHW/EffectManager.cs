using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

# region 파티클
partial class EffectManager : Singleton<EffectManager>
{
    public enum particle_list{
        FallParticle,
        MovingParticle,
        FlyEffectParticle1,
        FlyEffectParticle2,
        FlyEffectParticle3,
    }

    [System.Serializable]
    [SerializeField]
    public struct particle_Data
    {
        public particle_list particle_name;
        public ParticleSystem particle_source;
    }

    [SerializeField] public List<particle_Data> particleList;
    private Dictionary<particle_list, ParticleSystem> particleDictionary = new Dictionary<particle_list, ParticleSystem>();


    void Start()
    {
        // 인스펙터에서 넣은 이펙트들 다 딕셔너리로 넣기

        // 파티클
        for(int i=0;i< particleList.Count; i++)
        {
            particleDictionary.Add(particleList[i].particle_name, particleList[i].particle_source);
            particleList[i].particle_source.Stop();
        }

        // VFX
        for (int i = 0; i < VFXList.Count; i++)
        {
            VFXDictionary.Add(VFXList[i].VFX_name, VFXList[i].VFX_pool);
        }
    }

    public void PlayParticle(particle_list _type)
    {
        // 딕셔너리에서 이펙트 찾고, 재생
        particleDictionary[_type].Play(); //.gameObject.SetActive(true);
    }

    public void StopParticle(particle_list _type)
    {
        // 딕셔너리에서 이펙트 찾고, 멈춤
        particleDictionary[_type].Stop(); //.gameObject.SetActive(false);
    }

    public GameObject GetParticleObject(particle_list _type)
    {
        // 딕셔너리에서 이펙트 찾고, 리턴
        return particleDictionary[_type].gameObject;
    }
}
#endregion

#region VFX
partial class EffectManager
{
    public enum VFX_list
    {
        FLASH1,
        SPARK1,
    }

    [System.Serializable]
    [SerializeField]
    public struct VFX_Data
    {
        public VFX_list VFX_name;
        public GameObject VFX_pool;
    }

    [SerializeField] public List<VFX_Data> VFXList;
    private Dictionary<VFX_list, GameObject> VFXDictionary = new Dictionary<VFX_list, GameObject>();


    public void PlayVFX(VFX_list _type,GameObject _target)
    {
        // 딕셔너리에서 VFX pool 찾고, 안에서 비활성화된 VFX 찾아서 재생
        GameObject targetPool = VFXDictionary[_type];
        for (int i = 0; i < 10; i++)
        {
            if (targetPool.transform.GetChild(i).gameObject.activeSelf == false)
            {
                targetPool.transform.GetChild(i).gameObject.SetActive(true);
                targetPool.transform.GetChild(i).gameObject.transform.position = _target.transform.position;
                //targetPool.transform.GetChild(i).gameObject.GetComponent<VisualEffect>().Play();
                return;
            }
        }
    }
}
#endregion