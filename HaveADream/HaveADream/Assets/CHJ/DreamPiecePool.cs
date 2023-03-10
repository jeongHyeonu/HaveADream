using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
//������Ʈ Ǯ�� Ŭ����
public class ObjectInfo
{
    public GameObject DpPrefab;
    public int count;
    public Transform tfPoolParent;
}

public class DreamPiecePool : MonoBehaviour
{
    public static DreamPiecePool instance;

    [SerializeField] ObjectInfo[] objectInfo = null;
    [SerializeField] Transform DpAppear = null;


    //ťŸ������ ����� ���Լ���� ����
    public Queue<GameObject> DpQueue = new Queue<GameObject>();

    void Start()
    {
        instance = this;
        DpQueue = InsertQueue(objectInfo[0]);
    }

    Queue<GameObject> InsertQueue(ObjectInfo p_objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        for (int i = 0; i < p_objectInfo.count; i++)
        {
            GameObject t_clone = Instantiate(p_objectInfo.DpPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);       //�ٷ� ��Ȱ��ȭ
            if (p_objectInfo.tfPoolParent != null)
                t_clone.transform.SetParent(p_objectInfo.tfPoolParent);
            else
                t_clone.transform.SetParent(this.transform);

            /*if(t_clone==null)
            {
                Debug.Log("fuck");
            }*/

            t_queue.Enqueue(t_clone);


        }
        return t_queue;
    }
    void Update()
    {
        GameObject t_dp = DreamPiecePool.instance.DpQueue.Dequeue();
        t_dp.transform.position = DpAppear.position;
        t_dp.SetActive(true);
    }

}
